using System;
using System.IO;

class Program {
    static void Main() {
        string path = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        int rva = 0x357A00;
        int off = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 400; i += 16) {
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

