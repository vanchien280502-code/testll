using System;
using System.IO;

class Inspect326 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int startRva = 0x326300;
        int len = 0x400;
        Console.WriteLine("Dumping 0x{0:X} - 0x{1:X}...", startRva, startRva + len);
        int off = ToFileOffset(startRva);

        for (int i = 0; i < len; i++) {
            int cur = startRva + i;
            byte b = bytes[off + i];
            if (b == 0xBA && bytes[off + i + 1] == 0xCF && bytes[off + i + 2] == 0x00) {
                Console.WriteLine("  0x{0:X}: mov edx, 207", cur);
            }
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                int target = cur + 5 + rel;
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, target);
            }
        }
    }
}

