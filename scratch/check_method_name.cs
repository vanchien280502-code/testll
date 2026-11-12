using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");

    static void Main() {
        // Let's find the start of g_MethodPointers by scanning backwards from 0x16BEF70
        // Every entry is 0x180xxxxxx
        int ptrOffset = 0x16BEF70;
        int start = ptrOffset;
        while (start > 0) {
            long p = BitConverter.ToInt64(bytes, start - 8);
            if ((p >> 32) != 1) break; // not 0x180xxxxxx
            start -= 8;
        }
        Console.WriteLine("g_MethodPointers start at 0x{0:X}, index = {1}", start, (ptrOffset - start) / 8);

        int methodIndex = (ptrOffset - start) / 8;
        // In global-metadata.dat, Il2CppMethodDefinition offset is at header[0x38] (methodsOffset)
        int methodsOffset = BitConverter.ToInt32(meta, 0x38);
        int methodDefSize = 0x48; // usually 0x48 or 0x4c or 0x50 in Unity 2019/2020
        // Let's inspect methodDef at methodsOffset + methodIndex * size
        // Or find where nameIndex points to string table
        int stringOffset = BitConverter.ToInt32(meta, 0x18);
        for (int sz = 40; sz <= 80; sz += 4) {
            int off = methodsOffset + methodIndex * sz;
            if (off + 4 <= meta.Length) {
                int nameIdx = BitConverter.ToInt32(meta, off);
                if (nameIdx >= 0 && stringOffset + nameIdx < meta.Length) {
                    string s = GetString(meta, stringOffset + nameIdx);
                    if (s.Length > 2 && s.Length < 50 && IsAscii(s)) {
                        Console.WriteLine("Size {0} -> Method name: '{1}'", sz, s);
                    }
                }
            }
        }
    }

    static string GetString(byte[] data, int off) {
        int end = off;
        while (end < data.Length && data[end] != 0) end++;
        return System.Text.Encoding.UTF8.GetString(data, off, end - off);
    }
    static bool IsAscii(string s) {
        foreach (char c in s) if (c < 32 || c > 126) return false;
        return true;
    }
}

