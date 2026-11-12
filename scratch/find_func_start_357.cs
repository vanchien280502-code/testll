using System;
using System.IO;

class Program {
    static void Main() {
        string path = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        int rva = 0x357AC5;
        int off = (rva - 0x237000) + 0x235600;
        // Search backwards for prologue: 48 89 5C 24 or 40 53 or 55 or 48 83 EC
        int curr = off;
        while (curr > off - 2000) {
            if (bytes[curr] == 0xCC || bytes[curr] == 0xC3) {
                // start of func is curr + 1 (skipping 0xCC padding)
                int start = curr + 1;
                while (bytes[start] == 0xCC) start++;
                int funcRva = (start - 0x235600) + 0x237000;
                Console.WriteLine("Function start RVA: 0x{0:X6}", funcRva);
                break;
            }
            curr--;
        }
    }
}

