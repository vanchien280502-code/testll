using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4D8BE0;
        int off = ToFileOffset(rva);
        Console.WriteLine("0x4D8BE0 first 50 bytes:");
        for (int i = 0; i < 50; i++) Console.Write("{0:X2} ", bytes[off + i]);
        Console.WriteLine();
    }
}

