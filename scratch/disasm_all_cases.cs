using System;
using System.IO;

public class DisasmAllCases {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int[] cases = {
            0x4D8F69, 0x4D8FCF, 0x4D9035, 0x4D909B, 0x4D9101, 0x4D9177,
            0x4D91ED, 0x4D9216, 0x4D928C, 0x4D9302, 0x4D9378, 0x4D93EE,
            0x4D9464, 0x4D94DA, 0x4D9519
        };

        for (int c = 0; c < cases.Length; c++) {
            int rva = cases[c];
            int off = ToFileOffset(rva);
            int nextRva = (c + 1 < cases.Length) ? cases[c + 1] : rva + 60;
            int len = Math.Min(nextRva - rva, 80);
            Console.WriteLine(string.Format("=== Case {0} (Action {1}, RVA 0x{2:X}) ===", c, 2985 + c, rva));
            for (int i = 0; i < len; i++) {
                Console.Write("{0:X2} ", bytes[off + i]);
            }
            Console.WriteLine("\n");
            // print calls
            for (int i = 0; i < len; i++) {
                if (bytes[off + i] == 0xE8) {
                    int rel = BitConverter.ToInt32(bytes, off + i + 1);
                    Console.WriteLine(string.Format("  Call at 0x{0:X} -> 0x{1:X}", rva + i, rva + i + 5 + rel));
                }
            }
        }
    }
}

