using System;
using System.IO;

class Program {
    static void Main() {
        // Let's find global-metadata.dat
        string[] files = Directory.GetFiles(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2", "global-metadata.dat", SearchOption.AllDirectories);
        if (files.Length == 0) {
            Console.WriteLine("global-metadata.dat not found");
            return;
        }
        Console.WriteLine("Found metadata: " + files[0]);
    }
}

