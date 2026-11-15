using System;
using System.IO;

class Dump326AA0 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x326AA0;
        int off = ToFileOffset(rva);
        Console.WriteLine("Dumping 0x{0:X}:", rva);
        for (int i = 0; i < 64; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
    }
}

