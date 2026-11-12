using System;
using System.IO;

public class CheckExactBytes {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x4D8F69 + 0x40; // around 0x4D8FA9
        int off = ToFileOffset(rva);
        for (int i = 0; i < 20; i++) {
            Console.WriteLine(string.Format("0x{0:X}: {1:X2}", rva + i, bytes[off + i]));
        }
    }
}

