using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] rvas = { 0x3D02F0, 0x3D04D0, 0x3D0750 };
        foreach (int rva in rvas) {
            int fo = (rva - 0x237000) + 0x235600;
            Console.WriteLine("--- RVA 0x{0:X} ---", rva);
            for (int i = 0; i < 48; i += 16) {
                Console.WriteLine("{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, fo + i, 16).Replace('-', ' '));
            }
        }
    }
}
