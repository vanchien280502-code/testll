using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x2E28A0;
        int off = ToFileOffset(rva);
        Console.WriteLine("Entire 0x2E28A0 (up to C3):");
        for (int i = 0; i < 200; i++) {
            byte b = bytes[off + i];
            Console.Write("{0:X2} ", b);
            if (b == 0xC3) {
                Console.WriteLine("\nFound RET at offset +0x{0:X} (len {1})", i, i + 1);
                break;
            }
        }
    }
}

