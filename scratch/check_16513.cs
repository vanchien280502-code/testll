using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl");
        Console.WriteLine("Line 16513: " + lines[16513]);
        Console.WriteLine("Line 16514: " + lines[16514]);
        Console.WriteLine("Line 16515: " + lines[16515]);
    }
}

