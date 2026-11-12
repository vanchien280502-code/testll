using System;
using System.IO;

public class Disasm269740 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        Dump("0x269740", 0x269740, 60);
        Dump("0x26A0D0", 0x26A0D0, 60);
    }

    static void Dump(string label, int rva, int len) {
        int off = ToFileOffset(rva);
        Console.WriteLine("=== " + label + " (RVA 0x" + rva.ToString("X") + ") ===");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
        for (int i = 0; i < len; i++) {
            if (bytes[off + i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("Call at 0x{0:X} -> 0x{1:X}", rva + i, rva + i + 5 + rel);
            }
        }
    }
}

