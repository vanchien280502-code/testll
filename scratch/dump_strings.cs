using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        string metaPath = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat";
        byte[] meta = File.ReadAllBytes(metaPath);
        int stringOffset = BitConverter.ToInt32(meta, 24);
        int stringCount = BitConverter.ToInt32(meta, 28);
        Console.WriteLine(string.Format("String offset: 0x{0:X}, count: {1}", stringOffset, stringCount));

        // Read null-terminated strings
        int pos = stringOffset;
        int printed = 0;
        while (pos < stringOffset + stringCount && printed < 60) {
            int start = pos;
            while (pos < stringOffset + stringCount && meta[pos] != 0) pos++;
            string str = Encoding.UTF8.GetString(meta, start, pos - start);
            pos++; // skip null
            if (str.Length > 0) {
                Console.WriteLine("  " + str);
                printed++;
            }
        }
    }
}

