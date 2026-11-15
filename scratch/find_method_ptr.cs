using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Look for references to RVA 0x496F70 or 0x496F00 in GameAssembly.dll
        // In IL2CPP, method pointers are in method tables
        int targetRva = 0x496F00; // approx
        Console.WriteLine("Searching for method pointer around 0x496F00...");
        // Look in .data section for pointer to this function
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, i);
            // Check if pointer is an RVA or VA
            // VA base is usually 0x180000000
            if (ptr >= 0x180496E00 && ptr <= 0x180497100) {
                int rva = i - 0x235600 + 0x237000;
                Console.WriteLine("Found pointer to 0x{0:X} at file offset 0x{1:X}", ptr, i);
            }
        }
    }
}

