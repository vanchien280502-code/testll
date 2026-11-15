using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeBackend.cs");
        int idx = text.IndexOf("long[] ReadVector(");
        if (idx >= 0) {
            Console.WriteLine(text.Substring(idx, 400));
        }
    }
}
