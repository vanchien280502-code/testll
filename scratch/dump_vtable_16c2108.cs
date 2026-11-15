using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C2080;
        int fo = (rva - 0x237000) + 0x235600;
        Console.WriteLine("0x16C2080 - 0x16C2180:");
        for (int i = 0; i < 0x100; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, fo + i);
            long mRva = ptr - 0x180000000L;
            Console.WriteLine("Offset 0x{0:X3} (Slot {1:D3}): 0x{2:X}", i, i / 8, mRva);
        }
    }
}
