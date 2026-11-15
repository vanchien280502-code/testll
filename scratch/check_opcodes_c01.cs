using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl");
        foreach (string line in lines) {
            if (line.Contains("\"user_packet_sent\"")) {
                if (!line.Contains("\"opcode\":0,") && !line.Contains("\"opcode\":192,")) {
                    Console.WriteLine(line);
                }
            }
        }
    }
}

