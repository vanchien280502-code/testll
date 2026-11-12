using System;
using System.IO;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        // Token 0xA0004AED -> index is 0x4AED - 1 or 0x4AED?
        // In il2cpp, type definitions table is at offset header[0x60] (typeDefinitionsOffset)
        int typeDefOffset = BitConverter.ToInt32(meta, 0x60);
        int stringOffset = BitConverter.ToInt32(meta, 0x18);
        int typeDefSize = 0x58; // usually 0x58 (88 bytes) or 0x60 or 0x68 in v24/v27
        Console.WriteLine("TypeDefinitions offset: 0x{0:X}", typeDefOffset);

        int tokenIndex = 0x4AED;
        // Let's search all type definitions to find where token == 0x4AED or index == 0x4AED
        for (int i = 0; i < 20000; i++) {
            int off = typeDefOffset + i * 88;
            if (off + 88 > meta.Length) break;
            int nameIdx = BitConverter.ToInt32(meta, off);
            int nsIdx = BitConverter.ToInt32(meta, off + 4);
            int token = BitConverter.ToInt32(meta, off + 84); // token is often at +84 or +80 or +88
            if (nameIdx >= 0 && stringOffset + nameIdx < meta.Length) {
                string name = GetString(meta, stringOffset + nameIdx);
                string ns = (nsIdx >= 0 && stringOffset + nsIdx < meta.Length) ? GetString(meta, stringOffset + nsIdx) : "";
                if (i == tokenIndex || i == tokenIndex - 1 || name.ToLower().Contains("wheel") || name.ToLower().Contains("quay")) {
                    Console.WriteLine("Index {0}: {1}.{2}", i, ns, name);
                }
            }
        }
    }

    static string GetString(byte[] data, int off) {
        int end = off;
        while (end < data.Length && data[end] != 0) end++;
        return System.Text.Encoding.UTF8.GetString(data, off, end - off);
    }
}

