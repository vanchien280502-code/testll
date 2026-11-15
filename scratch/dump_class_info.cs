using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C1F00;
        int fo = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 0x100; i += 8) {
            long p = BitConverter.ToInt64(bytes, fo + i);
            Console.WriteLine("0x{0:X7}: 0x{1:X16}", rva + i, p);
        }
    }
}
