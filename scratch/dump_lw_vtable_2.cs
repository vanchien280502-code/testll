using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C6D00;
        int fo = (rva - 0x237000) + 0x235600;
        Console.WriteLine("LuckyWheel Vtable slots 0x200 - 0x300:");
        for (int i = 0x200; i < 0x300; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, fo + i);
            long mRva = ptr - 0x180000000L;
            Console.WriteLine("Offset 0x{0:X3} (Slot {1:D3}): 0x{2:X}", i, i / 8, mRva);
        }
    }
}
