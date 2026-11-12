using System;
using System.IO;
using System.Collections.Generic;

class Program {
    static void Main() {
        string logPath = @"c:\Users\chien\Downloads\langla-291\QLTK\logs\fbd574b1-6e16-4dcd-804b-06f190e5440b.jsonl";
        var lines = File.ReadAllLines(logPath);
        var opcodes = new Dictionary<int, int>();
        Console.WriteLine("Total lines: " + lines.Length);
        foreach (var line in lines) {
            if (line.Contains("\"user_packet_sent\"")) {
                int opIdx = line.IndexOf("\"opcode\":");
                if (opIdx >= 0) {
                    int comma = line.IndexOf(',', opIdx);
                    string opStr = line.Substring(opIdx + 9, comma - (opIdx + 9));
                    int op = int.Parse(opStr);
                    if (!opcodes.ContainsKey(op)) opcodes[op] = 0;
                    opcodes[op]++;
                }
            }
        }
        foreach (var kvp in opcodes) {
            Console.WriteLine("Opcode {0} (0x{0:X2}, sbyte {1}): count {2}", kvp.Key, (sbyte)kvp.Key, kvp.Value);
        }
    }
}

