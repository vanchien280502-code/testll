using System;
using System.IO;

class Find4D8B {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        long target = 0x180000000L + 0x4D8BE0;
        byte[] b = BitConverter.GetBytes(target);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == target) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Pointer to 0x4D8BE0 at offset 0x{0:X} (RVA 0x{1:X})", i, rva);
            }
        }
    }
}

