using System;
using System.IO;

public class Disasm4340 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x4340;
        int off = (rva - 0x1000) + 0x400; // .text section
        Console.WriteLine("=== 0x4340 ===");
        for (int i = 0; i < 48; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
        if (bytes[off] == 0xE9) {
            int rel = BitConverter.ToInt32(bytes, off + 1);
            Console.WriteLine("Jmp to 0x{0:X}", rva + 5 + rel);
        }
    }
}

