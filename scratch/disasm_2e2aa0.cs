using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x2E2AA0;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine(string.Format("=== Disassembly of 0x2E2AA0 (RVA 0x{0:X}, FileOffset 0x{1:X}) ===", rva, fileOffset));
        byte[] slice = new byte[128];
        Array.Copy(bytes, fileOffset, slice, 0, 128);
        for (int i = 0; i < 128; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(slice, i, 16).Replace('-', ' '));
        }
    }
}

