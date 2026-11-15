using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x3CEB00;
        int fo = (rva - 0x237000) + 0x235600;
        Console.WriteLine("0x3CEB00 - 0x3CEC70:");
        for (int i = 0; i < 0x170; i += 16) {
            Console.WriteLine("{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, fo + i, 16).Replace('-', ' '));
        }
    }
}
