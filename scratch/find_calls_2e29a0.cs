using System;
using System.IO;

class Program {
    static void Main() {
        string path = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        int targetRva = 0x2E29A0;
        Console.WriteLine("Searching for CALLs to 0x2E29A0 across GameAssembly.dll...");
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8) { // CALL rel32
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int rvaOfNext = (i + 5 - 0x235600) + 0x237000;
                int destRva = rvaOfNext + rel;
                if (destRva == targetRva) {
                    int callerRva = (i - 0x235600) + 0x237000;
                    Console.WriteLine("CALL at RVA 0x{0:X6}", callerRva);
                }
            }
        }
    }
}

