using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x2E2A60;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine(string.Format("0x2E2A60:"));
        byte[] slice = new byte[64];
        Array.Copy(bytes, fileOffset, slice, 0, 64);
        for (int i = 0; i < 64; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(slice, i, 16).Replace('-', ' '));
        }
    }
}

