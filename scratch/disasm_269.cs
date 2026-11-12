using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        Disasm(0x269740, 83, "0x269740");
        Disasm(0x26A0D0, 132, "0x26A0D0");
    }

    static void Disasm(int startRva, int len, string name) {
        Console.WriteLine("==================== {0} ====================", name);
        int off = ToFileOffset(startRva);
        for (int i = 0; i < len; i++) {
            int cur = startRva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, cur + 5 + rel);
            }
            if (b == 0x48 && bytes[off + i + 1] == 0x8B && (bytes[off + i + 2] & 0xC7) == 0x05) { // mov reg, [rip + disp]
                int disp = BitConverter.ToInt32(bytes, off + i + 3);
                Console.WriteLine("  0x{0:X}: mov reg, [0x{1:X}]", cur, cur + 7 + disp);
            }
        }
    }
}

