using System;
using System.IO;

public class FindVtable3A {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        long targetVa = 0x180000000L + 0x3A0170;
        byte[] p = BitConverter.GetBytes(targetVa);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            bool found = true;
            for (int j = 0; j < 8; j++) if (bytes[i + j] != p[j]) { found = false; break; }
            if (found) {
                Console.WriteLine("Found pointer to 0x3A0170 at file offset 0x{0:X}", i);
            }
        }
    }
}

