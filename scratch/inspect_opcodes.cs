using System;
using System.IO;

public class InspectOpcodes {
    public static void Main() {
        string[] lines = File.ReadAllLines(@"scratch\all_opcodes.txt");
        Console.WriteLine("Total lines in all_opcodes.txt: " + lines.Length);
        foreach (var l in lines) {
            Console.WriteLine(l);
        }
    }
}

