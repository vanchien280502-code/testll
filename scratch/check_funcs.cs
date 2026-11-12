using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        Dump(0x269740, "0x269740");
        Dump(0x26A0D0, "0x26A0D0");
    }

    static void Dump(int rva, string name) {
        Console.WriteLine("=== {0} ===", name);
        int off = ToFileOffset(rva);
        for (int i = 0; i < 48; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
        }
        Console.WriteLine();
    }
}

