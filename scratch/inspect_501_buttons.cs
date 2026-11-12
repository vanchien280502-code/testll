using System;
using System.IO;

class Inspect501Buttons {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Dump(int rva, int len, string label) {
        Console.WriteLine("=== " + label + " (RVA 0x" + rva.ToString("X") + ") ===");
        int off = ToFileOffset(rva);
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
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
            if (b == 0xB9 && i + 4 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov ecx, 0x{1:X} ({1})", cur, val);
            }
            if (b == 0x41 && bytes[off + i + 1] == 0xB8 && i + 5 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 2);
                Console.WriteLine("  0x{0:X}: mov r8d, 0x{1:X} ({1})", cur, val);
            }
            if (b == 0xC7 && (bytes[off + i + 1] >= 0x40 && bytes[off + i + 1] <= 0x47) && i + 6 < len) {
                int val = BitConverter.ToInt32(bytes, off + i + 3);
                Console.WriteLine("  0x{0:X}: mov [reg+disp], 0x{1:X} ({1})", cur, val);
            }
        }
    }

    static void Main() {
        Dump(0x501370, 70, "0x501370 (around 2987/2988)");
        Dump(0x5018D0, 70, "0x5018D0 (around 2985/2986)");
    }
}

