using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Let's find the Il2CppClass of MainController (fcClass = 0x16EE888)
        // In the game binary, where does fcClass get allocated or initialized?
        // Let's search for pointers to 0x16C0E18 (the vtable)
        // Or let's search for all `call [reg + disp]` where disp corresponds to this vtable slot!
        // First, let's find the start of the Il2CppClass containing 0x16C0E18.
        int vtableOff = 0x16C0E18;
        // In Il2CppClass, the first field is Il2CppClass* or Il2CppImage*.
        // Let's find where the Il2CppClass starts before 0x16C0E18.
        // Usually Il2CppClass has size around 0x138 to 0x140 before the vtable array starts.
        // In this binary, let's see how many pointers precede 0x16C0E18.
        int count = 0;
        for (int i = 8; i < 0x2000; i += 8) {
            long p = BitConverter.ToInt64(bytes, vtableOff - i);
            if (p >= 0x180000000L && p < 0x182000000L) {
                count++;
            } else {
                Console.WriteLine("Vtable started at offset -0x{0:X} ({1} slots before 0x4D8BE0)", i - 8, count);
                int vtableStart = vtableOff - (i - 8);
                int slotIndex = (vtableOff - vtableStart) / 8;
                Console.WriteLine("Slot index of 0x4D8BE0 is: {0}", slotIndex);
                int disp = 0x138 + slotIndex * 8; // if vtable is at 0x138
                int disp140 = 0x140 + slotIndex * 8; // if vtable is at 0x140
                int disp138_16 = 0x138 + slotIndex * 16; // if 16 bytes per slot
                Console.WriteLine("Possible disp: 0x{0:X}, 0x{1:X}, 0x{2:X}", disp, disp140, disp138_16);
                break;
            }
        }
    }
}

