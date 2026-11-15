using System;
using System.IO;

public class Inspect357 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void DumpCaller(int rva, string name) {
        Console.WriteLine("================ " + name + " (0x" + rva.ToString("X") + ") ================");
        int off = ToFileOffset(rva - 30);
        for (int i = 0; i < 90; i++) {
            int cur = rva - 30 + i;
            byte b = bytes[off + i];
            if (cur == rva) Console.Write(">>> ");
            Console.Write("{0:X2} ", b);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
    }

    public static void Main() {
        DumpCaller(0x357AC5, "Caller 1");
        DumpCaller(0x357B62, "Caller 2");
        DumpCaller(0x3581D2, "Caller 3");
        DumpCaller(0x3583AC, "Caller 4");
    }
}

