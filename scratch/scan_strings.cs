using System;
using System.IO;
using System.Text;

public class ScanMethodStrings {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int startRva = 0x39D190;
        int endRva = 0x3A4000;
        int startOffset = (startRva - 0x237000) + 0x235600;
        int endOffset = (endRva - 0x237000) + 0x235600;

        Console.WriteLine("Scanning instructions in range 0x{0:X} - 0x{1:X} for string loads...", startRva, endRva);

        for (int i = startOffset; i < endOffset - 7; i++) {
            // RIP-relative lea: 48 8D 0D / 48 8D 15 / 48 8D 05 xx xx xx xx
            if (bytes[i] == 0x48 && bytes[i + 1] == 0x8D && (bytes[i + 2] == 0x05 || bytes[i + 2] == 0x0D || bytes[i + 2] == 0x15)) {
                int rel = BitConverter.ToInt32(bytes, i + 3);
                int rvaCurrent = i - 0x235600 + 0x237000;
                long targetRva = (long)rvaCurrent + 7 + rel;
                // Check if target is in .rdata (0x129E000 - 0x16A23AC)
                if (targetRva >= 0x129E000 && targetRva < 0x16A23AC) {
                    int targetRaw = (int)(targetRva - 0x129E000 + 0x129C000);
                    // Check if it looks like Il2CppString (length at +0x10, unicode chars at +0x14)
                    int strLen = BitConverter.ToInt32(bytes, targetRaw + 0x10);
                    if (strLen > 0 && strLen < 100) {
                        // Read UTF-16
                        try {
                            string s = Encoding.Unicode.GetString(bytes, targetRaw + 0x14, strLen * 2);
                            Console.WriteLine("  [0x{0:X}] Il2CppString: \"{1}\"", rvaCurrent, s);
                        } catch {}
                    }
                }
            }
        }
    }
}

