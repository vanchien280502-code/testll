using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Looking for mov ..., [reg + 0x120]: 48 8B ... 20 01 00 00
        for (int i = 0x3CE000; i < 0x3D1000; i++) {
            int fo = (i - 0x237000) + 0x235600;
            if (bytes[fo] == 0x20 && bytes[fo+1] == 0x01 && bytes[fo+2] == 0x00 && bytes[fo+3] == 0x00) {
                int rva = i - 3;
                Console.WriteLine("Offset 0x120 access near RVA 0x{0:X}: {1}", rva, BitConverter.ToString(bytes, fo - 3, 7));
            }
        }
    }
}
