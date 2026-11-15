using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Search for 4C 8B 88 E0 02 00 00 (mov r9, [rax + 0x2E0])
        for (int i = 0; i < bytes.Length - 10; i++) {
            if (bytes[i] == 0x4C && bytes[i+1] == 0x8B && bytes[i+2] == 0x88 &&
                bytes[i+3] == 0xE0 && bytes[i+4] == 0x02 && bytes[i+5] == 0x00 && bytes[i+6] == 0x00) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Found mov r9, [rax + 0x2E0] at RVA 0x{0:X}", rva);
            }
        }
    }
}
