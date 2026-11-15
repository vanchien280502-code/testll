using System;
using System.IO;

class Program {
    static void Main() {
        foreach (var f in Directory.GetFiles(".", "*.ps1", SearchOption.AllDirectories)) {
            string txt = File.ReadAllText(f);
            if (txt.Contains("NativeBackend")) {
                Console.WriteLine("In {0}", f);
            }
        }
    }
}
