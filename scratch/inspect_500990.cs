using System;
using System.IO;

class Inspect500990 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x500970;
        int len = 80;
        int off = ToFileOffset(rva);

        Console.WriteLine("Around 0x500990 (Opcode 0x56):");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

