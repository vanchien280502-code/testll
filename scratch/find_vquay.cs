using System;
using System.IO;
using System.Text;

public class FindVQuay {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        string[] targets = { "V.Quay", "V.quay", "v.quay", "Vòng quay", "Vòng Quay", "Vòng xoay", "Vong quay", "Vong xoay" };
        foreach (var t in targets) {
            byte[] u16 = Encoding.Unicode.GetBytes(t);
            byte[] u8 = Encoding.UTF8.GetBytes(t);

            for (int i = 0; i < bytes.Length - u16.Length; i++) {
                bool match = true;
                for (int j = 0; j < u16.Length; j++) {
                    if (bytes[i + j] != u16[j]) { match = false; break; }
                }
                if (match) {
                    int rva = (i >= 0x235600 && i < 0x129C000) ? (i - 0x235600) + 0x237000 : i;
                    Console.WriteLine("UTF16 '{0}' at offset 0x{1:X} (RVA 0x{2:X})", t, i, rva);
                }
            }

            for (int i = 0; i < bytes.Length - u8.Length; i++) {
                bool match = true;
                for (int j = 0; j < u8.Length; j++) {
                    if (bytes[i + j] != u8[j]) { match = false; break; }
                }
                if (match) {
                    int rva = (i >= 0x235600 && i < 0x129C000) ? (i - 0x235600) + 0x237000 : i;
                    Console.WriteLine("UTF8 '{0}' at offset 0x{1:X} (RVA 0x{2:X})", t, i, rva);
                }
            }
        }
    }
}

