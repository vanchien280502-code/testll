using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int target = 0x495740;
        Console.WriteLine("Searching calls to 0x{0:X}:", target);
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
    }
}
