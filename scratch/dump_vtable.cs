using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C2800;
        int fo = (rva - 0x237000) + 0x235600;
        Console.WriteLine("0x16C2800 - 0x16C2900:");
        for (int i = 0; i < 0x100; i += 16) {
            long p1 = BitConverter.ToInt64(bytes, fo + i);
            long p2 = BitConverter.ToInt64(bytes, fo + i + 8);
            Console.WriteLine("{0:X7}: {1:X16} {2:X16}", rva + i, p1, p2);
        }
    }
}
