using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        string s1 = "Chơi lại";
        string s2 = "Vận may";
        string s3 = "Thử vận may";
        byte[] b1 = Encoding.UTF8.GetBytes(s1);
        byte[] b2 = Encoding.UTF8.GetBytes(s2);
        byte[] b3 = Encoding.UTF8.GetBytes(s3);
        Find(meta, b1, s1);
        Find(meta, b2, s2);
        Find(meta, b3, s3);
    }

    static void Find(byte[] data, byte[] pattern, string name) {
        for (int i = 0; i <= data.Length - pattern.Length; i++) {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++) {
                if (data[i + j] != pattern[j]) { match = false; break; }
            }
            if (match) {
                Console.WriteLine("Found '{0}' in metadata at offset 0x{1:X}", name, i);
            }
        }
    }
}

