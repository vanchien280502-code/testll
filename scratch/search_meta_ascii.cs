using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        string path = @"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat";
        byte[] bytes = File.ReadAllBytes(path);
        string[] searchStrings = { "Van may", "Choi lai", "vong quay", "Vong quay", "may man", "May man", "Wheel", "wheel" };
        foreach (string s in searchStrings) {
            byte[] pat = Encoding.ASCII.GetBytes(s);
            int found = 0;
            for (int i = 0; i < bytes.Length - pat.Length; i++) {
                bool m = true;
                for (int j = 0; j < pat.Length; j++) {
                    if (char.ToLower((char)bytes[i + j]) != char.ToLower((char)pat[j])) { m = false; break; }
                }
                if (m) {
                    // print context
                    int start = Math.Max(0, i - 10);
                    int len = Math.Min(bytes.Length - start, 40);
                    string ctx = "";
                    for (int k = 0; k < len; k++) {
                        char c = (char)bytes[start + k];
                        ctx += (c >= 32 && c <= 126) ? c : '.';
                    }
                    Console.WriteLine("Found \"{0}\" at 0x{1:X}: {2}", s, i, ctx);
                    found++;
                    if (found >= 5) break;
                }
            }
        }
    }
}

