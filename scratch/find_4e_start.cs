using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4E4093;
        int fo = rva - 0x1A00;
        for (int i = 1; i < 0x8000; i++) {
            int p = fo - i;
            if (bytes[p] == 0xC3 || (bytes[p] == 0xCC && bytes[p-1] == 0xC3)) {
                int startRva = (p + 1) + 0x1A00;
                while (bytes[startRva - 0x1A00] == 0xCC) startRva++;
                Console.WriteLine("Function start at RVA 0x{0:X} (-0x{1:X})", startRva, fo - (startRva - 0x1A00));
                break;
            }
        }
    }
}
