using System;
using System.IO;

public class DumpVtable2D {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int off = 0x16C2900;
        Console.WriteLine("Pointers around 0x16C2900:");
        for (int i = -0x80; i <= 0xA0; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, off + i);
            long rva = ptr - 0x180000000L;
            Console.WriteLine(string.Format("Offset {0:+000;-000} (0x{1:X}): 0x{2:X16} (RVA 0x{3:X})", i, off + i, ptr, rva));
        }
    }
}

