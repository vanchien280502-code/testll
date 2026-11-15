using System;
using System.IO;

class Program {
    static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int fileBase = 0x235600;
        int rvaBase = 0x237000;
        int off = (0x4D8FCF - rvaBase) + fileBase;
        for (int i = 0; i < 96; i++) {
            Console.Write("{0:X2} ", b[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
    }
}
