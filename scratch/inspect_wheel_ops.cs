using System;
using System.IO;

public class InspectOpcodes {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    static int ToFileOffset(int rva) {
        return (rva - 0x237000) + 0x235600;
    }

    static void DumpFunc(string label, int rva, int len = 120) {
        int off = ToFileOffset(rva);
        Console.WriteLine("================ " + label + " (RVA 0x" + rva.ToString("X") + ") ================");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }

    public static void Main() {
        DumpFunc("Opcode 106 (0x6A)", 0x3E5270, 160);
        DumpFunc("Opcode 107 (0x6B)", 0x4AB580, 160);
        DumpFunc("Opcode 108 (0x6C)", 0x4720F0, 160);
        DumpFunc("Opcode 100 (0x64)", 0x2D1A80, 160);
        DumpFunc("Opcode 101 (0x65)", 0x2D17B0, 160);
        DumpFunc("Opcode 99 (0x63)", 0x2D4650, 160);
        DumpFunc("Opcode 98 (0x62)", 0x2D5060, 160);
        DumpFunc("Opcode 140 (0x8C)", 0x3A3330, 160);
        DumpFunc("Opcode 111 (0x6F)", 0x3A3AB0, 160);
    }
}

