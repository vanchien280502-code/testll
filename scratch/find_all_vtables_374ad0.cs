using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        long target = 0x180000000L + 0x374AD0;
        Console.WriteLine("Searching for vtables containing 0x{0:X}...", target);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == target) {
                int rva = i + 0x2C00; // in .data
                Console.WriteLine("Vtable entry at FO 0x{0:X} (approx RVA 0x{1:X})", i, rva);
            }
        }
    }
}

