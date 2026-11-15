using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeBackend.cs");
        int idx = text.IndexOf("internal static byte[] Team(");
        if (idx >= 0) {
            Console.WriteLine(text.Substring(idx, 1500));
        }
    }
}

