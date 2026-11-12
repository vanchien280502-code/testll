using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        long targetVA = 0x180000000L + 0x16C5DE0;
        Console.WriteLine("Searching for pointer to method table 0x{0:X}...", targetVA);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            if (BitConverter.ToInt64(bytes, i) == targetVA) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Found pointer at RVA 0x{0:X}", rva);
            }
        }
    }
}

