using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static void Main() {
        int off = 0x16C12A8;
        for (int i = -0x40; i <= 0x40; i += 8) {
            long p = BitConverter.ToInt64(bytes, off + i);
            long rva = p >= 0x180000000L ? (p - 0x180000000L) : p;
            Console.WriteLine("[0x16C12A8 + {0:X2}] = 0x{1:X} (RVA 0x{2:X})", i, p, rva);
        }
    }
}

