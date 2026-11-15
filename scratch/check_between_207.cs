using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl");
        int last207 = -1;
        int pairCount = 0;
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("\"opcode\":207")) {
                if (last207 != -1) {
                    Console.WriteLine("--- Between 207 at line " + last207 + " and line " + i + ":");
                    for (int j = last207 + 1; j < i; j++) {
                        if (!lines[j].Contains("\"opcode\":0,")) {
                            Console.WriteLine("  " + lines[j]);
                        }
                    }
                    pairCount++;
                    if (pairCount >= 10) break;
                }
                last207 = i;
            }
        }
    }
}

