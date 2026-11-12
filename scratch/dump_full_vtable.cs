using System;
using System.IO;

class DumpFullVtable {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int centerOffset = 0x16C0E18;
        for (int i = 0; i <= 200; i++) {
            long p = BitConverter.ToInt64(bytes, centerOffset + i * 8);
            long rva = p - 0x180000000L;
            if (rva < 0x200000 || rva > 0x800000) break;
            Console.WriteLine("Slot {0,3} (off +0x{1:X3}): 0x{2:X}", i, i * 8, rva);
        }
    }
}

