using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4D8BE0;
        int fileOff = (rva - 0x237000) + 0x235600;
        byte[] expected = { 0x44, 0x89, 0x4c, 0x24, 0x20, 0x44, 0x89, 0x44, 0x24, 0x18, 0x48, 0x89, 0x54, 0x24, 0x10, 0x48, 0x89, 0x4c, 0x24, 0x08 };
        bool ok = true;
        for (int i = 0; i < 20; i++) {
            if (b[fileOff + i] != expected[i]) { ok = false; break; }
        }
        Console.WriteLine("Prefix match in dll: " + ok);
    }
}

