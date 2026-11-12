using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static void Main() {
        for (int i = -0x50; i <= 0x50; i += 8) {
            int off = 0x16C09D0 + i;
            long val = BitConverter.ToInt64(bytes, off);
            long rva = val >= 0x180000000L ? (val - 0x180000000L) : val;
            Console.WriteLine("  [0x16C09D0 + {0:X3}] = 0x{1:X} (RVA 0x{2:X})", i, val, rva);
        }
    }
}

