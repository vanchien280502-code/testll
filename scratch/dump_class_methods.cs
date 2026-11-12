using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int off = 0x16C5DE0; // file offset where 0x180356EE0 was found
        Console.WriteLine("Dumping method pointers around 0x16C5DE0:");
        for (int i = -40; i <= 60; i += 8) {
            long ptr = BitConverter.ToInt64(bytes, off + i);
            long rva = ptr - 0x180000000L;
            Console.WriteLine("{0:+00;-00}: 0x{1:X} (RVA 0x{2:X})", i, ptr, rva);
        }
    }
}

