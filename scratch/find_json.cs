using System;
using System.IO;

class Program {
    static void Main() {
        foreach (var f in Directory.GetFiles(".", "*.json", SearchOption.AllDirectories)) {
            Console.WriteLine(f);
        }
    }
}
