using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int targetRva = 0x2D8F00;
        for (int rva = 0x237000; rva < 0x1200000; rva++) {
            int off = ToFileOffset(rva);
            if (bytes[off] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + 1);
                if (rva + 5 + rel == targetRva) {
                    Console.WriteLine("CALL to 0x2D8F00 from RVA 0x{0:X}", rva);
                }
            }
        }
    }
}

