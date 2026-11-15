using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\ff899a59-b976-4829-b89c-45786dfce2cb.jsonl");
        int count207 = 0;
        foreach (string line in lines) {
            if (line.Contains("\"user_packet_sent\"") && line.Contains("\"opcode\":207")) {
                Console.WriteLine(line);
                count207++;
            }
        }
        Console.WriteLine("Total user_packet_sent with opcode 207: " + count207);
    }
}

