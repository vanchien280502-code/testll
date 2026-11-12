using System;
using System.IO;

public class PeInspectCallers {
    public static void Main() {
        string path = @"TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);

        int peOffset = BitConverter.ToInt32(bytes, 0x3C);
        short numSections = BitConverter.ToInt16(bytes, peOffset + 6);
        short optHeaderSize = BitConverter.ToInt16(bytes, peOffset + 20);
        int sectionHeadersOffset = peOffset + 24 + optHeaderSize;

        int[] vAddrs = new int[numSections];
        int[] vSizes = new int[numSections];
        int[] rawAddrs = new int[numSections];
        int[] rawSizes = new int[numSections];

        for (int i = 0; i < numSections; i++) {
            int off = sectionHeadersOffset + i * 40;
            vSizes[i] = BitConverter.ToInt32(bytes, off + 8);
            vAddrs[i] = BitConverter.ToInt32(bytes, off + 12);
            rawSizes[i] = BitConverter.ToInt32(bytes, off + 16);
            rawAddrs[i] = BitConverter.ToInt32(bytes, off + 20);
        }

        Func<int, int> rvaToRaw = (rva) => {
            for (int i = 0; i < numSections; i++) {
                if (rva >= vAddrs[i] && rva < vAddrs[i] + Math.Max(vSizes[i], rawSizes[i])) {
                    return rawAddrs[i] + (rva - vAddrs[i]);
                }
            }
            return -1;
        };

        int[] targets = { 0x3A3229, 0x4C7FD3, 0x4E5F5E, 0x4E6A10 };
        foreach (int rva in targets) {
            int raw = rvaToRaw(rva);
            Console.WriteLine("=== Context around RVA 0x{0:X} (Raw 0x{1:X}) ===", rva, raw);
            int start = Math.Max(0, raw - 40);
            int len = 80;
            for (int i = 0; i < len; i++) {
                int currRaw = start + i;
                if (currRaw == raw) Console.Write("[>>] ");
                Console.Write("{0:X2} ", bytes[currRaw]);
                if ((i + 1) % 16 == 0) Console.WriteLine();
            }
            Console.WriteLine("\n");
        }
    }
}

