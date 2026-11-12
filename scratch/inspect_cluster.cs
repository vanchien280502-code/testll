using System;
using System.IO;

class InspectCluster {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Inspect(int rva, int len, string name) {
        Console.WriteLine("=== " + name + " (0x" + rva.ToString("X") + ") ===");
        int off = ToFileOffset(rva);
        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                int target = cur + 5 + rel;
                if (target == 0x2E28A0 || target == 0x2E2AA0) {
                    Console.WriteLine("  0x{0:X}: CALL Message_ctor! (rel 0x{1:X})", cur, target);
                }
            }
            if (b == 0xB2 && i + 1 < len) {
                Console.WriteLine("  0x{0:X}: mov dl, 0x{1:X2} ({1})", cur, bytes[off + i + 1]);
            }
            if (b == 0xBA && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                if (val >= 2980 && val <= 3005) {
                    Console.WriteLine("  0x{0:X}: Action ID {1} (0x{1:X})", cur, val);
                }
            }
        }
    }

    static void Main() {
        Inspect(0x500A30, 0x500D90 - 0x500A30, "0x500A30 (Slot 143)");
        Inspect(0x500D90, 0x500FF0 - 0x500D90, "0x500D90 (Slot 144)");
        Inspect(0x5019A0, 0x501D10 - 0x5019A0, "0x5019A0 (Slot 147)");
    }
}

