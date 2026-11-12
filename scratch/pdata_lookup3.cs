using System;
using System.IO;

public class PdataLookup3 {
    public static void Main(string[] args) {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int peOffset = BitConverter.ToInt32(bytes, 0x3C);
        short optHeaderSize = BitConverter.ToInt16(bytes, peOffset + 20);
        int sectionHeadersOffset = peOffset + 24 + optHeaderSize;

        int pdataRaw = 0, pdataSize = 0;
        for (int i = 0; i < 20; i++) {
            int off = sectionHeadersOffset + i * 40;
            string name = System.Text.Encoding.ASCII.GetString(bytes, off, 8).Trim('\0');
            if (name == ".pdata") {
                pdataSize = BitConverter.ToInt32(bytes, off + 8);
                pdataRaw = BitConverter.ToInt32(bytes, off + 20);
                break;
            }
        }

        uint target = args.Length > 0 ? Convert.ToUInt32(args[0], 16) : 0x357A50;
        int count = pdataSize / 12;
        for (int i = 0; i < count; i++) {
            int off = pdataRaw + i * 12;
            uint begin = BitConverter.ToUInt32(bytes, off);
            uint end = BitConverter.ToUInt32(bytes, off + 4);
            if (target >= begin && target < end) {
                Console.WriteLine("0x{0:X} in Func [0x{1:X} - 0x{2:X}] (Size: {3})", target, begin, end, end - begin);
                return;
            }
        }
        Console.WriteLine("Not found in .pdata");
    }
}
