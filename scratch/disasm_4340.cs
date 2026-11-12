using System;
using System.IO;

public class Disasm0x4340 {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int raw = (0x4340 - 0x1000) + 0x400; // 0x3740
        Console.WriteLine("At raw offset 0x3740 (RVA 0x4340):");
        for (int i = 0; i < 48; i++) {
            Console.Write("{0:X2} ", bytes[raw + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

