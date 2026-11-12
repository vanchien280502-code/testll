using System;
using System.IO;

public class FindClassOfVtable {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Check for pointers to 0x1816C2880 or around it
        for (int delta = -0x100; delta <= 0x100; delta += 8) {
            long target = 0x1816C2900L + delta;
            byte[] p = BitConverter.GetBytes(target);
            for (int i = 0; i < bytes.Length - 8; i += 8) {
                bool found = true;
                for (int j = 0; j < 8; j++) if (bytes[i + j] != p[j]) { found = false; break; }
                if (found) {
                    Console.WriteLine(string.Format("Found pointer to 0x{0:X} at offset 0x{1:X}", target, i));
                }
            }
        }
    }
}

