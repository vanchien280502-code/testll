using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4D8BE0;
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Bytes at RVA 0x4D8BE0 (file 0x" + off.ToString("X") + "):");
        Console.WriteLine(BitConverter.ToString(b, off, 64).Replace('-', ' '));
    }
}

