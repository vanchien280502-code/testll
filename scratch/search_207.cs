using System;
using System.IO;

class Search207 {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rvaBase = 0x237000;
        int fileBase = 0x235600;

        Console.WriteLine("Searching for Message construction with opcode 207 (0xCF or -49)...");
        // Look for: mov edx, 207 (BA CF 00 00 00 or B2 CF) followed by CALL ctor
        for (int i = 0; i < bytes.Length - 10; i++) {
            if (bytes[i] == 0xBA && bytes[i + 1] == 0xCF && bytes[i + 2] == 0x00 && bytes[i + 3] == 0x00 && bytes[i + 4] == 0x00) {
                int rva = (i - fileBase) + rvaBase;
                Console.WriteLine("Found 'mov edx, 207' at RVA 0x{0:X} (file offset 0x{1:X})", rva, i);
            }
            if (bytes[i] == 0xBA && bytes[i + 1] == 0xD1 && bytes[i + 2] == 0xFF && bytes[i + 3] == 0xFF && bytes[i + 4] == 0xFF) { // -47?
            }
            // mov dl, 0xCF (B2 CF)
            if (bytes[i] == 0xB2 && bytes[i + 1] == 0xCF) {
                int rva = (i - fileBase) + rvaBase;
                Console.WriteLine("Found 'mov dl, 0xCF' at RVA 0x{0:X} (file offset 0x{1:X})", rva, i);
            }
        }
    }
}

