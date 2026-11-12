using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x356900;
        int off = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 0x356EE0 - 0x356900; i++) {
            if (bytes[off + i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                int targetRva = rva + i + 5 + rel;
                if (targetRva == 0x16F760) {
                    Console.WriteLine("Allocate at RVA 0x{0:X}", rva + i);
                }
            }
        }
    }
}

