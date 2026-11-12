using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // In 0x35704F, we saw:
        // mov rdi, rcx
        // mov rdx, [rsp+0x78]
        // call 0x3579D0
        // What was in rdx?
        // Let's disassemble 0x357000 - 0x357050
        int rva = 0x357000;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("0x357000 - 0x357050:");
        for (int i = 0; i < 80; i += 16) {
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }
}

