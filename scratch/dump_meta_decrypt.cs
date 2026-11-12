using System;
using System.IO;

public class DumpMetaDecrypt {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int fileOffset = 0x1A492F;
        Console.WriteLine("Around 0x1A492F (RVA 0x1A552F):");
        for (int i = -30; i < 200; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
            if ((i + 31) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

