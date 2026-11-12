using System;
using System.IO;
using System.Text;

public class FastSearchMeta {
    public static void Main() {
        string metaPath = @"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat";
        byte[] bytes = File.ReadAllBytes(metaPath);
        Console.WriteLine("Metadata size: {0} bytes", bytes.Length);

        // Search UTF-8 and UTF-16
        string[] words = { "Vứt", "vứt", "Bỏ", "bỏ", "Vứt bỏ", "vứt bỏ", "Bán", "bán", "Tách", "tách", "Khóa", "khóa" };
        foreach (string w in words) {
            byte[] patUtf8 = Encoding.UTF8.GetBytes(w);
            for (int i = 0; i < bytes.Length - patUtf8.Length; i++) {
                bool match = true;
                for (int j = 0; j < patUtf8.Length; j++) {
                    if (bytes[i + j] != patUtf8[j]) { match = false; break; }
                }
                if (match) {
                    // print context
                    int start = Math.Max(0, i - 10);
                    int len = Math.Min(bytes.Length - start, 40);
                    string ctx = Encoding.UTF8.GetString(bytes, start, len).Replace('\0', ' ');
                    Console.WriteLine("[UTF-8 0x{0:X}] '{1}' -> ctx: \"{2}\"", i, w, ctx);
                    break;
                }
            }
        }
    }
}

