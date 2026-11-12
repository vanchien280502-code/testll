using System;
using System.IO;
using System.Text;

public class SearchWheel {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        string[] keywords = { "Lucky", "lucky", "Wheel", "wheel", "Spin", "spin", "VongXoay", "VongQuay", "VeMayMan", "ve_may_man", "vong_xoay", "vong_quay" };

        Console.WriteLine("Searching for ASCII keywords in GameAssembly.dll...");
        foreach (var kw in keywords) {
            byte[] pat = Encoding.ASCII.GetBytes(kw);
            int count = 0;
            for (int i = 0; i < bytes.Length - pat.Length; i++) {
                bool m = true;
                for (int j = 0; j < pat.Length; j++) {
                    if (bytes[i + j] != pat[j]) { m = false; break; }
                }
                if (m) {
                    count++;
                    int start = Math.Max(0, i - 15);
                    int len = Math.Min(bytes.Length - start, 50);
                    string ctx = "";
                    for (int k = 0; k < len; k++) {
                        char c = (char)bytes[start + k];
                        ctx += (c >= 32 && c <= 126) ? c : '.';
                    }
                    int rva = i - 0x235600 + 0x237000;
                    Console.WriteLine("  [{0}] RVA 0x{1:X} (offset 0x{2:X}): \"{3}\"", kw, rva, i, ctx);
                    if (count >= 5) break;
                }
            }
        }
    }
}

