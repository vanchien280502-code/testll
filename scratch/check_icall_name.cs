using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x12FC2BB;
        int off = ToFileOffset(rva);
        int end = off;
        while (end < bytes.Length && bytes[end] != 0) end++;
        Console.WriteLine("String at 0x{0:X}: '{1}'", rva, System.Text.Encoding.ASCII.GetString(bytes, off, end - off));
    }
}

