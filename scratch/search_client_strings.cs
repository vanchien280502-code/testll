using System;
using System.IO;
using System.Text;

public class SearchClientStrings {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");

    public static void Main() {
        string[] targets = { "Vận may", "Chơi lại", "Thử vận may", "Nhận thưởng mỗi tuần" };
        foreach (var t in targets) {
            byte[] pat = Encoding.UTF8.GetBytes(t);
            Console.WriteLine("Searching for: " + t);
            for (int i = 0; i <= bytes.Length - pat.Length; i++) {
                bool match = true;
                for (int j = 0; j < pat.Length; j++) {
                    if (bytes[i + j] != pat[j]) { match = false; break; }
                }
                if (match) {
                    Console.WriteLine("  Found in GameAssembly.dll at 0x{0:X}", i);
                }
            }
        }
    }
}

