using System;
using System.IO;
using System.Text;

public class FastSearchHex {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        string[] words = { "Vứt", "vứt", "Bỏ", "bỏ", "Vứt bỏ", "vứt bỏ", "Bán", "bán", "Tách", "tách" };
        foreach (string w in words) {
            byte[] pattern = Encoding.Unicode.GetBytes(w);
            int count = 0;
            for (int i = 0; i < bytes.Length - pattern.Length; i += 2) {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++) {
                    if (bytes[i + j] != pattern[j]) { match = false; break; }
                }
                if (match) {
                    count++;
                    int rva = -1;
                    // convert offset to RVA
                    if (i >= 0x129C000 && i < 0x16A0400) rva = i - 0x129C000 + 0x129E000;
                    else if (i >= 0x235600 && i < 0x129C000) rva = i - 0x235600 + 0x237000;
                    else if (i >= 0x16A0400 && i < 0x1793200) rva = i - 0x16A0400 + 0x16A3000;
                    Console.WriteLine("Word '{0}' found at offset 0x{1:X} (RVA 0x{2:X})", w, i, rva);
                    if (count >= 5) break;
                }
            }
        }
    }
}

