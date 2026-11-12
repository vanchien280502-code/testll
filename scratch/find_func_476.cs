using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        for (int rva = 0x476680; rva >= 0x470000; rva--) {
            int off = ToFileOffset(rva);
            if (bytes[off] == 0x40 && bytes[off+1] == 0x55 && bytes[off+2] == 0x56 && bytes[off+3] == 0x57 ||
                bytes[off] == 0x48 && bytes[off+1] == 0x89 && bytes[off+2] == 0x5C && bytes[off+3] == 0x24 && bytes[off+4] == 0x08) {
                Console.WriteLine("Possible function start: 0x{0:X}", rva);
                break;
            }
        }
    }
}

