using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int off = 0x16C0F10;
        Console.WriteLine("Vtable entries around 0x16C0F10:");
        for (int i = -40; i <= 40; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, off + i);
            long rva = ptr - 0x180000000L;
            Console.WriteLine("Offset {0:+00;-00} (0x{1:X}): RVA 0x{2:X}", i, (0x16C3B10 + i), rva);
        }
    }
}
