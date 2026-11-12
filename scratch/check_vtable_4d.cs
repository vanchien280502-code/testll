using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetFileOff = 0x16C0E18;
        Console.WriteLine("Pointers around 0x16C0E18:");
        for (int i = -0x40; i <= 0x40; i += 8) {
            long val = BitConverter.ToInt64(b, targetFileOff + i);
            long rva = val - 0x180000000L;
            Console.WriteLine(string.Format("Offset {0:+000;-000} (0x{1:X}): 0x{2:X16} (RVA 0x{3:X})", i, targetFileOff + i, val, rva));
        }
    }
}

