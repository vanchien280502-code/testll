using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\NativeBackend.cs");
        Console.WriteLine("--- Lines 2780-2795 ---");
        for (int i = 2780; i < 2795 && i < lines.Length; i++) {
            Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
        }
        Console.WriteLine("--- Lines 5525-5545 ---");
        for (int i = 5525; i < 5545 && i < lines.Length; i++) {
            Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
        }
    }
}

