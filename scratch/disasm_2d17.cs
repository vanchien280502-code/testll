using System;
using System.IO;

public class Disasm2D17 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x2D1770;
        int off = ToFileOffset(rva);
        Console.WriteLine("Disassembling 0x2D1770 (Opcode 101):");
        for (int i = 0; i < 200; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

