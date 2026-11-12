using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int targetRva = 0x4DAC08;
        // Search inside [0x4D8BE0, 0x4DAC08] for lea/mov with 0x4DAC08
        for (int rva = 0x4D8BE0; rva < 0x4DAC08; rva++) {
            int off = ToFileOffset(rva);
            // check rip-relative: rva + 7 + disp32 == 0x4DAC08
            if (bytes[off] == 0x48 && bytes[off+1] == 0x8D) { // lea reg, [rip + disp]
                int disp = BitConverter.ToInt32(bytes, off + 3);
                if (rva + 7 + disp == targetRva) {
                    Console.WriteLine("Found reference to jump table at 0x{0:X}", rva);
                }
            }
        }
    }
}

