using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x5410;
        int fileOffset = (rva - 0x1000) + 0x400;
        Console.WriteLine("0x5410:");
        for (int i = 0; i < 48; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
        }
        Console.WriteLine();
    }
}

