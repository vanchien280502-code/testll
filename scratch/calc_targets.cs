using System;
using System.IO;

class Program {
    static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int fileBase = 0x235600;
        int rvaBase = 0x237000;
        int off = (0x496FA8 - rvaBase) + fileBase;
        // At 0x496FA8: BA CF 00 00 00 (5 bytes)
        // 48 8B CB (3 bytes)
        // E8 xx xx xx xx (5 bytes)
        int rel = BitConverter.ToInt32(b, off + 9);
        int targetRva = (0x496FA8 + 13) + rel;
        Console.WriteLine("Call at 0x496FA8 targets RVA: 0x{0:X}", targetRva);

        // At 0x32637D:
        off = (0x32637D - rvaBase) + fileBase;
        rel = BitConverter.ToInt32(b, off + 9);
        targetRva = (0x32637D + 13) + rel;
        Console.WriteLine("Call at 0x32637D targets RVA: 0x{0:X}", targetRva);

        // At 0x4970AF:
        off = (0x4970AF - rvaBase) + fileBase;
        // Let's see what is called around 0x4970AF
    }
}

