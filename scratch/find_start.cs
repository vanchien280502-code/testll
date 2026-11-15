using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x496FA8;
        int fileOffset = rva - 0x1A00;
        Console.WriteLine("Searching backward from 0x{0:X} (FO 0x{1:X})...", rva, fileOffset);
        for (int i = 0; i < 2000; i++) {
            int pos = fileOffset - i;
            if (bytes[pos] == 0xCC && bytes[pos-1] == 0xC3) {
                int startRva = (pos + 1) + 0x1A00;
                Console.WriteLine("Found function start at RVA 0x{0:X} (-{1} bytes, FO 0x{2:X})", startRva, i - 1, pos + 1);
                Console.WriteLine("Bytes at start: " + BitConverter.ToString(bytes, pos + 1, 16));
                break;
            }
        }
    }
}

