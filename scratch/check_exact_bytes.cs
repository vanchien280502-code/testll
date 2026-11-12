using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x357AD0;
        int off = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 32; i++) {
            Console.WriteLine("0x{0:X6}: {1:X2}", rva + i, bytes[off + i]);
        }
    }
}

