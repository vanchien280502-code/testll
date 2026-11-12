using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        long targetVa = 0x180000000L + 0x501420;
        Console.WriteLine("Pointers to 0x501420 (VA 0x" + targetVa.ToString("X") + "):");
        for (int i = 0; i <= b.Length - 8; i += 8) {
            if (BitConverter.ToInt64(b, i) == targetVa) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("  file 0x" + i.ToString("X") + " (RVA ~0x" + rva.ToString("X") + ")");
            }
        }
    }
}

