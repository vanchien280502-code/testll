using System;
using System.IO;

public class DisasmWheelFuncs {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Dump(string label, int rva, int len = 150) {
        int off = ToFileOffset(rva);
        Console.WriteLine("================ " + label + " (RVA 0x" + rva.ToString("X") + ") ================");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }

    public static void Main() {
        Dump("Function 1 (2987, 2988)", 0x500FF0, 160);
        Dump("Function 2 (2985, 2986)", 0x501420, 160);
    }
}

