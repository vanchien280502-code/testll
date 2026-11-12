using System;
using System.IO;

class FindVtableCalls {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // FF 90 xx xx 00 00 (call [rax + disp32])
        // or FF 92 xx xx 00 00 (call [rdx + disp32])
        // or 41 FF 90 / 41 FF 91 / 41 FF 92 ...
        // Search for disp32 = 0x488 (1160) or 0x490 (1168) or 0x470 (1136)
        int[] disps = new int[] { 0x470, 0x488, 0x490, 0x220, 0x278 };
        for (int i = 0; i < bytes.Length - 6; i++) {
            if (bytes[i] == 0xFF && (bytes[i+1] >= 0x90 && bytes[i+1] <= 0x97)) {
                int disp = BitConverter.ToInt32(bytes, i + 2);
                foreach (int d in disps) {
                    if (disp == d) {
                        int rva = (i - 0x235600) + 0x237000;
                        Console.WriteLine("Call [reg + 0x{0:X}] at RVA 0x{1:X}", d, rva);
                    }
                }
            }
        }
    }
}

