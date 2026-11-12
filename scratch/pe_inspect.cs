using System;
using System.IO;

public class PeInspector {
    public static void Main() {
        string path = @"TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);

        int peOffset = BitConverter.ToInt32(bytes, 0x3C);
        short numSections = BitConverter.ToInt16(bytes, peOffset + 6);
        short optHeaderSize = BitConverter.ToInt16(bytes, peOffset + 20);
        int sectionHeadersOffset = peOffset + 24 + optHeaderSize;

        Console.WriteLine("Sections ({0}):", numSections);
        int[] vAddrs = new int[numSections];
        int[] vSizes = new int[numSections];
        int[] rawAddrs = new int[numSections];
        int[] rawSizes = new int[numSections];
        string[] names = new string[numSections];

        for (int i = 0; i < numSections; i++) {
            int off = sectionHeadersOffset + i * 40;
            names[i] = System.Text.Encoding.ASCII.GetString(bytes, off, 8).Trim('\0');
            vSizes[i] = BitConverter.ToInt32(bytes, off + 8);
            vAddrs[i] = BitConverter.ToInt32(bytes, off + 12);
            rawSizes[i] = BitConverter.ToInt32(bytes, off + 16);
            rawAddrs[i] = BitConverter.ToInt32(bytes, off + 20);
            Console.WriteLine("  {0}: RVA 0x{1:X} - 0x{2:X}, RawOffset 0x{3:X}", names[i], vAddrs[i], vAddrs[i] + vSizes[i], rawAddrs[i]);
        }

        Func<int, int> rvaToRaw = (rva) => {
            for (int i = 0; i < numSections; i++) {
                if (rva >= vAddrs[i] && rva < vAddrs[i] + Math.Max(vSizes[i], rawSizes[i])) {
                    return rawAddrs[i] + (rva - vAddrs[i]);
                }
            }
            return -1;
        };

        Func<int, int> rawToRva = (raw) => {
            for (int i = 0; i < numSections; i++) {
                if (raw >= rawAddrs[i] && raw < rawAddrs[i] + rawSizes[i]) {
                    return vAddrs[i] + (raw - rawAddrs[i]);
                }
            }
            return -1;
        };

        // RVA 0x4e6a10
        int raw_4e6a10 = rvaToRaw(0x4e6a10);
        Console.WriteLine("RVA 0x4E6A10 -> Raw 0x{0:X}", raw_4e6a10);
        if (raw_4e6a10 != -1) {
            Console.WriteLine("Bytes at RVA 0x4E6A10 (Raw 0x{0:X}):", raw_4e6a10);
            for (int i = 0; i < 96; i++) {
                Console.Write("{0:X2} ", bytes[raw_4e6a10 + i]);
                if ((i + 1) % 16 == 0) Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Callers of RVA 0x4E6A10
        Console.WriteLine("Callers of RVA 0x4E6A10:");
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8 || bytes[i] == 0xE9) {
                int rvaSource = rawToRva(i);
                if (rvaSource == -1) continue;
                int rel = BitConverter.ToInt32(bytes, i + 1);
                long targetRva = (long)rvaSource + 5 + rel;
                if (targetRva == 0x4E6A10) {
                    Console.WriteLine("  Call from RVA 0x{0:X} (Raw 0x{1:X}) {2}", rvaSource, i, bytes[i] == 0xE8 ? "CALL" : "JMP");
                }
            }
        }
    }
}

