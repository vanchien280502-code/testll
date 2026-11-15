using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Look at callers of [rax + 0x298]
        // That's FF 90 98 02 00 00
        int count = 0;
        for (int i = 0; i < bytes.Length - 6; i++) {
            if (bytes[i] == 0xFF && bytes[i+1] == 0x90 && bytes[i+2] == 0x98 && bytes[i+3] == 0x02 && bytes[i+4] == 0x00 && bytes[i+5] == 0x00) {
                int rva = i - 0x235600 + 0x237000;
                Console.WriteLine("call [rax+0x298] at RVA 0x{0:X}", rva);
                count++;
                if (count > 20) break;
            }
        }
    }
}

