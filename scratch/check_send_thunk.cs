using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // In 0x2DB368:
        // call 0x4340 is Session.sendMessage!
        // Parameters to 0x4340:
        // rcx = 20 (0x14)
        // rdx = Message
        // r8 = Session
        Console.WriteLine("0x4340 takes: rcx=20, rdx=Message, r8=Session!");
    }
}

