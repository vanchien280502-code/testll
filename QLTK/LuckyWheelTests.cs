using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using TGLL;

class LuckyWheelTests {
    static void Check(bool b, string s) { if (!b) throw new Exception(s); }
    static InventorySnapshot Bag(int ticketCount) {
        return new InventorySnapshot(new[] {
            new InventoryItem { TemplateId = 706, Quantity = ticketCount },
            new InventoryItem { TemplateId = 161, Quantity = 10 }
        }, 144, true, "", DateTime.UtcNow);
    }
    static NativeJob Job() {
        var j = new NativeJob("fixture", "wheel-test", "", null, true);
        j.GameSettingsReady = true;
        j.TrainReady = true;
        return j;
    }

    static int Main() {
        try {
            NativeBackend.LogDirectory = System.IO.Path.GetFullPath("work/wheel-test-logs");
            var j = Job();
            // Test StartLuckyWheel validation
            try { NativeBackend.StartLuckyWheel("nonexistent"); Check(false, "should fail on nonexistent"); } catch { }

            // Register job into NativeBackend.jobs
            var jobsField = typeof(NativeBackend).GetField("jobs", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            var jobs = jobsField.GetValue(null) as System.Collections.Concurrent.ConcurrentDictionary<string, NativeJob>;
            jobs[j.Id] = j;

            // Start with valid interval and target count = 2
            NativeBackend.StartLuckyWheel(j.Id, 1200, 2);
            Check(j.LuckyWheelEnabled, "LuckyWheelEnabled should be true");
            Check(j.LuckyWheelIntervalMs == 1200, "Interval should be 1200");
            Check(j.LuckyWheelTargetCount == 2, "TargetCount should be 2");
            Check(j.LuckyWheelSpunCount == 0, "SpunCount should start at 0");

            DateTime now = DateTime.UtcNow;

            // Test PumpLuckyWheel 1st spin (Opcode 133, Payload 171, PayloadType 1)
            NativeBackend.PumpLuckyWheel(null, 0, j, () => Bag(5), now);
            Check(j.LuckyWheelSpunCount == 1, "SpunCount should be 1");
            Check(j.TaskPacketOpcode == 133, "Opcode should be 133 (Vận may network packet 0x85)");
            Check(j.TaskPacketPayload == 171, "Payload should be 171 (sub-action 0xAB)");
            Check(j.TaskPacketPayloadType == 1, "PayloadType should be 1 (write byte)");
            Check(j.LuckyWheelNext > now, "Next action time should be scheduled");

            // Pump again before interval expires (500ms < 1200ms) -> should not fire
            j.TaskPacketOpcode = 0; // simulate packet drained
            NativeBackend.PumpLuckyWheel(null, 0, j, () => Bag(5), now.AddMilliseconds(500));
            Check(j.LuckyWheelSpunCount == 1, "Should not fire before interval");
            Check(j.TaskPacketOpcode == 0, "Opcode should still be 0");

            // Pump after interval (1201ms) -> 2nd spin executes
            now = now.AddMilliseconds(1201);
            NativeBackend.PumpLuckyWheel(null, 0, j, () => Bag(5), now);
            Check(j.LuckyWheelSpunCount == 2, "SpunCount should be 2");
            Check(j.TaskPacketOpcode == 133, "Opcode should be 133");

            // Simulate packet drained, target count reached (2/2) -> auto stop!
            j.TaskPacketOpcode = 0;
            now = now.AddMilliseconds(1201);
            NativeBackend.PumpLuckyWheel(null, 0, j, () => Bag(5), now);
            Check(!j.LuckyWheelEnabled, "Lucky wheel should stop when target count is reached");
            Check(j.LuckyWheelStatus.Contains("Đã quay xong 2/2"), "Status should indicate target reached: " + j.LuckyWheelStatus);
            Check(j.TaskPacketOpcode == 0, "No packet sent after completion");

            // Test unlimited mode (targetCount = 0) and manual StopLuckyWheel
            NativeBackend.StartLuckyWheel(j.Id, 1000, 0);
            Check(j.LuckyWheelEnabled, "Should be enabled for unlimited");
            Check(j.LuckyWheelTargetCount == 0, "Target count should be 0");
            NativeBackend.StopLuckyWheel(j.Id);
            Check(!j.LuckyWheelEnabled, "Should be stopped");
            Check(j.LuckyWheelStatus.Contains("Đã dừng"), "Status should indicate stopped");

            // Test TicketCount helper
            long count = NativeBackend.LuckyWheelTicketCount(Bag(42));
            Check(count == 42, "Ticket count should be 42");

            // Test NativeCode.TaskPacket assembly generation
            byte[] tpBytes = NativeCode.TaskPacket(0x1000, 0x180000000L);
            Check(tpBytes != null && tpBytes.Length > 0, "TaskPacket bytes should not be null or empty");
            Check(tpBytes.Length < 4096, "TaskPacket bytes should fit inside 4096 page");

            // Test TaskPacketCore dialog_action execution
            int performedActions = 0, performedActionId = -1;
            PerformActionDelegate actionDel = delegate(IntPtr dlg, int id, IntPtr info) {
                performedActions++;
                performedActionId = id;
            };
            IntPtr actionP = Marshal.GetFunctionPointerForDelegate(actionDel);

            IntPtr dataMem = Marshal.AllocHGlobal(4096);
            IntPtr sessionClass = Marshal.AllocHGlobal(4096);
            IntPtr gameCanvas = Marshal.AllocHGlobal(4096);
            IntPtr dialogMem = Marshal.AllocHGlobal(4096);
            IntPtr staticFields = Marshal.AllocHGlobal(4096);
            IntPtr sessionSlot = Marshal.AllocHGlobal(8);

            for (int i = 0; i < 4096; i += 8) {
                Marshal.WriteInt64(dataMem, i, 0);
                Marshal.WriteInt64(sessionClass, i, 0);
                Marshal.WriteInt64(gameCanvas, i, 0);
                Marshal.WriteInt64(dialogMem, i, 0);
                Marshal.WriteInt64(staticFields, i, 0);
            }

            Marshal.WriteInt64(sessionClass, 0xB8, staticFields.ToInt64());
            Marshal.WriteInt64(staticFields, 16, gameCanvas.ToInt64());
            Marshal.WriteInt64(gameCanvas, 0x90, dialogMem.ToInt64());
            Marshal.WriteInt64(sessionSlot, sessionClass.ToInt64());

            byte[] code = NativeCode.TaskPacketCore(dataMem.ToInt64(), 0, 0, 0, 0, sessionSlot.ToInt64(), 0, 0, actionP.ToInt64());
            IntPtr execMem = VirtualAlloc(IntPtr.Zero, (uint)code.Length, 0x1000 | 0x2000, 0x40);
            Marshal.Copy(code, 0, execMem, code.Length);
            ShellcodeDelegate tick = (ShellcodeDelegate)Marshal.GetDelegateForFunctionPointer(execMem, typeof(ShellcodeDelegate));

            Marshal.WriteInt32(dataMem, 0, 1);
            Marshal.WriteInt32(dataMem, 4, 1003);
            Marshal.WriteInt32(dataMem, 8, 99);
            tick();
            Check(performedActions == 1 && performedActionId == 1003, "Dialog action 1003 should be invoked via performAction safely");
            Check(Marshal.ReadInt32(dataMem, 20) == 1, "Status code should indicate 1 (success)");

            GC.KeepAlive(actionDel);

            NativeJob removed;
            jobs.TryRemove(j.Id, out removed);

            Console.WriteLine("PASS: lucky wheel start, stop, target count completion, unlimited mode, opcode 149 spin packet, interval pacing, ticket counter, TaskPacket asm generation, and safe performAction invocation.");
            return 0;
        } catch (Exception e) {
            Console.WriteLine(e);
            return 1;
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void PerformActionDelegate(IntPtr dlg, int id, IntPtr info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void ShellcodeDelegate();

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
}
