using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x2DB340;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Around 0x2DB368:");
        for (int i = 0; i < 64; i += 16) {
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

