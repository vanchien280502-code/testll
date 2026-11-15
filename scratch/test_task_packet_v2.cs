using System;
using System.Collections.Generic;

class Code64 {
    internal readonly List<byte> Bytes = new List<byte>();
    internal readonly Dictionary<string, int> Labels = new Dictionary<string, int>();
    internal readonly List<Tuple<int, string, int>> Fixups = new List<Tuple<int, string, int>>();
    internal void Emit(params byte[] b) { Bytes.AddRange(b); }
    internal void Q(long v) { Bytes.AddRange(BitConverter.GetBytes(v)); }
    internal void I(int v) { Bytes.AddRange(BitConverter.GetBytes(v)); }
    internal void Rax(long v) { Emit(0x48, 0xb8); Q(v); }
    internal void Rcx(long v) { Emit(0x48, 0xb9); Q(v); }
    internal void Call(long v) { Emit(0x48, 0xb8); Q(v); Emit(0xff, 0xd0); }
    internal void Label(string n) { Labels[n] = Bytes.Count; }
    internal void Jump(string n) { Emit(0xe9); Fixups.Add(Tuple.Create(Bytes.Count, n, 4)); Emit(0, 0, 0, 0); }
    internal void J(byte op, string n) { Emit(0x0f, op); Fixups.Add(Tuple.Create(Bytes.Count, n, 4)); Emit(0, 0, 0, 0); }
    internal byte[] Finish() {
        foreach (var f in Fixups) {
            int target = Labels[f.Item2];
            int rel = target - (f.Item1 + f.Item3);
            byte[] b = BitConverter.GetBytes(rel);
            for (int i = 0; i < f.Item3; i++) Bytes[f.Item1 + i] = b[i];
        }
        return Bytes.ToArray();
    }
}

class Program {
    static byte[] TaskPacketCore(long data, long serviceSlot, long allocate, long ctor, long initialize, long sessionSlot, long sendMessageThunk, long ensureClassInit) {
        Code64 a = new Code64();
        // push rbx; push rsi; sub rsp, 0x28; mov rbx, data
        a.Emit(0x53, 0x56, 0x48, 0x83, 0xec, 0x28, 0x48, 0xbb); a.Q(data);
        // cmp dword ptr [rbx], 1; jne return
        a.Emit(0x83, 0x3b, 1); a.J(0x85, "return");
        // mov dword ptr [rbx], 2 (in flight)
        a.Emit(0xc7, 0x03, 2, 0, 0, 0);
        // mov dword ptr [rbx + 20], 1 (started)
        a.Emit(0xc7, 0x43, 20, 1, 0, 0, 0);

        // 1. Resolve serviceSlot (Message class)
        string msgReady = "msg_ready_" + a.Bytes.Count;
        a.Rax(serviceSlot);
        a.Emit(0x48, 0x8b, 0x00); // mov rax, [rax]
        a.Emit(0x48, 0x85, 0xc0); a.J(0x84, "err_102"); // test rax, rax; jz err_102
        a.Emit(0xa8, 0x01); a.J(0x84, msgReady); // test al, 1; jz msgReady
        a.Rcx(serviceSlot); a.Call(initialize); // mov rcx, serviceSlot; call initialize
        a.Rax(serviceSlot); a.Emit(0x48, 0x8b, 0x00); // mov rax, [rax]
        a.Emit(0x48, 0x85, 0xc0); a.J(0x84, "err_102");
        a.Emit(0xa8, 0x01); a.J(0x85, "err_102");
        a.Label(msgReady);
        a.Emit(0xc7, 0x43, 20, 2, 0, 0, 0); // step 2

        // 2. Allocate Message instance
        a.Emit(0x48, 0x89, 0xc1); a.Call(allocate); // mov rcx, rax; call allocate
        a.Emit(0x48, 0x85, 0xc0); a.J(0x84, "err_103"); // test rax, rax; jz err_103
        a.Emit(0x48, 0x89, 0xc6); // mov rsi, rax (rsi = Message)
        a.Emit(0xc7, 0x43, 20, 3, 0, 0, 0); // step 3

        // 3. Constructor Message(sbyte opcode)
        a.Emit(0x48, 0x89, 0xf1); // mov rcx, rsi
        a.Emit(0x8b, 0x53, 4);    // mov edx, [rbx + 4] (opcode)
        a.Emit(0x45, 0x33, 0xc0); // xor r8d, r8d
        a.Call(ctor);
        a.Emit(0xc7, 0x43, 20, 4, 0, 0, 0); // step 4

        // 4. Payload serialization
        a.Emit(0x83, 0x7b, 8, 0); a.J(0x84, "send"); // cmp dword [rbx+8], 0; jz send
        a.Emit(0x83, 0x7b, 8, 2); a.J(0x84, "write_short");
        a.Emit(0x83, 0x7b, 8, 3); a.J(0x84, "turnin_item");
        a.Emit(0x83, 0x7b, 8, 4); a.J(0x84, "two_shorts");
        a.Emit(0x83, 0x7b, 8, 5); a.J(0x84, "short_byte");

        // default / type 1: write_byte
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x30, 2, 0, 0, 0x48, 0x89, 0xf1, 0x8b, 0x53, 12, 0xff, 0x90, 0x28, 2, 0, 0);
        a.Jump("send");

