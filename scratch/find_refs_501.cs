using System;
using System.IO;

class FindRefs {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        long[] targets = new long[] {
            0x180000000L + 0x500FF0,
            0x180000000L + 0x501420
        };

        for (int t = 0; t < targets.Length; t++) {
            long target = targets[t];
            byte[] b = BitConverter.GetBytes(target);
            for (int i = 0; i < bytes.Length - 8; i += 8) {
                long p = BitConverter.ToInt64(bytes, i);
                if (p == target) {
                    int rva = (i - 0x235600) + 0x237000;
                    Console.WriteLine("Target {0:X} referenced at offset 0x{1:X} (RVA 0x{2:X})", target - 0x180000000L, i, rva);
                }
            }
        }
    }
}

