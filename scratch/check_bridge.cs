using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeUiBridge.cs");
        Console.WriteLine(text.Substring(0, Math.Min(500, text.Length)));
    }
}
