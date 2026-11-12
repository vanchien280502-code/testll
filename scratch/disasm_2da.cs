using System;
using System.IO;

class Disasm2DA {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x2DA370;
        int len = 250;
        int off = ToFileOffset(rva);

        Console.WriteLine("Around 0x2DA370:");
        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, cur + 5 + rel);
            }
            if (b == 0xB9 && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov ecx, 0x{1:X} ({1})", cur, val);
            }
            if (b == 0xBA && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov edx, 0x{1:X} ({1})", cur, val);
            }
            if (b == 0x41 && b == 0xB9 && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov r9d, 0x{1:X} ({1})", cur, val);
            }
            // Check immediate values in instructions (like cmp, mov)
            if (cur == 0x2DA40E || cur == 0x2DA40F) {
                Console.WriteLine("--> EXACT 0x{0:X}: {1:X2} {2:X2} {3:X2} {4:X2} {5:X2}", cur, bytes[off+i], bytes[off+i+1], bytes[off+i+2], bytes[off+i+3], bytes[off+i+4]);
            }
        }
    }
}

