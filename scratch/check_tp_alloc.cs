using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeBackend.cs");
        int idx = text.IndexOf("taskPacketCode");
        while (idx >= 0) {
            int lineStart = text.LastIndexOf('\n', idx);
            int lineEnd = text.IndexOf('\n', idx);
            Console.WriteLine(text.Substring(lineStart + 1, lineEnd - lineStart - 1).Trim());
            idx = text.IndexOf("taskPacketCode", idx + 14);
        }
    }
}

