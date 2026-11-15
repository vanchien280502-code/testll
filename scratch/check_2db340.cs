using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x2DB340;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine(string.Format("0x2DB340:"));
        byte[] slice = new byte[128];
        Array.Copy(bytes, fileOffset - 32, slice, 0, 128);
        for (int i = 0; i < 128; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(slice, i, 16).Replace('-', ' '));
        }
    }
}

