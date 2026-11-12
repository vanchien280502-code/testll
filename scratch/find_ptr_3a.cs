using System;
using System.IO;

public class FindPointer3A {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Target RVA 0x3A0170 -> 64-bit address in module with ImageBase 0x180000000 = 0x1803A0170
        // Or relative RVA
        long va = 0x180000000L + 0x3A0170;
        byte[] pBytes = BitConverter.GetBytes(va);

        Console.WriteLine("Searching for VA 0x{0:X}...", va);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            bool m = true;
            for (int j = 0; j < 8; j++) {
                if (bytes[i + j] != pBytes[j]) { m = false; break; }
            }
            if (m) {
                int rva = i - 0x235600 + 0x237000;
                Console.WriteLine("  Found in binary at offset 0x{0:X} (RVA 0x{1:X})", i, rva);
            }
        }
    }
}

