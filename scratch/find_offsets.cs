using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"QLTK\NativeBackend.cs");
        var matches = Regex.Matches(text, @"fc\s*\+\s*0x[0-9a-fA-F]+");
        var set = new HashSet<string>();
        foreach (Match m in matches) {
            set.Add(m.Value.Replace(" ", ""));
        }
        foreach (var s in set) {
            Console.WriteLine(s);
        }
    }
}

