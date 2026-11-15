using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x496F90;
        int fileOffset = (rva - 0x237000) + 0x235600;
        // Search backward for function prologue (48 89 5C 24 or 40 55 or 48 83 EC or push rbp)
        for (int i = 0; i < 400; i++) {
            int pos = fileOffset - i;
            if (bytes[pos] == 0xCC || (bytes[pos-1] == 0xC3 && (bytes[pos] == 0x40 || bytes[pos] == 0x48 || bytes[pos] == 0x55 || bytes[pos] == 0x56 || bytes[pos] == 0x57))) {
                int funcRva = (pos - 0x235600) + 0x237000;
                Console.WriteLine("Function start found at RVA 0x{0:X}", funcRva);
                break;
            }
        }
    }
}

