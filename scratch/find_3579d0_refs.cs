using System;
using System.IO;

class Program {
    static void Main() {
        string path = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        int targetRva = 0x3579D0;
        Console.WriteLine("Searching for CALLs or vtable pointers to 0x3579D0...");
        long targetVA = 0x180000000L + targetRva;
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            if (BitConverter.ToInt64(bytes, i) == targetVA) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Pointer to 0x{0:X} at RVA 0x{1:X}", targetRva, rva);
            }
        }
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

