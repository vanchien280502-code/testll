using System;
using System.IO;

public class Disasm3E {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x3E4DA0;
        int off = ToFileOffset(rva);
        Console.WriteLine("Disassembling 0x3E4DA0 (Function containing Opcode 106):");
        for (int i = 0; i < 200; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

