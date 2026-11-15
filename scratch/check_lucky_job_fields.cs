using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\NativeBackend.cs");
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("LuckyWheel")) {
                Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
            }
        }
    }
}

