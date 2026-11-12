using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        Dump(0x269740, 83, "0x269740");
        Dump(0x26A0D0, 132, "0x26A0D0");
    }

    static void Dump(int startRva, int len, string name) {
        Console.WriteLine("==================== {0} ====================", name);
        int off = ToFileOffset(startRva);
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }
}

