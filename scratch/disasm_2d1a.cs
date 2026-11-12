using System;
using System.IO;

public class Disasm2D1A {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x2D1A50;
        int off = ToFileOffset(rva);
        Console.WriteLine("Disassembling 0x2D1A50 (Opcode 100):");
        for (int i = 0; i < 154; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

