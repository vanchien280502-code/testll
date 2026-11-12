using System;
using System.IO;

public class FindMetaRef {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetOff = 0x15A84F0;
        int targetRva = (targetOff - 0x1468E00) + 0x146B000; // 0x15AA6F0

        // Search for lea rcx/rdx, [rip + disp]
        // 48 8D 0D / 15 / ...
        for (int i = 0x400; i < 0x235600 - 7; i++) {
            if (bytes[i] == 0x48 && bytes[i+1] == 0x8D) {
                int rel = BitConverter.ToInt32(bytes, i + 3);
                int currRva = (i - 0x400) + 0x1000;
                int resolvedRva = currRva + 7 + rel;
                if (resolvedRva == targetRva) {
                    Console.WriteLine("Ref to 'global-metadata.dat' at RVA 0x{0:X} (offset 0x{1:X})", currRva, i);
                }
            }
        }
    }
}

