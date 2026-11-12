using System;
using System.IO;
using System.Collections.Generic;

public class DumpAllOpcodes {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] ctors = { 0x2E28A0, 0x2E2AA0 };

        foreach (var ctorRva in ctors) {
            var opcodes = new SortedDictionary<int, List<int>>();
            for (int i = 0; i < bytes.Length - 5; i++) {
                if (bytes[i] == 0xE8) {
                    int rel = BitConverter.ToInt32(bytes, i + 1);
                    int rvaSource = i - 0x235600 + 0x237000;
                    long target = (long)rvaSource + 5 + rel;
                    if (target == ctorRva) {
                        int op = -1;
                        for (int back = 1; back < 25; back++) {
                            int pos = i - back;
                            if (pos >= 0 && bytes[pos] == 0xB2) {
                                op = bytes[pos + 1];
                                break;
                            } else if (pos >= 0 && bytes[pos] == 0xBA && bytes[pos + 2] == 0 && bytes[pos + 3] == 0 && bytes[pos + 4] == 0) {
                                op = bytes[pos + 1];
                                break;
                            } else if (pos >= 0 && bytes[pos] == 0xB9 && bytes[pos + 2] == 0 && bytes[pos + 3] == 0 && bytes[pos + 4] == 0) {
                                op = bytes[pos + 1];
                                break;
                            }
                        }
                        if (op >= 0) {
                            if (!opcodes.ContainsKey(op)) opcodes[op] = new List<int>();
                            opcodes[op].Add(rvaSource);
                        } else {
                            if (!opcodes.ContainsKey(-1)) opcodes[-1] = new List<int>();
                            opcodes[-1].Add(rvaSource);
                        }
                    }
                }
            }

            Console.WriteLine("Ctor RVA 0x{0:X}: Found {1} distinct opcodes ({2} total calls):", ctorRva, opcodes.Count,
                opcodes.Values.Count);
            foreach (var kvp in opcodes) {
                if (kvp.Key == -1) {
                    Console.WriteLine("  Opcode (dynamic register): {0} call(s)", kvp.Value.Count);
                    continue;
                }
                Console.Write("  Opcode 0x{0:X2} ({0,3}): {1} call(s) [", kvp.Key, kvp.Value.Count);
                for (int k = 0; k < Math.Min(3, kvp.Value.Count); k++) {
                    Console.Write("0x{0:X} ", kvp.Value[k]);
                }
                Console.WriteLine("]");
            }
            Console.WriteLine();
        }
    }
}
