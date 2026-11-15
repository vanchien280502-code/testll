using System;
using System.IO;

class Program {
    static void Main() {
        foreach (var f in Directory.GetFiles(".", "*.*", SearchOption.AllDirectories)) {
            if (f.EndsWith(".ps1") || f.EndsWith(".cmd") || f.EndsWith(".bat")) {
                string txt = File.ReadAllText(f);
                if (txt.Contains("NativeBackend.cs")) {
                    Console.WriteLine("{0}", f);
                }
            }
        }
    }
}