        a.Label("write_short");
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x50, 2, 0, 0, 0x48, 0x89, 0xf1, 0x8b, 0x53, 12, 0xff, 0x90, 0x48, 2, 0, 0);
        a.Jump("send");

        a.Label("turnin_item");
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x30, 2, 0, 0, 0x48, 0x89, 0xf1, 0xba, 1, 0, 0, 0, 0xff, 0x90, 0x28, 2, 0, 0);
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x50, 2, 0, 0, 0x48, 0x89, 0xf1, 0x8b, 0x53, 12, 0xff, 0x90, 0x48, 2, 0, 0);
        a.Jump("send");

        a.Label("two_shorts");
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x50, 2, 0, 0, 0x48, 0x89, 0xf1, 0x8b, 0x53, 12, 0xff, 0x90, 0x48, 2, 0, 0);
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x50, 2, 0, 0, 0x48, 0x89, 0xf1, 0x8b, 0x53, 16, 0xff, 0x90, 0x48, 2, 0, 0);
        a.Jump("send");

        a.Label("short_byte");
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x50, 2, 0, 0, 0x48, 0x89, 0xf1, 0x8b, 0x53, 12, 0xff, 0x90, 0x48, 2, 0, 0);
        a.Emit(0x48, 0x8b, 0x06, 0x4c, 0x8b, 0x80, 0x20, 2, 0, 0, 0x48, 0x89, 0xf1, 0x8b, 0x53, 16, 0xff, 0x90, 0x18, 2, 0, 0);

        a.Label("send");
        a.Emit(0xc7, 0x43, 20, 5, 0, 0, 0); // step 5

        // 5. Session resolution and Send:
        string sessReady = "sess_ready_" + a.Bytes.Count;
        a.Rax(sessionSlot);
        a.Emit(0x48, 0x8b, 0x00); // mov rax, [rax]
        a.Emit(0x48, 0x85, 0xc0); a.J(0x84, "err_106");
        a.Emit(0xa8, 0x01); a.J(0x84, sessReady);
        a.Rcx(sessionSlot); a.Call(initialize);
        a.Rax(sessionSlot); a.Emit(0x48, 0x8b, 0x00);
        a.Emit(0x48, 0x85, 0xc0); a.J(0x84, "err_106");
        a.Emit(0xa8, 0x01); a.J(0x85, "err_106");
        a.Label(sessReady);
        a.Emit(0xc7, 0x43, 20, 6, 0, 0, 0); // step 6

        // Ensure runtime class is initialized (call ensureClassInit = 0x48F0)
        a.Emit(0x48, 0x89, 0xc1); // mov rcx, rax (klass)
        a.Call(ensureClassInit);
        a.Emit(0xc7, 0x43, 20, 7, 0, 0, 0); // step 7

        // Read static_fields
        a.Rax(sessionSlot); a.Emit(0x48, 0x8b, 0x00); // mov rax, [sessionSlot]
        a.Emit(0x48, 0x8b, 0x80, 0xb8, 0, 0, 0); // mov rax, [rax + 0xB8]
        a.Emit(0x48, 0x85, 0xc0); a.J(0x84, "err_108");
        a.Emit(0xc7, 0x43, 20, 8, 0, 0, 0); // step 8

        // Read session instance at [static_fields + 0x18]
        a.Emit(0x4c, 0x8b, 0x40, 0x18); // mov r8, [rax + 0x18]
        a.Emit(0x4d, 0x85, 0xc0); a.J(0x84, "err_109");
        a.Emit(0xc7, 0x43, 20, 9, 0, 0, 0); // step 9

        // Call sendMessageThunk (0x4340)
        a.Emit(0xb9, 0x14, 0, 0, 0); // mov ecx, 0x14
        a.Emit(0x48, 0x89, 0xf2);    // mov rdx, rsi (Message)
        a.Emit(0xc7, 0x43, 20, 10, 0, 0, 0); // step 10
        a.Call(sendMessageThunk);
        a.Emit(0xc7, 0x43, 20, 11, 0, 0, 0); // step 11

        a.Label("cleanup");
        // Message.cleanup ([rsi vtable + 0x298])
        a.Emit(0x48, 0x8b, 0x06); // mov rax, [rsi]
        a.Emit(0x48, 0x8b, 0x90, 0xa0, 2, 0, 0); // mov rdx, [rax + 0x2A0]
        a.Emit(0x48, 0x89, 0xf1); // mov rcx, rsi
        a.Emit(0xff, 0x90, 0x98, 2, 0, 0); // call [rax + 0x298]
        a.Emit(0xc7, 0x43, 20, 12, 0, 0, 0); // step 12
        a.Emit(0xc7, 0x03, 0, 0, 0, 0); // mov dword [rbx], 0 (done!)
        a.Jump("return");

        a.Label("err_102");
        a.Emit(0xc7, 0x43, 20, 102, 0, 0, 0);
        a.Emit(0xc7, 0x03, 3, 0, 0, 0);
        a.Jump("return");

        a.Label("err_103");
        a.Emit(0xc7, 0x43, 20, 103, 0, 0, 0);
        a.Emit(0xc7, 0x03, 3, 0, 0, 0);
        a.Jump("return");

        a.Label("err_106");
        a.Emit(0xc7, 0x43, 20, 106, 0, 0, 0);
        a.Jump("cleanup_err");

        a.Label("err_108");
        a.Emit(0xc7, 0x43, 20, 108, 0, 0, 0);
        a.Jump("cleanup_err");

        a.Label("err_109");
        a.Emit(0xc7, 0x43, 20, 109, 0, 0, 0);
        a.Jump("cleanup_err");

        a.Label("cleanup_err");
        a.Emit(0x48, 0x85, 0xf6); a.J(0x84, "mark_failed"); // test rsi, rsi; jz mark_failed
        a.Emit(0x48, 0x8b, 0x06, 0x48, 0x8b, 0x90, 0xa0, 2, 0, 0, 0x48, 0x89, 0xf1, 0xff, 0x90, 0x98, 2, 0, 0);
        a.Label("mark_failed");
        a.Emit(0xc7, 0x03, 3, 0, 0, 0);

        a.Label("return");
        a.Emit(0x48, 0x83, 0xc4, 0x28, 0x5e, 0x5b, 0xc3); // add rsp, 0x28; pop rsi; pop rbx; ret
        return a.Finish();
    }

    static void Main() {
        byte[] code = TaskPacketCore(0x10000, 0x20000, 0x30000, 0x40000, 0x50000, 0x60000, 0x70000, 0x80000);
        Console.WriteLine("Generated code length: " + code.Length + " bytes");
        Console.WriteLine("Assembly generated successfully!");
    }
}

