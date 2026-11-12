using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // In il2cpp, what handles incoming messages?
        // Usually Session.onMessage(Message msg) or MessageHandler.onMessage(Message msg)
        // Let's search for references to string "onMessage" or "read"
        Console.WriteLine("Done");
    }
}

