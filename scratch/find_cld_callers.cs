using System;
using System.IO;
using System.Collections.Generic;

public class Program {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetRva = 0x2e28a0;
        int secStart = 0x235600;
        int secEnd = secStart + 0x10668BC;
        var list = new List<string>();

        for (int i = secStart; i < secEnd - 5; i++) {
            if (bytes[i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int currRva = (i - 0x235600) + 0x237000;
                int calledRva = currRva + 5 + rel;
                if (calledRva == targetRva) {
                    string arg = "unknown";
                    for (int k = 1; k <= 20; k++) {
                        int pos = i - k;
                        if (pos < 0) break;
                        if (bytes[pos] == 0xB1) { // mov cl, imm8
                            arg = "0x" + bytes[pos + 1].ToString("X2") + " (" + bytes[pos + 1] + ") [mov cl]";
                            break;
                        }
                    }
                    list.Add(string.Format("RVA 0x{0:X} -> 0x2E28A0 arg: {1}", currRva, arg));
                }
            }
        }
        foreach (var s in list) Console.WriteLine(s);
        Console.WriteLine("Total callers: " + list.Count);
    }
}
