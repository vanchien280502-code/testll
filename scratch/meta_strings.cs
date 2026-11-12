using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class MetaStrings {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int stringOffset = BitConverter.ToInt32(meta, 0x18);
        int stringSize = BitConverter.ToInt32(meta, 0x1C);
        Console.WriteLine("String table: offset 0x{0:X}, size {1}", stringOffset, stringSize);

        int end = stringOffset + stringSize;
        int curr = stringOffset;
        string[] searchTerms = new string[] {
            "quay", "wheel", "lucky", "vong", "event", "menu", "thuvanmay", "vanmay"
        };

        var matches = new List<string>();
        while (curr < end) {
            int len = 0;
            while (curr + len < end && meta[curr + len] != 0) len++;
            if (len > 0) {
                string s = Encoding.UTF8.GetString(meta, curr, len);
                string sl = s.ToLowerInvariant();
                foreach (var term in searchTerms) {
                    if (sl.Contains(term)) {
                        matches.Add(string.Format("0x{0:X}: {1}", curr - stringOffset, s));
                        break;
                    }
                }
            }
            curr += len + 1;
        }

        Console.WriteLine("Total matches: " + matches.Count);
        foreach (var m in matches) {
            Console.WriteLine(m);
        }
    }
}

