using System;
using System.IO;
using System.Text;

public class FindMetaInit {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        byte[] pat = Encoding.ASCII.GetBytes("global-metadata.dat");
        for (int i = 0; i < bytes.Length - pat.Length; i++) {
            bool match = true;
            for (int j = 0; j < pat.Length; j++) {
                if (bytes[i + j] != pat[j]) { match = false; break; }
            }
            if (match) {
                Console.WriteLine("String 'global-metadata.dat' at offset 0x{0:X}", i);
                // Search for references to this offset
                long imageBase = 0x180000000;
                long strRva = (i >= 0x235600 && i < 0x129C000) ? (i - 0x235600) + 0x237000 : (i - 0x1468E00 + 0x146B000);
                long strVa = imageBase + strRva;
                Console.WriteLine("VA: 0x{0:X}", strVa);
            }
        }
    }
}

