using System;
using System.IO;
using System.Text;

public class FindThuVanMay {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        string[] targets = {
            "Chọn loại vòng quay",
            "Chon loai vong quay",
            "Thử vận may",
            "Thu van may",
            "Chơi lại",
            "Choi lai",
            "Vận may",
            "Van may",
            "Vòng quay ma quái",
            "Vòng quay vàng"
        };

        foreach (var t in targets) {
            byte[] u16 = Encoding.Unicode.GetBytes(t);
            byte[] u8 = Encoding.UTF8.GetBytes(t);

            Search(bytes, u16, "UTF16", t);
            Search(bytes, u8, "UTF8", t);
        }
    }

    static void Search(byte[] bytes, byte[] pat, string enc, string text) {
        for (int i = 0; i < bytes.Length - pat.Length; i++) {
            bool match = true;
            for (int j = 0; j < pat.Length; j++) {
                if (bytes[i + j] != pat[j]) { match = false; break; }
            }
            if (match) {
                int rva = (i >= 0x235600 && i < 0x129C000) ? (i - 0x235600) + 0x237000 : i;
                Console.WriteLine("{0} '{1}' found at file offset 0x{2:X} (RVA 0x{3:X})", enc, text, i, rva);
            }
        }
    }
}

