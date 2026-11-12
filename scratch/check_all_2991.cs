using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int[] rvas = new int[] { 0x17B3C3, 0x2DA410, 0x2E16A5, 0x2E53A6, 0x3056F5, 0x3B1C80 };
        foreach (var r in rvas) {
            Console.WriteLine("=== RVA 0x{0:X} ===", r);
            int off = ToFileOffset(r - 16);
            for (int i = 0; i < 40; i++) Console.Write("{0:X2} ", bytes[off + i]);
            Console.WriteLine();
        }
    }
}

