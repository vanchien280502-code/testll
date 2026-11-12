using System;
using System.IO;

public class DisasmWheelUI {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Dump(string label, int rva, int len = 120) {
        int off = ToFileOffset(rva);
        Console.WriteLine("================ " + label + " (RVA 0x" + rva.ToString("X") + ") ================");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }

    public static void Main() {
        Dump("RVA 0x501350", 0x501350, 160);
        Dump("RVA 0x5018A0", 0x5018A0, 160);
    }
}

