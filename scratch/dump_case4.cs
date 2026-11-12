using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x357C24;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("From Case 4 (0x357C24):");
        for (int i = 0; i < 96; i += 16) {
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

