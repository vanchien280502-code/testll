using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int target = 0x16ED588;
        Console.WriteLine("Searching references to 0x{0:X}:", target);
        for (int i = 0; i < bytes.Length - 7; i++) {
            // rip-relative 32-bit: 48 8B 05 xx xx xx xx
            if (bytes[i] == 0x48 && bytes[i+1] == 0x8B && (bytes[i+2] == 0x05 || bytes[i+2] == 0x0D)) {
                int rel = BitConverter.ToInt32(bytes, i + 3);
                int rva = (i - 0x235600) + 0x237000;
                int dest = rva + 7 + rel;
                if (dest == target) {
                    Console.WriteLine("  mov at 0x{0:X}", rva);
                }
            }
        }
    }
}
