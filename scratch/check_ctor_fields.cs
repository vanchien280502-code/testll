using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x2E2AA0 + 0x48;
        int off = ToFileOffset(rva);
        for (int i = 0; i < 64; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

