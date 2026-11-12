using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4B3830;
        int off = ToFileOffset(rva);
        Console.WriteLine("At RVA 0x4B3830:");
        for (int i = 0; i < 200; i += 16) {
            Console.WriteLine("{0:X4} (0x{1:X6}): {2}", i, rva + i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

