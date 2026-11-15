using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeBackend.cs");
        int idx = 0;
        while ((idx = text.IndexOf("TeamHook", idx)) != -1) {
            Console.WriteLine("Found TeamHook at {0}: {1}", idx, text.Substring(idx, Math.Min(200, text.Length - idx)));
            idx += 8;
        }
        idx = 0;
        while ((idx = text.IndexOf("TeamCode", idx)) != -1) {
            Console.WriteLine("Found TeamCode at {0}: {1}", idx, text.Substring(idx, Math.Min(200, text.Length - idx)));
            idx += 8;
        }
    }
}

