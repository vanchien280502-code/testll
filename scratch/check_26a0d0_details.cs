using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x26A0D0;
        int off = ToFileOffset(rva);
        int len = 132;
        int i = 0;
        while (i < len) {
            int cur = rva + i;
            byte b = bytes[off + i];
            // call rel32
            if (b == 0xE8 && i + 5 <= len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("0x{0:X}: CALL 0x{1:X}", cur, cur + 5 + rel);
                i += 5;
                continue;
            }
            // jmp rel32
            if (b == 0xE9 && i + 5 <= len) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("0x{0:X}: JMP 0x{1:X}", cur, cur + 5 + rel);
                i += 5;
                continue;
            }
            // print byte
            Console.WriteLine("0x{0:X}: {1:X2}", cur, b);
            i++;
        }
    }
}

