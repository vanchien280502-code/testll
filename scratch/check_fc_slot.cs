using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16ee888;
        int fo = rva - 0x2C00;
        long val = BitConverter.ToInt64(bytes, fo);
        Console.WriteLine("0x16ee888 value: 0x{0:X16}", val);
    }
}
