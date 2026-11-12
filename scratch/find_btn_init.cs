using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x356900;
        int off = (rva - 0x237000) + 0x235600;
        // Search for instructions writing 5, 6, 7, 8 to [reg + 0x10] (e.g. C6 4x 10 05, C7 4x 10 05 00 00 00)
        // or passing 5, 6, 7, 8 to button constructor
        for (int i = 0; i < 0x356EE0 - 0x356900; i++) {
            if (bytes[off + i] == 0xC6 && bytes[off + i + 1] >= 0x40 && bytes[off + i + 1] <= 0x47 && bytes[off + i + 2] == 0x10) {
                int val = bytes[off + i + 3];
                Console.WriteLine("mov byte ptr [r+0x10], {0} at RVA 0x{1:X}", val, rva + i);
            }
            if (bytes[off + i] == 0xB2 && (bytes[off + i + 1] >= 5 && bytes[off + i + 1] <= 8)) {
                Console.WriteLine("mov dl, {0} at RVA 0x{1:X}", bytes[off + i + 1], rva + i);
            }
            if (bytes[off + i] == 0x41 && bytes[off + i + 1] == 0xB0 && (bytes[off + i + 2] >= 5 && bytes[off + i + 2] <= 8)) {
                Console.WriteLine("mov r8b, {0} at RVA 0x{1:X}", bytes[off + i + 2], rva + i);
            }
        }
    }
}

