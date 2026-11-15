using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16ED588;
        int fo = (rva - 0x237000) + 0x235600;
        long val = BitConverter.ToInt64(bytes, fo);
        Console.WriteLine("0x16ED588: 0x{0:X16}", val);
    }
}
