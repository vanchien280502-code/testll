using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\59d76774-6a7f-4bc7-9225-5d22758a3b19.jsonl");
        Console.WriteLine("Total lines: " + lines.Length);
        for (int i = 0; i < Math.Min(20, lines.Length); i++) {
            Console.WriteLine(lines[i]);
        }
        for (int i = Math.Max(0, lines.Length - 10); i < lines.Length; i++) {
            Console.WriteLine(lines[i]);
        }
    }
}
