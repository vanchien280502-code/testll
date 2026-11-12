using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] rvas = new int[] { 0x4E6994, 0x4E6A5D, 0x4E6B44, 0x4FB715, 0x4CD518 };
        
        foreach (int rva in rvas) {
            int fileOffset = (rva - 0x237000) + 0x235600;
            int start = fileOffset - 40;
            int len = 120;
            byte[] sub = new byte[len];
            Array.Copy(bytes, start, sub, 0, len);
            Console.WriteLine("=== RVA 0x" + rva.ToString("X") + " (Offset 0x" + fileOffset.ToString("X") + ") ===");
            Console.WriteLine(BitConverter.ToString(sub).Replace('-', ' '));
        }
    }
}

