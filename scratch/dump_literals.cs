using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class DumpLiterals {
    public static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int strLitOffset = BitConverter.ToInt32(meta, 8);
        int strLitSize = BitConverter.ToInt32(meta, 12);
        int strLitDataOffset = BitConverter.ToInt32(meta, 16);
        int strLitDataSize = BitConverter.ToInt32(meta, 20);

        Console.WriteLine(string.Format("strLit: 0x{0:X}, size: {1}, data: 0x{2:X}, dataSize: {3}", strLitOffset, strLitSize, strLitDataOffset, strLitDataSize));

        int count = strLitSize / 8; // sizeof(Il2CppStringLiteral) = 8 (length 4, dataIndex 4)
        for (int i = 0; i < count; i++) {
            int len = BitConverter.ToInt32(meta, strLitOffset + i * 8);
            int dataIndex = BitConverter.ToInt32(meta, strLitOffset + i * 8 + 4);
            if (len > 0 && dataIndex >= 0 && dataIndex + len <= strLitDataSize) {
                string s = Encoding.UTF8.GetString(meta, strLitDataOffset + dataIndex, len);
                string lower = s.ToLower();
                if (lower.Contains("quay") || lower.Contains("vé") || lower.Contains("may mắn") || lower.Contains("vòng")) {
                    Console.WriteLine(string.Format("[{0}] {1}", i, s));
                }
            }
        }
    }
}

