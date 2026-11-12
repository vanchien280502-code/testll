using System;
using System.IO;

class SearchInts {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] targets = new int[] { 2985, 2986, 2987, 2988, 2989, 2990, 2991, 2998, 2999 };

        foreach (int target in targets) {
            byte[] b = BitConverter.GetBytes(target);
            Console.WriteLine("--- Target {0} (0x{0:X4}) ---", target);
            for (int i = 0; i < bytes.Length - 4; i++) {
                if (bytes[i] == b[0] && bytes[i+1] == b[1] && bytes[i+2] == b[2] && bytes[i+3] == b[3]) {
                    int rva = (i - 0x235600) + 0x237000;
                    Console.WriteLine("  Offset 0x{0:X} (RVA 0x{1:X})", i, rva);
                }
            }
        }
    }
}

