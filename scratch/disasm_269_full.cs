using System;
using System.IO;

class Disasm269Full {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Dump(int rva, int len, string name) {
        Console.WriteLine("=== " + name + " (RVA 0x" + rva.ToString("X") + ") ===");
        int off = ToFileOffset(rva);
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
        }
    }

    static void Main() {
        Dump(0x269740, 83, "0x269740");
        Dump(0x26A0D0, 132, "0x26A0D0");
    }
}

