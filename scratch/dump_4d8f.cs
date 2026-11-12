using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4D8F69;
        int off = ToFileOffset(rva);
        Console.WriteLine("0x4D8F69:");
        Print(off, 120);
        Console.WriteLine("\n0x4D8FCF:");
        Print(ToFileOffset(0x4D8FCF), 120);
    }

    static void Print(int off, int count) {
        for (int i = 0; i < count; i += 16) {
            int chunk = Math.Min(16, count - i);
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(bytes, off + i, chunk).Replace('-', ' '));
        }
    }
}

