using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int lwVtableRva = 0x16C6D00;
        int fo = (lwVtableRva - 0x237000) + 0x235600;
        Console.WriteLine("All unique methods in LuckyWheel vtable:");
        for (int i = 0; i < 0x400; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, fo + i);
            long mRva = ptr - 0x180000000L;
            if (mRva >= 0x3CE000 && mRva <= 0x3D1000) {
                Console.WriteLine("  Slot {0,3} (offset 0x{1:X3}): 0x{2:X}", i / 8, i, mRva);
            }
        }
    }
}
