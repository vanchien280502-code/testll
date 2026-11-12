using System;
using System.IO;
using System.Text;

class SearchStrings {
    static void Search(string path) {
        Console.WriteLine("Searching " + path);
        byte[] data = File.ReadAllBytes(path);
        // Let's search for "Vòng", "vong quay", "quay", "Thử vận"
        // Also let's search for bytes of "Vòng" in UTF8: 0x56, 0xC3, 0xB2, 0x6E, 0x67
        // Or TCVN3 / Windows-1258 / ASCII
        byte[] u8_vong = Encoding.UTF8.GetBytes("Vòng");
        byte[] u8_quay = Encoding.UTF8.GetBytes("quay");
        byte[] u8_mayman = Encoding.UTF8.GetBytes("vận may");

        for (int i = 0; i < data.Length - 10; i++) {
            // Check UTF-8 "quay"
            if (data[i] == 'q' && data[i+1] == 'u' && data[i+2] == 'a' && data[i+3] == 'y') {
                // print surrounding 50 bytes as UTF-8
                int start = Math.Max(0, i - 20);
                int len = Math.Min(data.Length - start, 60);
                string s = Encoding.UTF8.GetString(data, start, len);
                // filter printable
                bool printable = true;
                foreach (char c in s) {
                    if (c < 32 && c != '\r' && c != '\n' && c != '\t') { printable = false; break; }
                }
                if (printable && (s.IndexOf("vòng", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                  s.IndexOf("loại", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                  s.IndexOf("thử", StringComparison.OrdinalIgnoreCase) >= 0)) {
                    Console.WriteLine("Found at 0x{0:X}: {1}", i, s);
                }
            }
        }
    }

    static void Main() {
        Search(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        Search(@"TGLL_minCPU_v2\GameAssembly.dll");
    }
}

