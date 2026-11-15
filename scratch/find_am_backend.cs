using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\AccountManager.ps1");
        int idx = 0;
        while ((idx = text.IndexOf("NativeBackend.cs", idx)) != -1) {
            Console.WriteLine("At {0}: {1}", idx, text.Substring(Math.Max(0, idx - 50), 200));
            idx += 16;
        }
    }
}
