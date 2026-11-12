using System;
using System.IO;

public class Disasm3A0170 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x3A0170;
        int off = ToFileOffset(rva);
        Console.WriteLine("Prologue of 0x3A0170:");
        for (int i = 0; i < 120; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

