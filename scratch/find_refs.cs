using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int target = 0x4E04C0;
        Console.WriteLine("Searching references to 0x{0:X}:", target);
        // Search 32-bit call relative
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int rva = (i - 0x235600) + 0x237000;
                int dest = rva + 5 + rel;
                if (dest == target) {
                    Console.WriteLine("  Call from 0x{0:X}", rva);
                }
            }
        }
        // Search 64-bit absolute pointers in data
        long targetRva = 0x180000000L + target;
        byte[] ptrBytes = BitConverter.GetBytes(targetRva);
        for (int i = 0; i < bytes.Length - 8; i++) {
            bool match = true;
            for (int k = 0; k < 8; k++) if (bytes[i+k] != ptrBytes[k]) { match = false; break; }
            if (match) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("  Pointer at RVA 0x{0:X}", rva);
            }
        }
    }
}
