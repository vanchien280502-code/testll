using System;
using System.IO;

public class Disasm2DMethods {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Dump(string name, int rva, int len = 80) {
        int off = ToFileOffset(rva);
        Console.WriteLine("=== " + name + " (RVA 0x" + rva.ToString("X") + ") ===");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }

    public static void Main() {
        Dump("0x2D18F0", 0x2D18F0);
        Dump("0x2D1AF0", 0x2D1AF0);
        Dump("0x2D1B20", 0x2D1B20);
    }
}

