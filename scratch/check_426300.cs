using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x426300;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine("0x426300:");
        for (int i = 0; i < 48; i++) Console.Write("{0:X2} ", bytes[fileOffset + i]);
        Console.WriteLine();
    }
}

