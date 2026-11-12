using System;
using System.IO;

public class ScanSlots {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x3A3229; // Call to 0x4E6A10
        int fileOffset = (rva - 0x237000) + 0x235600;

        // Print instructions around 0x3A3200
        int start = fileOffset - 60;
        Console.WriteLine("Instructions before 0x3A3229:");
        for (int i = 0; i < 80; i++) {
            Console.Write("{0:X2} ", bytes[start + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

