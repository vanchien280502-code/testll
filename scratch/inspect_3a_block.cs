using System;
using System.IO;

public class Inspect3A {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    static int ToFileOffset(int rva) {
        return (rva - 0x237000) + 0x235600;
    }

    public static void Main() {
        int[] targets = { 0x3A0904, 0x3A09DD, 0x3A0A55, 0x3A0ACD, 0x3A1801, 0x3A18C6, 0x3A3259, 0x3A3343, 0x3A3AC2 };
        foreach (int t in targets) {
            int off = ToFileOffset(t);
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("RVA 0x" + t.ToString("X") + " (Opcode call):");
            // print 40 bytes before and 40 bytes after
            for (int i = -32; i < 48; i++) {
                if (i == 0) Console.Write("[CALL] ");
                Console.Write("{0:X2} ", bytes[off + i]);
                if ((i + 33) % 16 == 0) Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}

