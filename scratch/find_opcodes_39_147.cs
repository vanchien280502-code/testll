using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Look for constructors of Message with opcode 147 (0x93 or -109)
        // mov dl, 0x93 or mov edx, 147
        Console.WriteLine("Searching for opcode 147 (0x93)...");
        for (int i = 0; i < bytes.Length - 10; i++) {
            if ((bytes[i] == 0xB2 && bytes[i+1] == 0x93) || (bytes[i] == 0xBA && bytes[i+1] == 0x93 && bytes[i+2] == 0 && bytes[i+3] == 0 && bytes[i+4] == 0)) {
                int rva = i - 0x235600 + 0x237000;
                Console.WriteLine("Found opcode 147 at RVA 0x{0:X}", rva);
            }
        }
        Console.WriteLine("Searching for opcode 39 (0x27)...");
        for (int i = 0; i < bytes.Length - 10; i++) {
            if ((bytes[i] == 0xB2 && bytes[i+1] == 0x27) || (bytes[i] == 0xBA && bytes[i+1] == 0x27 && bytes[i+2] == 0 && bytes[i+3] == 0 && bytes[i+4] == 0)) {
                int rva = i - 0x235600 + 0x237000;
                Console.WriteLine("Found opcode 39 at RVA 0x{0:X}", rva);
            }
        }
    }
}

