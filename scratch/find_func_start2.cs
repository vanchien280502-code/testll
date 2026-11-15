using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x496F90;
        int fileOffset = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 200; i++) {
            if (bytes[fileOffset - i] == 0xCC) {
                int startRva = (fileOffset - i + 1 - 0x235600) + 0x237000;
                Console.WriteLine("Start RVA = 0x{0:X} (offset -{1})", startRva, i);
                break;
            }
        }
    }
}

