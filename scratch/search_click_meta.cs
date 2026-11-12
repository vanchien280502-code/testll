using System;
using System.IO;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        // Search for strings containing "performAction" or "click" or "pointer" or "touch" or "mouse"
        string[] targets = new[] { "performAction", "onPointerClick", "onClick", "onSelect", "bntClick", "Action" };
        foreach (var t in targets) {
            byte[] pat = System.Text.Encoding.UTF8.GetBytes(t);
            int count = 0;
            for (int i = 0; i <= meta.Length - pat.Length; i++) {
                bool match = true;
                for (int j = 0; j < pat.Length; j++) {
                    if (meta[i + j] != pat[j]) { match = false; break; }
                }
                if (match) {
                    count++;
                }
            }
            Console.WriteLine("String '{0}' found {1} times in metadata.", t, count);
        }
    }
}

