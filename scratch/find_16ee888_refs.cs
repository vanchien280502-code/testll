using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Look at callers of 0x16EE888
        int targetRva = 0x16EE888;
        Console.WriteLine("Searching for RIP-relative references to 0x16EE888...");
        for (int i = 0; i < bytes.Length - 7; i++) {
            if (bytes[i] == 0x48 || bytes[i] == 0x4C) {
                if (bytes[i+1] == 0x8B && (bytes[i+2] == 0x05 || bytes[i+2] == 0x0D || bytes[i+2] == 0x15 || bytes[i+2] == 0x1D)) {
                    int disp = BitConverter.ToInt32(bytes, i + 3);
                    int rva = i - 0x235600 + 0x237000;
                    long target = (long)rva + 7 + disp;
                    if (target == targetRva) {
                        Console.WriteLine("Ref at RVA 0x{0:X} (FileOffset 0x{1:X})", rva, i);
                    }
                }
            }
        }
    }
}

