using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        string metaPath = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat";
        byte[] meta = File.ReadAllBytes(metaPath);
        string[] search = { "Session", "sendMessage", "VongQuay", "Lucky", "Wheel", "Service", "Message" };
        foreach (var s in search) {
            byte[] pat = Encoding.UTF8.GetBytes(s);
            int count = 0;
            for (int i = 0; i < meta.Length - pat.Length; i++) {
                bool match = true;
                for (int j = 0; j < pat.Length; j++) {
                    if (meta[i + j] != pat[j]) { match = false; break; }
                }
                if (match) {
                    count++;
                }
            }
            Console.WriteLine(string.Format("String '{0}': found {1} occurrences", s, count));
        }
    }
}

