using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] callers = {
            0x4D8FB1, 0x4D9017, 0x4D907D, 0x4D90E3, 0x4D9159, 0x4D91CF, 0x4D926E, 0x4D92E4, 0x4D935A, 0x4D93D0, 0x4D9446, 0x4D94BC
        };
        foreach (int rva in callers) {
            int fileOff = (rva - 0x237000) + 0x235600;
            // Search backward up to 100 bytes for mov dl, X or mov edx, X followed by ctor 0x2E2AA0
            for (int i = fileOff - 100; i < fileOff; i++) {
                if (bytes[i] == 0xB2) {
                    byte op = bytes[i + 1];
                    Console.WriteLine("Caller at 0x{0:X}: opcode = {1} (0x{1:X2})", rva, op);
                    break;
                }
            }
        }
    }
}

