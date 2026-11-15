using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x495740;
        int fo = rva - 0x1A00;
        Console.WriteLine("0x495740 bytes (128 bytes):");
        for (int i = 0; i < 128; i += 16) {
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, fo + i, 16));
        }
    }
}

