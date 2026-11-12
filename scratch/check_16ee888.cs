using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int off = 0x16EE888 - 0x237000 + 0x235600;
        Console.WriteLine("Offset of 0x16EE888: 0x{0:X}", off);
        // It's in BSS / data pointer slot.
    }
}

