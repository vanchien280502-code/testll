using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class SearchDllStrings {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Check UTF-16 strings
        for (int i = 0; i < bytes.Length - 10; i += 2) {
            // Check for Unicode pattern
            if (bytes[i+1] == 0 && bytes[i+3] == 0 && bytes[i+5] == 0) {
                // find length
                int len = 0;
                while (i + len * 2 + 1 < bytes.Length && bytes[i + len * 2 + 1] == 0 && bytes[i + len * 2] >= 32 && bytes[i + len * 2] < 127) {
                    len++;
                }
                if (len >= 4 && len < 60) {
                    string s = Encoding.Unicode.GetString(bytes, i, len * 2);
                    string lower = s.ToLower();
                    if (lower.Contains("wheel") || lower.Contains("lucky") || lower.Contains("quay") || lower.Contains("spin")) {
                        Console.WriteLine(string.Format("UTF16 at 0x{0:X}: {1}", i, s));
                    }
                }
            }
        }
        // Also check UTF-8
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] >= 32 && bytes[i] < 127) {
                int len = 0;
                while (i + len < bytes.Length && bytes[i + len] >= 32 && bytes[i + len] < 127) {
                    len++;
                }
                if (len >= 4 && len < 60) {
                    string s = Encoding.ASCII.GetString(bytes, i, len);
                    string lower = s.ToLower();
                    if (lower.Contains("wheel") || lower.Contains("lucky") || lower.Contains("quay") || lower.Contains("spin")) {
                        Console.WriteLine(string.Format("ASCII at 0x{0:X}: {1}", i, s));
                    }
                }
                i += len;
            }
        }
    }
}

