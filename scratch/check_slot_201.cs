using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C26C8;
        int fo = (rva - 0x237000) + 0x235600;
        long ptr = BitConverter.ToInt64(bytes, fo);
        Console.WriteLine("Slot 201 (0x16C26C8): 0x{0:X}", ptr - 0x180000000L);
    }
}
