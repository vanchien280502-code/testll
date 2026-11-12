using System;
using System.IO;

public class RawDumpVtable {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int fileOffset = 0x16C4DD8;
        Console.WriteLine("Raw 8-byte values around 0x{0:X}:", fileOffset);
        for (int i = -0x80; i <= 0x40; i += 8) {
            long val = BitConverter.ToInt64(bytes, fileOffset + i);
            Console.WriteLine("  Offset {0:X3}: 0x{1:X16}", i, val);
        }
    }
}

