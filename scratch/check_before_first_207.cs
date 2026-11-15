using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"QLTK\logs\c01dfd0c-1bdf-4e97-a4f8-38a6a29373d5.jsonl");
        for (int i = 16490; i <= 16516; i++) {
            Console.WriteLine("{0}: {1}", i, lines[i]);
        }
    }
}

