using System;
using System.IO;

class Program {
    static void Main() {
        string path = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        int rva = 0x2E29A0;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Dumping 0x2E29A0 (around 0x2E29E5):");
        for (int i = 0; i < 160; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

