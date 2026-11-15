using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\ff899a59-b976-4829-b89c-45786dfce2cb.jsonl");
        Console.WriteLine("Total lines now: " + lines.Length);
        // Print last 50 lines
        int start = Math.Max(0, lines.Length - 50);
        for (int i = start; i < lines.Length; i++) {
            if (lines[i].Contains("\"user_packet_sent\"") || lines[i].Contains("lucky") || lines[i].Contains("task_packet")) {
                Console.WriteLine("{0}: {1}", i, lines[i]);
            }
        }
    }
}

