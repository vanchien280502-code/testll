using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeBackend.cs");
        int idx = 0;
        while ((idx = text.IndexOf("teamCode", idx)) != -1) {
            int lineStart = text.LastIndexOf('\n', idx);
            int lineEnd = text.IndexOf('\n', idx);
            Console.WriteLine(text.Substring(lineStart + 1, lineEnd - lineStart - 1).Trim());
            idx += 8;
        }
    }
}

