using System;
using System.IO;

class Scan2D8 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int startRva = 0x2D8F00;
        int endRva = 0x2DC35C;
        int off = ToFileOffset(startRva);
        int len = endRva - startRva;

        Console.WriteLine("Scanning all actions and calls in 0x2D8F00 - 0x2DC35C:");
        for (int i = 0; i < len; i++) {
            int cur = startRva + i;
            byte b = bytes[off + i];
            // Look for mov r9d, imm32 (41 B9 xx xx xx xx)
            // or mov edx, imm32 (BA xx xx xx xx)
            if (b == 0x41 && bytes[off + i + 1] == 0xB9 && i + 5 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 2);
                if (val >= 1000 && val <= 5000) {
                    Console.WriteLine("  0x{0:X}: mov r9d, {1} (0x{1:X})", cur, val);
                }
            }
            if (b == 0xBA && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                if (val >= 1000 && val <= 5000) {
                    Console.WriteLine("  0x{0:X}: mov edx, {1} (0x{1:X})", cur, val);
                }
            }
        }
    }
}

