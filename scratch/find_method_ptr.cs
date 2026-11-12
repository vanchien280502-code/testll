using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        long targetVa = 0x180000000L + 0x4D8BE0;
        // Search method pointers array in .rdata or .data
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == targetVa) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Pointer to 0x4D8BE0 at file 0x{0:X} (RVA 0x{1:X})", i, rva);
            }
        }
    }
}
