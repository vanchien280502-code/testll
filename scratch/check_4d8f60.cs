using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4D8F60;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine(string.Format("0x4D8F60:"));
        byte[] slice = new byte[160];
        Array.Copy(bytes, fileOffset, slice, 0, 160);
        for (int i = 0; i < 160; i += 16) {
            Console.WriteLine("{0:X4} (0x{1:X6}): {2}", i, rva + i, BitConverter.ToString(slice, i, 16).Replace('-', ' '));
        }
    }
}

