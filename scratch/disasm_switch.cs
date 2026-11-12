using System;
using System.IO;

public class DisasmSwitch {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x3A3099;
        int fileOffset = (rva - 0x237000) + 0x235600;

        Console.WriteLine("Dumping 0x3A3099 to 0x3A3400:");
        for (int i = 0; i < 600; i++) {
            int cur = fileOffset + i;
            int curRva = cur - 0x235600 + 0x237000;
            if (curRva == 0x3A3229) {
                Console.Write("\n[TARGET CALL AT 0x3A3229] ");
            }
            Console.Write("{0:X2} ", bytes[cur]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

