using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x2E2AA0;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Message_ctor 0x2E2AA0:");
        for (int i = 0; i < 160; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

