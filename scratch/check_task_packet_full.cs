using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeBackend.cs");
        int idx = text.IndexOf("internal static byte[] TaskPacketCore(");
        if (idx >= 0) {
            Console.WriteLine(text.Substring(idx, 2000));
        }
    }
}

