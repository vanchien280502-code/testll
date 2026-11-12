using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4D9012; // E8 24 B3 B2 FF
        int rel = BitConverter.ToInt32(bytes, ToFileOffset(rva + 1));
        int target = rva + 5 + rel;
        Console.WriteLine("Target RVA: 0x{0:X}", target);
        int off = ToFileOffset(target);
        for (int i = 0; i < 48; i++) Console.Write("{0:X2} ", bytes[off + i]);
        Console.WriteLine();
    }
}

