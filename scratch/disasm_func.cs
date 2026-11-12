using System;
using System.IO;

public class DisasmFunc {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4E6A10;
        int fileOffset = (rva - 0x237000) + 0x235600;

        Console.WriteLine("Dumping 0x4E6A10 (size ~150 bytes):");
        for (int i = 0; i < 150; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

