using System;
using System.IO;

public class FindMetaRef2 {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetRva = 0x15AA6F0;

        for (int i = 0; i < bytes.Length - 7; i++) {
            if (bytes[i] == 0x48 && bytes[i+1] == 0x8D) { // lea
                int rel = BitConverter.ToInt32(bytes, i + 3);
                int currRva = 0;
                if (i >= 0x400 && i < 0x235600) currRva = (i - 0x400) + 0x1000;
                else if (i >= 0x235600 && i < 0x129C000) currRva = (i - 0x235600) + 0x237000;
                else continue;

                int resolvedRva = currRva + 7 + rel;
                if (resolvedRva == targetRva) {
                    Console.WriteLine("Ref to 'global-metadata.dat' at RVA 0x{0:X} (offset 0x{1:X})", currRva, i);
                }
            }
        }
    }
}

