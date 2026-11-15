using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x48E0;
        int fileOffset = (rva - 0x1000) + 0x400;
        Console.WriteLine("0x48E0:");
        for (int i = 0; i < 32; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
        }
        Console.WriteLine();
    }
}

