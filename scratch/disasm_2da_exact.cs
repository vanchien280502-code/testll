using System;
using System.IO;

class Disasm2DAExact {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int startRva = 0x2DA3C0;
        int len = 90;
        int off = ToFileOffset(startRva);

        Console.WriteLine("Instructions from 0x2DA3C0 to 0x2DA420:");
        for (int i = 0; i < len; i++) {
            int cur = startRva + i;
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }
}

