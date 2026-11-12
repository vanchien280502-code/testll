using System;
using System.IO;

public class PdataLookup2 {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int peOffset = BitConverter.ToInt32(bytes, 0x3C);
        short numSections = BitConverter.ToInt16(bytes, peOffset + 6);
        short optHeaderSize = BitConverter.ToInt16(bytes, peOffset + 20);
        int sectionHeadersOffset = peOffset + 24 + optHeaderSize;

        int pdataRaw = 0, pdataSize = 0;
        for (int i = 0; i < numSections; i++) {
            int off = sectionHeadersOffset + i * 40;
            string name = System.Text.Encoding.ASCII.GetString(bytes, off, 8).Trim('\0');
            if (name == ".pdata") {
                pdataSize = BitConverter.ToInt32(bytes, off + 8);
                pdataRaw = BitConverter.ToInt32(bytes, off + 20);
                break;
            }
        }

        uint[] targets = { 0x2DA40E, 0x4D8BE0, 0x4D94DA, 0x4D9519, 0x501350, 0x5018A0, 0x269740, 0x26A0D0 };
        int count = pdataSize / 12;
        foreach (uint target in targets) {
            for (int i = 0; i < count; i++) {
                int off = pdataRaw + i * 12;
                uint begin = BitConverter.ToUInt32(bytes, off);
                uint end = BitConverter.ToUInt32(bytes, off + 4);
                if (target >= begin && target < end) {
                    Console.WriteLine("0x{0:X} in Func [0x{1:X} - 0x{2:X}] (Size: {3})", target, begin, end, end - begin);
                    break;
                }
            }
        }
    }
}

