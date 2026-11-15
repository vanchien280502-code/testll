using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\AccountManager.ps1");
        int idx = text.IndexOf("NativeBackend");
        while (idx >= 0) {
            int lineStart = text.LastIndexOf('\n', idx);
            int lineEnd = text.IndexOf('\n', idx);
            Console.WriteLine(text.Substring(lineStart + 1, lineEnd - lineStart - 1).Trim());
            idx = text.IndexOf("NativeBackend", idx + 14);
        }
    }
}
