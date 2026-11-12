using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x357C78;
        int off = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 8; i++) {
            uint val = BitConverter.ToUInt32(bytes, off + i * 4);
            Console.WriteLine("Case {0}: 0x{1:X8}", i, val);
        }
    }
}

