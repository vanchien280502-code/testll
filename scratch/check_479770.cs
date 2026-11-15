using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x479770;
        int fileOffset = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 48; i++) {
            Console.WriteLine("{0:X6}: {1:X2}", rva + i, bytes[fileOffset + i]);
        }
    }
}

