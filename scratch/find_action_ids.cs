using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int count = 0;
        // Search for 2985 (0x00000BA9) and 2986 (0x00000BAA)
        for (int i = 0; i <= b.Length - 4; i++) {
            int val = BitConverter.ToInt32(b, i);
            if (val == 2985 || val == 2986 || val == 2987 || val == 2988) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine(string.Format("Found {0} at file 0x{1:X} (RVA ~0x{2:X})", val, i, rva));
                count++;
            }
        }
        Console.WriteLine("Total: " + count);
    }
}
