using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // In Message class (game+0x16ee820):
        // Let's find Message vtable in dll or in a dump
        // In scratch, do we have dump_msg_vtable.cs?
        Console.WriteLine("Searching for Message vtable...");
    }
}

