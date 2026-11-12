using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int targetRva = 0x4340;
        Console.WriteLine("Searching for CALLs to 0x4340 across GameAssembly.dll...");
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8) {
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

