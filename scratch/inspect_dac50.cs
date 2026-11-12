using System;
using System.IO;

class InspectDAC50 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4DAC50;
        int len = 200;
        int off = ToFileOffset(rva);

        Console.WriteLine("Method 0x4DAC50 (off +0x08 in vtable):");
        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            byte b = bytes[off + i];
            if (b == 0xE8 && i + 4 < len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: CALL 0x{1:X}", cur, cur + 5 + rel);
            }
            if (cur == 0x4DAE77) {
                Console.WriteLine("--> AT 0x4DAE77 (Call [reg + 0x488]!):");
                for (int k = 0; k < 16; k++) Console.Write("{0:X2} ", bytes[off + i + k]);
                Console.WriteLine();
            }
        }
    }
}

