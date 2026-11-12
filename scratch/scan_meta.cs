using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ScanMeta {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int start = -1;
        var list = new HashSet<string>();
        for (int i = 0; i < bytes.Length; i++) {
            if (bytes[i] == 0) {
                if (start >= 0 && i - start >= 3 && i - start < 100) {
                    try {
                        string s = Encoding.UTF8.GetString(bytes, start, i - start);
                        string lower = s.ToLower();
                        if (lower.Contains("quay") || lower.Contains("may mắn") || lower.Contains("vé") || lower.Contains("wheel") || lower.Contains("lucky")) {
                            list.Add(s);
                        }
                    } catch {}
                }
                start = i + 1;
            }
        }
        foreach (var s in list) {
            Console.WriteLine(s);
        }
    }
}
