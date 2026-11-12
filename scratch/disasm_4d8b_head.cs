using System;
using System.IO;

class Disasm4D8BHead {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int startRva = 0x4D8BE0;
        int endRva = 0x4D8F60;
        int len = endRva - startRva;
        int off = ToFileOffset(startRva);

        Console.WriteLine("Scanning all cmp edx / sub edx / switches in 0x4D8BE0:");
        for (int i = 0; i < len; i++) {
            int cur = startRva + i;
            byte b = bytes[off + i];
            // cmp edx, imm32: 81 FA xx xx xx xx
            if (b == 0x81 && bytes[off + i + 1] == 0xFA && i + 6 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 2);
                Console.WriteLine("  0x{0:X}: cmp edx, {1} (0x{1:X})", cur, val);
            }
            // cmp edx, imm8: 83 FA xx
            if (b == 0x83 && bytes[off + i + 1] == 0xFA && i + 3 < len) {
                sbyte val = (sbyte)bytes[off + i + 2];
                Console.WriteLine("  0x{0:X}: cmp edx, {1} (0x{1:X})", cur, val);
            }
            // sub edx, imm32: 81 EA xx xx xx xx
            if (b == 0x81 && bytes[off + i + 1] == 0xEA && i + 6 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 2);
                Console.WriteLine("  0x{0:X}: sub edx, {1} (0x{1:X})", cur, val);
            }
            // lea eax, [rdx - imm32]: 8D 82 xx xx xx xx
            if (b == 0x8D && bytes[off + i + 1] == 0x82 && i + 6 < len) {
                int val = -BitConverter.ToInt32(bytes, off + i + 2);
                Console.WriteLine("  0x{0:X}: lea eax, [rdx - {1}] (0x{1:X})", cur, val);
            }
        }
    }
}

