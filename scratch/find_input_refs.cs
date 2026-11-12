using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Find pointers to 0x180000000 + RVA of those strings
        // Strings at 0x13049FB, etc. are in .rdata
        int strOff = 0x13049FB;
        int rva = (strOff - 0x235600) + 0x237000;
        long va = 0x180000000L + rva;
        Console.WriteLine("Searching for VA of string: 0x{0:X}", va);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            if (BitConverter.ToInt64(bytes, i) == va) {
                int refRva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Pointer to GetMouseButton at file 0x{0:X} (RVA 0x{1:X})", i, refRva);
            }
        }
    }
}

