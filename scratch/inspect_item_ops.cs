using System;
using System.IO;

public class InspectItemOps {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] rvas = { 0x3A08FF, 0x3A09D8, 0x3A0A50, 0x3A0AC8, 0x4E698F, 0x4E6A58, 0x4E6B3F };
        foreach (int rva in rvas) {
            int fileOffset = (rva - 0x237000) + 0x235600;
            Console.WriteLine("--- RVA 0x{0:X} ---", rva);
            int start = Math.Max(0, fileOffset - 20);
            for (int i = 0; i < 60; i++) {
                Console.Write("{0:X2} ", bytes[start + i]);
                if ((i + 1) % 16 == 0) Console.WriteLine();
            }
            Console.WriteLine("\n");
        }
    }
}

