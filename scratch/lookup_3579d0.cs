using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        byte[] meta = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        long targetVA = 0x1803579D0L;
        int targetFileOffset = -1;
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            if (BitConverter.ToInt64(bytes, i) == targetVA) {
                Console.WriteLine("Found VA 0x{0:X} at file offset 0x{1:X}", targetVA, i);
                targetFileOffset = i;
                break;
            }
        }
        if (targetFileOffset < 0) {
            Console.WriteLine("Target VA not found in table");
            return;
        }

        int ptrOffset = targetFileOffset;
        int start = ptrOffset;
        while (start > 0) {
            long p = BitConverter.ToInt64(bytes, start - 8);
            if ((p >> 32) != 1) break;
            start -= 8;
        }
        int methodIndex = (ptrOffset - start) / 8;
        Console.WriteLine("Method index: {0}", methodIndex);

        int methodsOffset = BitConverter.ToInt32(meta, 0x40); // 64 = 0x40
        int stringOffset = BitConverter.ToInt32(meta, 0x18);
        for (int sz = 32; sz <= 80; sz += 4) {
            int off = methodsOffset + methodIndex * sz;
            if (off + 4 <= meta.Length) {
                int nameIdx = BitConverter.ToInt32(meta, off);
                if (nameIdx >= 0 && stringOffset + nameIdx < meta.Length) {
                    int end = stringOffset + nameIdx;
                    while (end < meta.Length && meta[end] != 0) end++;
                    string s = System.Text.Encoding.UTF8.GetString(meta, stringOffset + nameIdx, end - (stringOffset + nameIdx));
                    Console.WriteLine("Size {0} -> Method name: '{1}'", sz, s);
                }
            }
        }
    }
}

