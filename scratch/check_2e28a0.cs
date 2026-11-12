using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x2E28A0;
        int fileOff = (rva - 0x237000) + 0x235600;
        Console.WriteLine("Bytes at 0x2E28A0:");
        Console.WriteLine(BitConverter.ToString(b, fileOff, 32).Replace('-', ' '));
    }
}
