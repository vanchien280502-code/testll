using System;
using System.IO;

class Program {
    static void Main() {
        string logPath = @"c:\Users\chien\Downloads\langla-291\QLTK\logs\fbd574b1-6e16-4dcd-804b-06f190e5440b.jsonl";
        var lines = File.ReadAllLines(logPath);
        foreach (var line in lines) {
            if (line.Contains("\"user_packet_sent\"") || line.Contains("bi_ma") || line.Contains("lucky")) {
                Console.WriteLine(line);
            }
        }
    }
}

