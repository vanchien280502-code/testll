using System;
using System.IO;

public class PdataLookup {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int peOffset = BitConverter.ToInt32(bytes, 0x3C);
        short numSections = BitConverter.ToInt16(bytes, peOffset + 6);
        short optHeaderSize = BitConverter.ToInt16(bytes, peOffset + 20);
        int sectionHeadersOffset = peOffset + 24 + optHeaderSize;

        int pdataRaw = 0, pdataSize = 0, pdataRva = 0;
        for (int i = 0; i < numSections; i++) {
            int off = sectionHeadersOffset + i * 40;
            string name = System.Text.Encoding.ASCII.GetString(bytes, off, 8).Trim('\0');
            if (name == ".pdata") {
                pdataRva = BitConverter.ToInt32(bytes, off + 12);
                pdataSize = BitConverter.ToInt32(bytes, off + 8);
                pdataRaw = BitConverter.ToInt32(bytes, off + 20);
                break;
            }
        }
        Console.WriteLine(".pdata at RVA 0x{0:X}, Raw 0x{1:X}, Size 0x{2:X}", pdataRva, pdataRaw, pdataSize);

        int[] targets = { 0x501350, 0x5018A0 };
        int count = pdataSize / 12;
        foreach (int target in targets) {
            for (int i = 0; i < count; i++) {
                int off = pdataRaw + i * 12;
                uint begin = BitConverter.ToUInt32(bytes, off);
                uint end = BitConverter.ToUInt32(bytes, off + 4);
                if (target >= begin && target < end) {
                    Console.WriteLine("Target 0x{0:X} belongs to Function [0x{1:X} - 0x{2:X}] (Size: {3} bytes)",
                        target, begin, end, end - begin);
                    break;
                }
            }
        }
    }
}

