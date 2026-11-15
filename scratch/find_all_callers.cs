using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int off = 0x438;
        Console.WriteLine("Searching for vtable slots at offset 0x438...");
        // Let's find button click implementations
        // In GameAssembly.dll, let's look for call 0x374AD0 callers:
        int target = 0x374AD0;
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int rva = (i + 5) + 0x1A00;
                if (rva + rel == target) {
                    Console.WriteLine("Caller at RVA 0x{0:X}", rva - 5);
                }
            }
        }
    }
}

