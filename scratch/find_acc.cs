using System;
using System.IO;

class FindAcc {
    static void Main() {
        string text = File.ReadAllText(@"Data\accounts.json");
        int idx = text.IndexOf("b57a5c25-fef3-4015-88a4-bed7339946dd");
        if (idx >= 0) {
            int start = Math.Max(0, idx - 100);
            int len = Math.Min(text.Length - start, 500);
            Console.WriteLine(text.Substring(start, len));
        } else {
            Console.WriteLine("Not found!");
        }
    }
}

