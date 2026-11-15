using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Function preamble searching backward from 0x4D8F60
        int startRva = 0x4D8F50;
        int fileOffset = (startRva - 0x237000) + 0x235600;
        for (int i = 0; i < 150; i++) {
            Console.WriteLine("{0:X6}: {1:X2} {2:X2} {3:X2} {4:X2}", 
                startRva + i, 
                bytes[fileOffset + i], 
                bytes[fileOffset + i + 1], 
                bytes[fileOffset + i + 2], 
                bytes[fileOffset + i + 3]);
        }
    }
}

