using System;
using System.IO;
using System.Text;

public class SearchDataFiles {
    public static void Main() {
        string[] targets = {
            "Chọn loại vòng quay",
            "Vòng quay vàng",
            "Vòng quay bạc",
            "Thử vận may",
            "Chơi lại",
            "Vận may",
            "Vòng Quay",
            "Vòng quay"
        };

        var files = Directory.GetFiles(@"TGLL_minCPU_v2\TheGioiLangLa_Data", "*.*", SearchOption.AllDirectories);
        foreach (var file in files) {
            byte[] bytes = File.ReadAllBytes(file);
            foreach (var t in targets) {
                byte[] u8 = Encoding.UTF8.GetBytes(t);
                byte[] u16 = Encoding.Unicode.GetBytes(t);
                Search(bytes, u8, "UTF8", t, file);
                Search(bytes, u16, "UTF16", t, file);
            }
        }
    }

    static void Search(byte[] bytes, byte[] pat, string enc, string text, string file) {
        for (int i = 0; i <= bytes.Length - pat.Length; i++) {
            bool m = true;
            for (int j = 0; j < pat.Length; j++) {
                if (bytes[i + j] != pat[j]) { m = false; break; }
            }
            if (m) {
                Console.WriteLine("{0} '{1}' found in {2} at offset 0x{3:X}", enc, text, Path.GetFileName(file), i);
            }
        }
    }
}

