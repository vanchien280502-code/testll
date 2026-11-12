using System;
using System.IO;

class Program {
    static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int off = 0x16C0E18;
        Console.WriteLine("Pointers around 0x16C0E18:");
        for (int i = -0x150; i <= 0x50; i += 8) {
            long val = BitConverter.ToInt64(b, off + i);
            long rva = val >= 0x180000000L ? (val - 0x180000000L) : val;
            Console.WriteLine("  [0x16C0E18 + {0:X3}] = 0x{1:X} (RVA 0x{2:X})", i, val, rva);
        }
    }
}
