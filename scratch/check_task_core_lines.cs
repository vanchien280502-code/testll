using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\NativeBackend.cs");
        int start = -1;
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("TaskPacketCore(")) start = i;
            if (start >= 0 && i < start + 100) {
                Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
            }
        }
    }
}

