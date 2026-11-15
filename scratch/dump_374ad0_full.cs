using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x374AD0;
        int fo = rva - 0x1A00;
        Console.WriteLine("0x374AD0 full disassembly bytes (150 bytes):");
        for (int i = 0; i < 150; i += 16) {
            int chunk = Math.Min(16, 150 - i);
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, fo + i, chunk));
        }
    }
}

