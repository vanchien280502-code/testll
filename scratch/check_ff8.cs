using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\ff899a59-b976-4829-b89c-45786dfce2cb.jsonl");
        Console.WriteLine("Total lines: " + lines.Length);
        int luckyCount = 0;
        foreach (string line in lines) {
            if (line.Contains("\"lucky_wheel") || line.Contains("\"task_packet") || line.Contains("\"opcode\":207")) {
                Console.WriteLine(line);
                luckyCount++;
                if (luckyCount > 50) break;
            }
        }
    }
}

