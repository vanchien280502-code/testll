using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int[] callers = new int[] { 0x258ED6, 0x269FE6, 0x293A36, 0x2ADB10, 0x474162, 0x4D920B };
        foreach (var c in callers) {
            Console.WriteLine("=== Caller at 0x{0:X} ===", c);
            int off = ToFileOffset(c - 20);
            for (int i = 0; i < 40; i++) {
                Console.Write("{0:X2} ", bytes[off + i]);
            }
            Console.WriteLine();
        }
    }
}

