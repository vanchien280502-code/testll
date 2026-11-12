using System;
using System.IO;

public class Disasm3E527B {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x3E527B - 60;
        int off = ToFileOffset(rva);
        Console.WriteLine("Around 0x3E527B (Opcode 106):");
        for (int i = 0; i < 180; i++) {
            int curRva = rva + i;
            if (curRva == 0x3E527B) Console.Write("\n[Opcode 106 CALL] ");
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

