using System;
using System.IO;

public class InspectClass {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int fileOffset = 0x16C4DD8;

        Console.WriteLine("Dumping around offset 0x{0:X}:", fileOffset);
        for (int i = -0x200; i < 0x80; i += 8) {
            long val = BitConverter.ToInt64(bytes, fileOffset + i);
            if (val >= 0x180000000L && val <= 0x182000000L) {
                // Check if target points to ASCII string
                int targetRaw = (int)(val - 0x180000000L); // rough RVA
                // Let's check if targetRaw is within file
                // If it's in .rdata (RVA 0x129E000 -> Raw 0x129C000):
                int raw = -1;
                if (targetRaw >= 0x129E000 && targetRaw < 0x16A23AC) raw = targetRaw - 0x129E000 + 0x129C000;
                if (raw >= 0 && raw < bytes.Length - 10) {
                    if (bytes[raw] >= 32 && bytes[raw] <= 126) {
                        string s = "";
                        for (int k = 0; k < 40 && bytes[raw + k] >= 32 && bytes[raw + k] <= 126; k++) {
                            s += (char)bytes[raw + k];
                        }
                        if (s.Length > 1) {
                            Console.WriteLine("  Offset {0:X}: 0x{1:X} -> '{2}'", i, val, s);
                        }
                    }
                }
            }
        }
    }
}
