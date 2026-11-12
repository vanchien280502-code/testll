using System;
using System.IO;

class Disasm5007E0 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x5007E0;
        int len = 0x500A2C - 0x5007E0;
        int off = ToFileOffset(rva);

        Console.WriteLine("Disassembling 0x5007E0 (len {0}):", len);
        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, cur + 5 + rel);
            }
            if (b == 0xB2 && i + 1 < len) {
                Console.WriteLine("  0x{0:X}: mov dl, 0x{1:X2} ({1})", cur, bytes[off + i + 1]);
            }
            if (b == 0xBA && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov edx, 0x{1:X} ({1})", cur, val);
            }
            if (b == 0xB9 && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov ecx, 0x{1:X} ({1})", cur, val);
            }
            if (cur == 0x500990) {
                Console.WriteLine("--> AT 0x500990:");
            }
        }
    }
}

