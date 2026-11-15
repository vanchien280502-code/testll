using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\ff899a59-b976-4829-b89c-45786dfce2cb.jsonl");
        Console.WriteLine("First 15 lines of ff899a59:");
        for (int i = 0; i < Math.Min(15, lines.Length); i++) {
            Console.WriteLine(lines[i]);
        }
    }
}

