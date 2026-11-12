using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int targetRva = 0x16ED638;
        for (int rva = 0x237000; rva < 0x1200000; rva++) {
            int off = ToFileOffset(rva);
            // check rip-relative: rva + 7 + disp32 == targetRva
            if (bytes[off] == 0x48 && (bytes[off+1] == 0x8B || bytes[off+1] == 0x8D) && (bytes[off+2] & 0xC7) == 0x05) {
                int disp = BitConverter.ToInt32(bytes, off + 3);
                if (rva + 7 + disp == targetRva) {
                    Console.WriteLine("Reference to 0x16ED638 at RVA 0x{0:X}", rva);
                }
            }
        }
    }
}

