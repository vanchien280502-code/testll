using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C6D00 + 0x488;
        int fo = (rva - 0x237000) + 0x235600;
        long ptr = BitConverter.ToInt64(bytes, fo);
        Console.WriteLine("LuckyWheel slot 0x488: 0x{0:X}", ptr - 0x180000000L);
    }
}
