using System;
using System.IO;

public class InspectCallerStrings {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] rvas = { 0x4C7FD3, 0x4E5F5E };
        foreach (int rva in rvas) {
            int fileOffset = (rva - 0x237000) + 0x235600;
            Console.WriteLine("=== Around RVA 0x{0:X} ===", rva);
            // Search nearby for slot loads or constants
            int start = fileOffset - 150;
            for (int i = 0; i < 300; i++) {
                int cur = start + i;
                // look for B9 xx 00 00 00 or BA xx 00 00 00 (common argument passing)
                if (bytes[cur] == 0xB9 || bytes[cur] == 0xBA) {
                    int val = BitConverter.ToInt32(bytes, cur + 1);
                    if (val >= 0 && val < 200) {
                        Console.WriteLine("  Offset +0x{0:X}: arg = {1} (0x{1:X})", i - 150, val);
                    }
                }
            }
        }
    }
}

