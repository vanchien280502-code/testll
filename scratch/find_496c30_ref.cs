using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x496C30;
        long target = 0x180000000L + rva;
        Console.WriteLine("Searching for references to 0x{0:X}...", target);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == target) {
                int refRva = i + 0x1A00;
                Console.WriteLine("Pointer at FO 0x{0:X} (RVA 0x{1:X})", i, refRva);
            }
        }
    }
}

