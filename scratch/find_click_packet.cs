using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\ff899a59-b976-4829-b89c-45786dfce2cb.jsonl");
        Console.WriteLine("Lines in ff899a59 with opcodes other than 0 and 208:");
        for (int i = 0; i < lines.Length; i++) {
            string line = lines[i];
            if (line.Contains("\"user_packet_sent\"")) {
                if (!line.Contains("\"opcode\":0,") && !line.Contains("\"opcode\":208,")) {
                    Console.WriteLine("{0}: {1}", i, line);
                }
            }
        }
    }
}

