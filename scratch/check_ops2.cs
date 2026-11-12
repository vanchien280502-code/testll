using System;
using System.IO;

class CheckOps2 {
    static void Main() {
        foreach (string line in File.ReadAllLines(@"scratch\all_opcodes.txt")) {
            if (line.Contains("0x54") || line.Contains("0x55") || line.Contains("0x56")) {
                Console.WriteLine(line);
            }
        }
    }
}

