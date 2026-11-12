using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C5DE0;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Dumping around file offset 0x{0:X} (RVA 0x{1:X}):", off, rva);
        for (int i = -32; i <= 48; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, off + i);
            long destRva = ptr - 0x180000000L;
            Console.WriteLine("{0:+00;-00}: 0x{1:X} (RVA 0x{2:X})", i, ptr, destRva);
        }
    }
}

