using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rvaStart = 0x496F50;
        int rvaEnd = 0x497220;
        int foStart = rvaStart - 0x1A00;
        int len = rvaEnd - rvaStart;
        Console.WriteLine("Instructions from 0x{0:X} to 0x{1:X}:", rvaStart, rvaEnd);
        for (int i = 0; i < len; i += 16) {
            int chunk = Math.Min(16, len - i);
            Console.WriteLine("0x{0:X6}: {1}", rvaStart + i, BitConverter.ToString(bytes, foStart + i, chunk));
        }
    }
}

