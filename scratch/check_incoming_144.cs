using System;
using System.IO;

class Program {
    static void Main() {
        // Let's find incoming packet handler (Session.onMessage / MessageHandler)
        // In TGLL, packet handler switch is in Controller / MessageHandler
        // Let's search for case 144 (0x90) and case 145 (0x91) in GameAssembly.dll
        Console.WriteLine("Searching for incoming handlers of 144 and 145...");
    }
}

