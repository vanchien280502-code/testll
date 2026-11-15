using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl");
        // Find line with opcode: 207
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("\"opcode\":207")) {
                Console.WriteLine("--- Found opcode 207 at index " + i + " ---");
                int start = Math.Max(0, i - 10);
                int end = Math.Min(lines.Length - 1, i + 10);
                for (int j = start; j <= end; j++) {
                    Console.WriteLine("{0}{1}: {2}", (j == i ? "==> " : "    "), j, lines[j]);
                }
                break; // print first one
            }
        }
    }
}

