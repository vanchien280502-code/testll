using System;
using System.IO;

class Disasm4D8BEarly {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4D8BE0;
        int len = 200;
        int off = ToFileOffset(rva);

        Console.WriteLine("First 200 bytes of 0x4D8BE0:");
        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, cur + 5 + rel);
            }
            if (b == 0xBA && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov edx, 0x{1:X} ({1})", cur, val);
            }
            if (b == 0x3D && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: cmp eax, 0x{1:X} ({1})", cur, val);
            }
            if (b == 0x83 && bytes[off + i + 1] == 0xF8 && i + 3 < len) {
                sbyte val = (sbyte)bytes[off + i + 2];
                Console.WriteLine("  0x{0:X}: cmp eax, {1} (0x{1:X})", cur, val);
            }
        }
    }
}

