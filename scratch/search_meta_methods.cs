using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class SearchMetaMethods {
    public static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int stringOffset = BitConverter.ToInt32(meta, 0x18);
        int stringSize = BitConverter.ToInt32(meta, 0x1C);
        Console.WriteLine("String literal offset: 0x{0:X}, size: 0x{1:X}", stringOffset, stringSize);

        // Also string pool at offset 0x20/0x24 or method table
        // Let's dump all header fields (first 32 ints)
        for (int i = 0; i < 32; i++) {
            int val = BitConverter.ToInt32(meta, i * 4);
            Console.WriteLine("Header[{0:D2}] (0x{1:X2}): 0x{2:X8} ({2})", i, i * 4, val);
        }
    }
}

