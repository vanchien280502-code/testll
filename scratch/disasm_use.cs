using System;
using System.IO;

public class DisasmUse {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4FB6D0;
        int fileOffset = (rva - 0x237000) + 0x235600;

        Console.WriteLine("Dumping 0x4FB6D0 (NativeBackend UseItem target):");
        for (int i = 0; i < 96; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

