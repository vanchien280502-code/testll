using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x496F90;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine("0x496F90 - 0x497250:");
        byte[] slice = new byte[700];
        Array.Copy(bytes, fileOffset, slice, 0, 700);
        for (int i = 0; i < 700; i += 16) {
            int len = Math.Min(16, 700 - i);
            Console.WriteLine("{0:X4} (0x{1:X6}): {2}", i, rva + i, BitConverter.ToString(slice, i, len).Replace('-', ' '));
        }
    }
}

