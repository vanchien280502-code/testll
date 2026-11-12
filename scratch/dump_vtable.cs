using System;
using System.IO;

class DumpVTable {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int centerOffset = 0x16C0E18;
        for (int i = -0x80; i <= 0x80; i += 8) {
            long p = BitConverter.ToInt64(bytes, centerOffset + i);
            long rva = p - 0x180000000L;
            Console.WriteLine("VTable[{0:+000;-000}]: 0x{1:X}", i, rva);
        }
    }
}

