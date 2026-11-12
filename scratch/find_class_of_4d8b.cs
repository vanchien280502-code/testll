using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // find pointer to 0x1816C2818 or search metadata for 0x4D8BE0
        long vtable = 0x180000000L + 0x16C2818;
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == vtable) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Pointer to vtable at offset 0x{0:X} (RVA 0x{1:X})", i, rva);
            }
        }
    }
}

