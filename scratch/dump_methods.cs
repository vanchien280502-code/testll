using System;
using System.IO;

class Program {
    static int RvaToFo(int rva) {
        return (rva - 0x237000) + 0x235600;
    }

    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x2BD690;
        int fo = RvaToFo(rva);
        
        for (int i = 0; i < 0x2BDB60 - 0x2BD690; i++) {
            if (bytes[fo + i] == 0xBA && bytes[fo + i + 1] == 0x18 && bytes[fo + i + 2] == 0) {
                int atRva = rva + i;
                Console.WriteLine("mov edx, 24 at RVA 0x" + atRva.ToString("X"));
            }
        }
    }
}
