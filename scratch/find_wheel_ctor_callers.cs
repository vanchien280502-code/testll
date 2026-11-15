using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Lucky Wheel ctor was 0x495740!
        // Let's find callers of 0x495740!
        int target = 0x495740;
        Console.WriteLine("Searching for callers of Lucky Wheel ctor 0x495740...");
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int rva = (i + 5) + 0x1A00;
                if (rva + rel == target) {
                    Console.WriteLine("Caller at RVA 0x{0:X} (FO 0x{1:X})", rva - 5, i);
                }
            }
        }
    }
}
