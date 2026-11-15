using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\NativeBackend.cs");
        for (int i = 4336; i < 4382; i++) {
            Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
        }
    }
}

