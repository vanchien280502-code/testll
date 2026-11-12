using System;
using System.IO;

class Program {
    static void DumpCaller(byte[] bytes, int rva, string name) {
        int off = (rva - 0x237000) + 0x235600;
        Console.WriteLine("=== {0} at RVA 0x{1:X6} ===", name, rva);
        for (int i = -32; i <= 32; i += 16) {
            Console.WriteLine("{0:+00;-00}: {1}", i, BitConverter.ToString(bytes, off + i, 16).Replace('-', ' '));
        }
    }

    static void Main() {
        string path = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        DumpCaller(bytes, 0x357AC5, "Caller 1");
        DumpCaller(bytes, 0x357B62, "Caller 2");
        DumpCaller(bytes, 0x3581D2, "Caller 3");
        DumpCaller(bytes, 0x3583AC, "Caller 4");
    }
}

