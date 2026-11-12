using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x357C78;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Jump table at 0x357C78:");
        for (int i = 0; i < 8; i++) {
            int rel = BitConverter.ToInt32(bytes, off + i * 4);
            int targetRva = 0x357A78 + rel;
            Console.WriteLine("Case {0} (cmd {1}): RVA 0x{2:X6}", i, i + 5, targetRva);
        }
    }
}

