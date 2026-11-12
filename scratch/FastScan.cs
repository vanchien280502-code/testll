using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Program {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetRva = 0x2e2aa0;
        int secStart = 0x235600;
        int secEnd = secStart + 0x10668BC;
        var list = new List<string>();

        for (int i = secStart; i < secEnd - 5; i++) {
            if (bytes[i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int currRva = (i - 0x235600) + 0x237000;
                int calledRva = currRva + 5 + rel;
                if (calledRva == targetRva) {
                    string op = "unknown";
                    for (int k = 1; k <= 30; k++) {
                        int pos = i - k;
                        if (pos < 0) break;
                        if (bytes[pos] == 0xB2) {
                            op = "0x" + bytes[pos + 1].ToString("X2") + " (" + bytes[pos + 1] + ") [mov dl]";
                            break;
                        }
                        if (bytes[pos] == 0xBA && bytes[pos + 2] == 0 && bytes[pos + 3] == 0 && bytes[pos + 4] == 0) {
                            op = "0x" + bytes[pos + 1].ToString("X2") + " (" + bytes[pos + 1] + ") [mov edx]";
                            break;
                        }
                        if (bytes[pos] == 0x33 && bytes[pos + 1] == 0xD2) {
                            op = "0x00 (0) [xor edx,edx]";
                            break;
                        }
                    }
                    list.Add("RVA 0x" + currRva.ToString("X") + " -> Opcode: " + op);
                }
            }
        }
        File.WriteAllLines(@"scratch\all_opcodes.txt", list);
        Console.WriteLine("Found " + list.Count + " opcodes.");
    }
}
