using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl");
        int countNonZero = 0;
        foreach (string line in lines) {
            if (line.Contains("\"user_packet_sent\"") && !line.Contains("\"len\":0")) {
                Console.WriteLine(line);
                countNonZero++;
                if (countNonZero > 20) break;
            }
        }
        Console.WriteLine("Total user_packet_sent with len > 0: " + countNonZero);
    }
}

