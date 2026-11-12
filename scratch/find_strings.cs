using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class FindStrings {
    public static void Main() {
        string[] files = { @"TGLL_minCPU_v2\GameAssembly.dll", @"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat" };
        string[] targets = { "vứt", "Vứt", "vut", "Vut", "Bỏ", "bỏ", "Tách", "tách", "Cất", "Rút", "Bán", "bán" };

        foreach (var file in files) {
            Console.WriteLine("Searching in " + file);
            byte[] bytes = File.ReadAllBytes(file);

            // Search UTF-8
            for (int i = 0; i < bytes.Length - 4; i++) {
                foreach (var t in targets) {
                    byte[] tBytes = Encoding.UTF8.GetBytes(t);
                    bool match = true;
                    for (int j = 0; j < tBytes.Length; j++) {
                        if (bytes[i + j] != tBytes[j]) { match = false; break; }
                    }
                    if (match) {
                        // Extract context
                        int start = Math.Max(0, i - 10);
                        int end = Math.Min(bytes.Length, i + tBytes.Length + 40);
                        string s = Encoding.UTF8.GetString(bytes, start, end - start).Replace('\0', ' ');
                        Console.WriteLine("  [UTF8 0x{0:X}] '{1}'", i, s.Trim());
                        break;
                    }
                }
            }

            // Search UTF-16LE
            for (int i = 0; i < bytes.Length - 4; i += 2) {
                foreach (var t in targets) {
                    byte[] tBytes = Encoding.Unicode.GetBytes(t);
                    bool match = true;
                    for (int j = 0; j < tBytes.Length; j++) {
                        if (bytes[i + j] != tBytes[j]) { match = false; break; }
                    }
                    if (match) {
                        int start = Math.Max(0, i - 20);
                        int end = Math.Min(bytes.Length, i + tBytes.Length + 60);
                        string s = Encoding.Unicode.GetString(bytes, start, end - start).Replace('\0', ' ');
                        Console.WriteLine("  [UTF16 0x{0:X}] '{1}'", i, s.Trim());
                        break;
                    }
                }
            }
        }
    }
}

