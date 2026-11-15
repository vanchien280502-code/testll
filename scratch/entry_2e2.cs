using System;
using System.IO;

public class Entry2E2 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        for (int rva = 0x2E29B0; rva <= 0x2E2A15; rva++) {
            int off = ToFileOffset(rva);
            Console.WriteLine("0x{0:X}: {1:X2}", rva, bytes[off]);
        }
    }
}

