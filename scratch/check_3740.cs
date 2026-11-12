using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    static void Main() {
        int off = 0x4340 - 0x1000 + 0x400;
        Console.WriteLine("File offset: 0x{0:X}", off);
        for (int i = 0; i < 48; i++) Console.Write("{0:X2} ", bytes[off + i]);
        Console.WriteLine();
    }
}

