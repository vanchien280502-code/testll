using System;
using System.IO;

class CheckOps {
    static void Main() {
        foreach (string line in File.ReadAllLines(@"scratch\all_opcodes.txt")) {
            if (line.Contains("Opcode  84") || line.Contains("Opcode  85") || line.Contains("Opcode  86")) {
                Console.WriteLine(line);
            }
        }
    }
}

