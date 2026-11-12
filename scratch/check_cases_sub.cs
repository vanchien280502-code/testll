using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4D8F30;
        int fileOff = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Bytes from 0x4D8F30 to 0x4D8F65:");
        for (int i = 0; i < 48; i += 16) {
            Console.WriteLine(string.Format("0x{0:X6}: {1}", rva + i, BitConverter.ToString(b, fileOff + i, 16).Replace('-', ' ')));
        }
    }
}

