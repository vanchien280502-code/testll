using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Message class is at RVA 0x16ee820
        // Let's find Message vtable
        int rva = 0x16ee820;
        int fileOffset = (rva - 0x129B000) + 0x1298400; // section .data
        Console.WriteLine("Reading Message class at file offset 0x{0:X}...", fileOffset);
        // In il2cpp, let's look at ctor of Message (0x2E28A0)
        // Let's check callers of 0x2E28A0 or see what vtable is in 0x2E2AA0
    }
}

