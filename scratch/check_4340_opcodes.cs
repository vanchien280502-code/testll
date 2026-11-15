using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int target = 0x4340;
        int ctor1 = 0x2E28A0;
        int ctor2 = 0x2E2AA0;

        int found = 0;
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int rvaSource = i - 0x235600 + 0x237000;
                long dest = (long)rvaSource + 5 + rel;
                if (dest == target) {
                    // Check previous 200 bytes for calls to ctor1 or ctor2
                    for (int j = Math.Max(0, i - 200); j < i; j++) {
                        if (bytes[j] == 0xE8) {
                            int r2 = BitConverter.ToInt32(bytes, j + 1);
                            int s2 = j - 0x235600 + 0x237000;
                            long d2 = (long)s2 + 5 + r2;
                            if (d2 == ctor1 || d2 == ctor2) {
                                Console.WriteLine(string.Format("Caller of 0x4340 at RVA 0x{0:X}, ctor at 0x{1:X}", rvaSource, s2));
                                found++;
                                if (found > 30) return;
                                break;
                            }
                        }
                    }
                }
            }
        }
        Console.WriteLine("Total found: " + found);
    }
}
