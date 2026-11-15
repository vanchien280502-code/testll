using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Let's find callers of 0x374AD0 or where button click is defined
        // We know dialog buttons array is at popup + 0x128
        // Let's check where [dialog + 0x128] is populated in the dialog ctor!
        // In 0x496C30 or dialog ctor:
        // Let's search for "0x128" in the Lucky Wheel dialog methods!
        Console.WriteLine("Searching for 0x128 in Lucky Wheel code...");
        int rvaStart = 0x495000;
        int rvaEnd = 0x498000;
        int foStart = rvaStart - 0x1A00;
        for (int i = 0; i < rvaEnd - rvaStart; i++) {
            // mov [rsi + 0x128], rax or similar
            if (bytes[foStart + i] == 0x28 && bytes[foStart + i + 1] == 0x01 && bytes[foStart + i + 2] == 0x00 && bytes[foStart + i + 3] == 0x00) {
                int rva = rvaStart + i - 3; // approximate
                Console.WriteLine("Offset 0x128 accessed near RVA 0x{0:X}", rva);
            }
        }
    }
}

