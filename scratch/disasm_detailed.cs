using System;
using System.IO;

class DisasmDetailed {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Dump(int rva, int len, string label) {
        Console.WriteLine("--- " + label + " (RVA 0x" + rva.ToString("X") + ") ---");
        int off = ToFileOffset(rva);
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }

    static void Main() {
        Dump(0x4D94DA, 64, "Case 13 (Action 2998, Opcode 0x55)");
        Dump(0x4D9519, 64, "Case 14 (Action 2999, Opcode 0x54)");
        Dump(0x4D91ED, 40, "Case 6 (Action 2991)");
    }
}

