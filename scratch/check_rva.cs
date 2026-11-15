using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x258416;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Bytes at 0x258416:");
        for (int i = 0; i < 48; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
        }
        Console.WriteLine();
    }
}

