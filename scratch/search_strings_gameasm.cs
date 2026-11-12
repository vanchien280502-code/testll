using System;
using System.IO;
using System.Text;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    static void Main() {
        Search("Chọn");
        Search("vòng quay");
        Search("Vé vận may");
    }

    static void Search(string s) {
        byte[] u8 = Encoding.UTF8.GetBytes(s);
        byte[] u16 = Encoding.Unicode.GetBytes(s);

        int c8 = 0, c16 = 0;
        for (int i = 0; i < bytes.Length - u8.Length; i++) {
            bool ok = true;
            for (int j = 0; j < u8.Length; j++) if (bytes[i+j] != u8[j]) { ok = false; break; }
            if (ok) { Console.WriteLine("Found UTF-8 '{0}' at 0x{1:X}", s, i); c8++; if (c8 > 3) break; }
        }
        for (int i = 0; i < bytes.Length - u16.Length; i++) {
            bool ok = true;
            for (int j = 0; j < u16.Length; j++) if (bytes[i+j] != u16[j]) { ok = false; break; }
            if (ok) { Console.WriteLine("Found UTF-16 '{0}' at 0x{1:X}", s, i); c16++; if (c16 > 3) break; }
        }
    }
}

