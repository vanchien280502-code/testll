using System;
using System.IO;

public class Disasm2E28A0All {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x2E28A0;
        int off = ToFileOffset(rva);
        Console.WriteLine("=== 0x2E28A0 ===");
        for (int i = 0; i < 150; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
        // Print calls
        for (int i = 0; i < 150; i++) {
            if (bytes[off + i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("Call at 0x{0:X} -> 0x{1:X}", rva + i, rva + i + 5 + rel);
            }
        }
    }
}

