using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x16ED580;
        int off = ToFileOffset(rva);
        Console.WriteLine("Bytes at 0x16ED580: " + BitConverter.ToString(bytes, off, 16));
    }
}

