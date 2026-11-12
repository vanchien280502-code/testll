using System;
using System.IO;

public class DisasmRange {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int startRva = 0x3A08C0;
        int len = 0x250;
        int off = ToFileOffset(startRva);
        Console.WriteLine("Dumping from 0x3A08C0 to 0x3A0B10:");
        for (int i = 0; i < len; i++) {
            int curRva = startRva + i;
            if (curRva == 0x3A08C3) Console.Write("\n[FUNC 0x3A08C3 START] ");
            if (curRva == 0x3A0904) Console.Write("\n[Opcode 113 (0x71)] ");
            if (curRva == 0x3A09DD) Console.Write("\n[Opcode 115 (0x73)] ");
            if (curRva == 0x3A0A55) Console.Write("\n[Opcode 112 (0x70)] ");
            if (curRva == 0x3A0ACD) Console.Write("\n[Opcode 114 (0x72)] ");
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

