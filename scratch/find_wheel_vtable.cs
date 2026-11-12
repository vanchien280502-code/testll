using System;
using System.IO;

public class FindWheelVtable {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] targets = { 0x500FF0, 0x501420 };
        foreach (int t in targets) {
            long targetVa = 0x180000000L + t;
            byte[] p = BitConverter.GetBytes(targetVa);
            for (int i = 0; i < bytes.Length - 8; i += 8) {
                bool found = true;
                for (int j = 0; j < 8; j++) if (bytes[i + j] != p[j]) { found = false; break; }
                if (found) {
                    Console.WriteLine("Found pointer to 0x{0:X} at file offset 0x{1:X}", t, i);
                }
            }
        }
    }
}

