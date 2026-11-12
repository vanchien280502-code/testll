using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x5018A0;
        int off = ToFileOffset(rva);
        // Let's check what r10 and r8 are before 0x5018E0
        for (int i = 0; i < 0x50; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0x4C && bytes[off + i + 1] == 0x8B) {
                Console.WriteLine("0x{0:X}: 4C 8B ...", cur);
            }
        }
    }
}

