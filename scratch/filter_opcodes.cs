using System;
using System.IO;

class Program {
    static void Main() {
        var lines = File.ReadAllLines(@"c:\Users\chien\Downloads\langla-291\scratch\all_opcodes.txt");
        foreach (var l in lines) {
            if (l.Contains("0x90") || l.Contains("144") || l.Contains("0x36") || l.Contains(" 54 ") || l.Contains("0x9A") || l.Contains("0x9C") || l.Contains("0x85")) {
                Console.WriteLine(l);
            }
        }
    }
}

