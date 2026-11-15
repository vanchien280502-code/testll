using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Let's find callers of 0x4340 or callers of Message methods
        // Let's disassemble around 0x2E28A0 (Message ctor)
        int rva = 0x2E28A0;
        int fileOffset = (rva - 0x237000) + 0x235600;
        Console.WriteLine(string.Format("0x2E28A0:"));
        byte[] slice = new byte[128];
        Array.Copy(bytes, fileOffset, slice, 0, 128);
        for (int i = 0; i < 128; i += 16) {
            Console.WriteLine("{0:X4}: {1}", i, BitConverter.ToString(slice, i, 16).Replace('-', ' '));
        }
    }
}

