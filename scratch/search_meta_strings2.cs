using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        string path = @"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat";
        byte[] bytes = File.ReadAllBytes(path);
        string[] searchStrings = { "Vận may", "Chơi lại", "vòng quay", "Vòng quay", "vòng quay vàng" };
        foreach (string s in searchStrings) {
            byte[] pat = Encoding.UTF8.GetBytes(s);
            int found = 0;
            for (int i = 0; i < bytes.Length - pat.Length; i++) {
                bool m = true;
                for (int j = 0; j < pat.Length; j++) {
                    if (bytes[i + j] != pat[j]) { m = false; break; }
                }
                if (m) {
                    Console.WriteLine("Found \"{0}\" at offset 0x{1:X}", s, i);
                    found++;
                    if (found >= 5) break;
                }
            }
        }
    }
}
