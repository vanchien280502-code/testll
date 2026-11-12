using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int secStart = 0x235600;
        int startRva = 0x4E6700;
        int endRva = 0x4E6D00;
        
        for (int rva = startRva; rva < endRva; rva++) {
            int off = (rva - 0x237000) + secStart;
            // Check for function prologue: 48 89 5C 24 ... or 40 53 / 48 83 EC ...
            // Or sub rsp, XX after mov [rsp+...], ...
            if ((bytes[off] == 0x48 && bytes[off+1] == 0x89 && bytes[off+2] == 0x5C && bytes[off+3] == 0x24) ||
                (bytes[off] == 0x40 && bytes[off+1] == 0x53 && bytes[off+2] == 0x48 && bytes[off+3] == 0x83 && bytes[off+4] == 0xEC) ||
                (bytes[off] == 0x48 && bytes[off+1] == 0x83 && bytes[off+2] == 0xEC)) {
                Console.WriteLine("Function at RVA 0x" + rva.ToString("X"));
            }
        }
    }
}

