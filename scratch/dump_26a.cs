using System;
using System.IO;

class Dump26A {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x26A0D0;
        int len = 132;
        int off = ToFileOffset(rva);

        Console.WriteLine("All bytes of 0x26A0D0:");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
        // Print all calls and static refs
        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, cur + 5 + rel);
            }
            if (b == 0x48 && (bytes[off + i + 1] == 0x8D || bytes[off + i + 1] == 0x8B) && (bytes[off + i + 2] == 0x0D || bytes[off + i + 2] == 0x05)) {
                int disp = BitConverter.ToInt32(bytes, off + i + 3);
                Console.WriteLine("  0x{0:X}: REF 0x{1:X}", cur, cur + 7 + disp);
            }
        }
    }
}

