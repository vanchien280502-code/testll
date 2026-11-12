using System;
using System.IO;

public class Disasm4DCases {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x4D8F69;
        int off = ToFileOffset(rva);
        Console.WriteLine("Cases 0-3 (RVA 0x4D8F69 - 0x4D9100):");
        for (int i = 0; i < 400; i++) {
            int curRva = rva + i;
            if (curRva == 0x4D8F69) Console.Write("\n[CASE 0: 0x9C (2985)] ");
            if (curRva == 0x4D8FCF) Console.Write("\n[CASE 1: 0x9A (2986)] ");
            if (curRva == 0x4D9035) Console.Write("\n[CASE 2: 0x9B (2987)] ");
            if (curRva == 0x4D909B) Console.Write("\n[CASE 3: 0x99 (2988)] ");
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

