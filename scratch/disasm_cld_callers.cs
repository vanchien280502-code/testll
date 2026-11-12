using System;
using System.IO;

public class DisasmCallersCld {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int[] rvas = { 0x3A1CA3, 0x3A2776, 0x3A32DD, 0x3E5D2D, 0x4D8F6D, 0x4D8FD3, 0x4D9039, 0x4D909F, 0x5102DE, 0x4AC362 };
        foreach (int rva in rvas) {
            int off = ToFileOffset(rva);
            Console.WriteLine("================ RVA 0x" + rva.ToString("X") + " ================");
            for (int i = -16; i < 16; i++) {
                if (i == 0) Console.Write("[CALL] ");
                Console.Write("{0:X2} ", bytes[off + i]);
            }
            Console.WriteLine("\n");
        }
    }
}

