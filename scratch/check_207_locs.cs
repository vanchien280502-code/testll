using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva1 = 0x4326F0;
        int off1 = (rva1 - 0x237000) + 0x235600;
        Console.WriteLine("0x4326F0:");
        for (int i = 0; i < 48; i++) Console.Write("{0:X2} ", bytes[off1 + i]);
        Console.WriteLine();

        int rva2 = 0x44E7E0;
        int off2 = (rva2 - 0x237000) + 0x235600;
        Console.WriteLine("0x44E7E0:");
        for (int i = 0; i < 48; i++) Console.Write("{0:X2} ", bytes[off2 + i]);
        Console.WriteLine();
    }
}

