using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4E6A10;
        int fileOffset = (rva - 0x237000) + 0x235600;
        
        int end = fileOffset;
        while (bytes[end] != 0xC3) end++;
        int len = (end - fileOffset) + 1;
        
        byte[] func = new byte[len];
        Array.Copy(bytes, fileOffset, func, 0, len);
        
        Console.WriteLine("Function at 0x" + rva.ToString("X") + " length " + len + " bytes:");
        Console.WriteLine(BitConverter.ToString(func).Replace('-', ' '));
    }
}

