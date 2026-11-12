using System;
using System.IO;

public class DisasmCompleteA10 {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4E6A10;
        int fileOffset = (rva - 0x237000) + 0x235600;

        Console.WriteLine("Dumping FULL 0x4E6A10 until RET (C3):");
        int offset = fileOffset;
        while (offset < fileOffset + 300) {
            byte b = bytes[offset];
            Console.Write("{0:X2} ", b);
            if ((offset - fileOffset + 1) % 16 == 0) Console.WriteLine();
            if (b == 0xC3) {
                Console.WriteLine("\n[FOUND RET]");
                break;
            }
            offset++;
        }
        Console.WriteLine();
    }
}

