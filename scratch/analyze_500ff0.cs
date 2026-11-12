using System;
using System.IO;

class Analyze500FF0 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Analyze(int rva, int len, string name) {
        Console.WriteLine("==================================================");
        Console.WriteLine("Analyzing " + name + " (RVA 0x" + rva.ToString("X") + ", len " + len + ")");
        Console.WriteLine("==================================================");
        int off = ToFileOffset(rva);

        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                int target = cur + 5 + rel;
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, target);
            }
            // Check vtable calls on Message (readByte, readShort, readInt, etc.)
            // FF 90 / FF 91 / FF 92
            if (b == 0xFF && (bytes[off + i + 1] >= 0x90 && bytes[off + i + 1] <= 0x97) && i + 5 < len) {
                int disp = BitConverter.ToInt32(bytes, off + i + 2);
                Console.WriteLine("  0x{0:X}: CALL [reg + 0x{1:X}]", cur, disp);
            }
        }
    }

    static void Main() {
        Analyze(0x500FF0, 400, "0x500FF0 (Slot 145)");
        Analyze(0x501420, 400, "0x501420 (Slot 146)");
    }
}

