using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int target = 0x4A1FF0;
        long target64 = 0x180000000L + target;
        byte[] b = BitConverter.GetBytes(target64);
        Console.WriteLine("Searching for pointer to 0x{0:X}...", target);
        for (int i = 0; i < bytes.Length - 8; i++) {
            bool ok = true;
            for (int k = 0; k < 8; k++) if (bytes[i+k] != b[k]) { ok = false; break; }
            if (ok) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("  Found at RVA 0x{0:X}", rva);
            }
        }
    }
}
