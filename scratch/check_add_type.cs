using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\AccountManager.ps1");
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("Add-Type") || lines[i].Contains("NativeBackend.cs")) {
                Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
            }
        }
    }
}
