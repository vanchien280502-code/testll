using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4D8F60;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Bytes around 0x4D8F60:");
        for (int i = 0; i < 120; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

