using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // Look for methods near 0x356EE0 - 0x358500
        // Let's dump all functions in this class
        int startRva = 0x356000;
        int endRva = 0x359000;
        // Search for constants 5, 6, 7, 8 being written into a field
        Console.WriteLine("Searching for button setup in [0x{0:X} - 0x{1:X}]...", startRva, endRva);
    }
}

