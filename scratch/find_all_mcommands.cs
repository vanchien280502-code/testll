using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Search in 0x2D8F00 - 0x2DC35C for CALL 0x5290 (creating mCommand)
        for (int rva = 0x2D8F00; rva < 0x2DC35C; rva++) {
            int off = ToFileOffset(rva);
            if (bytes[off] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + 1);
                int target = rva + 5 + rel;
                if (target == 0x5290 || target == 0x5288 || target == 0x5280) {
                    // Check previous instructions for action id
                    int prevOff = Math.Max(0, off - 30);
                    Console.Write("mCommand created at RVA 0x{0:X}: ", rva);
                    for (int k = off - 15; k < off; k++) {
                        Console.Write("{0:X2} ", bytes[k]);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}

