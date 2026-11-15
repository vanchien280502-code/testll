using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] rvas = new int[] { 0x500FF0, 0x501420, 0x5019A0 };
        foreach (int rva in rvas) {
            int fo = rva - 0x1A00;
            Console.WriteLine("--- RVA 0x{0:X} ---", rva);
            for (int i = 0; i < 48; i += 16) {
                Console.WriteLine("{0:X2}: {1}", i, BitConverter.ToString(bytes, fo + i, 16));
            }
        }
    }
}

