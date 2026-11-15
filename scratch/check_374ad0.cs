using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x374AD0;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine("0x374AD0 length 256 bytes:");
        for (int i = 0; i < 256; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(bytes, fileOffset + i, 16).Replace('-', ' '));
        }
    }
}
