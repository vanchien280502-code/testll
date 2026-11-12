using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    static void Main() {
        int targetRva = 0x2D8F00;
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == 0x180000000L + targetRva) {
                Console.WriteLine("Found ref to 0x2D8F00 at file offset 0x{0:X}", i);
            }
        }
    }
}

