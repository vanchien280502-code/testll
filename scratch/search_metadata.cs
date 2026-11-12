using System;
using System.IO;
using System.Text;

public class MetadataSearch {
    public static void Main() {
        string metaPath = @"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat";
        byte[] meta = File.ReadAllBytes(metaPath);
        int stringLiteralOffset = BitConverter.ToInt32(meta, 0x08);
        int stringLiteralSize = BitConverter.ToInt32(meta, 0x0C);
        int stringLiteralDataOffset = BitConverter.ToInt32(meta, 0x10);
        int stringLiteralDataSize = BitConverter.ToInt32(meta, 0x14);

        int stringOffset = BitConverter.ToInt32(meta, 0x18);
        int stringSize = BitConverter.ToInt32(meta, 0x1C);

        string[] keywords = { "quay", "vòng", "may mắn", "vận may", "vé " };
        Console.WriteLine("--- String Table ---");
        int end = Math.Min(meta.Length, stringOffset + stringSize);
        int curr = stringOffset;
        while (curr < end) {
            int len = 0;
            while (curr + len < end && meta[curr + len] != 0) len++;
            if (len > 0) {
                string s = Encoding.UTF8.GetString(meta, curr, len);
                foreach (var kw in keywords) {
                    if (s.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) {
                        Console.WriteLine("String at 0x{0:X}: '{1}'", curr - stringOffset, s);
                        break;
                    }
                }
            }
            curr += len + 1;
        }

        Console.WriteLine("\n--- String Literals ---");
        int count = stringLiteralSize / 8;
        for (int i = 0; i < count; i++) {
            int len = BitConverter.ToInt32(meta, stringLiteralOffset + i * 8);
            int dataOffset = BitConverter.ToInt32(meta, stringLiteralOffset + i * 8 + 4);
            if (len > 0 && dataOffset >= 0 && stringLiteralDataOffset + dataOffset + len <= meta.Length) {
                string s = Encoding.UTF8.GetString(meta, stringLiteralDataOffset + dataOffset, len);
                foreach (var kw in keywords) {
                    if (s.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) {
                        Console.WriteLine("Literal [{0}]: '{1}'", i, s);
                        break;
                    }
                }
            }
        }
    }
}
