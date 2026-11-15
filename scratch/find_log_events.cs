using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\59d76774-6a7f-4bc7-9225-5d22758a3b19.jsonl");
        foreach (var l in lines) {
            if (l.Contains("pid") || l.Contains("lucky_wheel") || l.Contains("game_started")) {
                Console.WriteLine(l);
            }
        }
    }
}
