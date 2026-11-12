using System;
using System.IO;

class Inspect4DAC08 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4DAC08;
        int off = ToFileOffset(rva);
        Console.WriteLine("Jump Table at 0x4DAC08:");
        for (int i = 0; i < 20; i++) {
            int rel = BitConverter.ToInt32(bytes, off + i * 4);
            int targetRva = rva + rel;
            Console.WriteLine("  Case {0}: rel 0x{1:X}, target 0x{2:X}", i, rel, targetRva);
        }
    }
}

