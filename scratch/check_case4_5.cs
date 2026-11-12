using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        Dump(0x4D9101, 80, "Case 4 (Action 2989)");
        Dump(0x4D9177, 80, "Case 5 (Action 2990)");
    }

    static void Dump(int rva, int len, string name) {
        Console.WriteLine("=== {0} at 0x{1:X} ===", name, rva);
        int off = ToFileOffset(rva);
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }
}

