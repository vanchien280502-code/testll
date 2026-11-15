using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In check_258f30.cs:
        // At 0x258F7B: call [r8 + 0x298]
        // Let's trace callers of the actual function that [r8 + 0x298] points to!
        // Wait, where is the vtable populated?
        // In IL2CPP, class initialization populates vtable from method pointers in metadata or code.
        // Let's search for the function that sends messages!
        // In check_4340_opcodes:
        // 0x4340 has callers:
        // 0x2DB368, 0x2DB66B, 0x2DB7AA, 0x2DB84B, 0x426311, 0x4797BA, 0x4D8FB1, 0x4D9017...
        // Let's check 0x4797BA!
        Console.WriteLine("Let's disassemble 0x4797A0 to 0x4797E0");
        int rva = 0x4797A0;
        int fileOffset = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 64; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(bytes, fileOffset + i, 16).Replace('-', ' '));
        }
    }
}

