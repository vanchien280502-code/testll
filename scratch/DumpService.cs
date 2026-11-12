using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // RVA 0x4E6800 to 0x4E6C00
        int startRva = 0x4E6800;
        int fileOffset = (startRva - 0x237000) + 0x235600;
        int len = 0x500;
        byte[] sub = new byte[len];
        Array.Copy(bytes, fileOffset, sub, 0, len);
        
        File.WriteAllBytes(@"scratch\item_service_dump.bin", sub);
        Console.WriteLine("Dumped 0x" + len.ToString("X") + " bytes from RVA 0x" + startRva.ToString("X"));
    }
}

