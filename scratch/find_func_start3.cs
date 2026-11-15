using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x496FB0;
        int fo = rva - 0x1A00;
        // Search backwards for typical function prologues
        // e.g., 40 53, 40 55, 48 89 5C 24, 48 83 EC
        for (int i = 0; i < 0x2000; i++) {
            int p = fo - i;
            if (bytes[p] == 0xCC && (bytes[p+1] == 0x40 || bytes[p+1] == 0x48 || bytes[p+1] == 0x55 || bytes[p+1] == 0x56)) {
                int startRva = (p + 1) + 0x1A00;
                Console.WriteLine("Function start found at RVA 0x{0:X} (-0x{1:X} bytes)", startRva, i - 1);
                Console.WriteLine("Bytes: " + BitConverter.ToString(bytes, p + 1, 16));
                break;
            }
        }
    }
}

