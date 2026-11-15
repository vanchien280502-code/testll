using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4E4093;
        int fo = rva - 0x1A00;
        // Search backward for function start
        for (int i = 0; i < 0x2000; i++) {
            int p = fo - i;
            if (bytes[p] == 0xCC && (bytes[p+1] == 0x40 || bytes[p+1] == 0x48 || bytes[p+1] == 0x55 || bytes[p+1] == 0x56)) {
                int startRva = (p + 1) + 0x1A00;
                Console.WriteLine("Function start at RVA 0x{0:X} (-0x{1:X})", startRva, i - 1);
                break;
            }
        }
    }
}
