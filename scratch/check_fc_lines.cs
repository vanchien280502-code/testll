using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\NativeBackend.cs");
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("fc+0x90") || lines[i].Contains("fc+0x88") || lines[i].Contains("fc + 0x90") || lines[i].Contains("fc + 0x88")) {
                Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
            }
        }
    }
}

