using System;
using System.IO;

public class Inspect4F {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        // Let's check callers of 0x4f0300 and 0x4f2570
        long imageBase = 0x180000000;
        long target1 = imageBase + 0x4f0300;
        long target2 = imageBase + 0x4f2570;

        Console.WriteLine("Searching for function pointers near 0x4F0300 in GameAssembly.dll...");
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == target1) Console.WriteLine("target1 (0x4f0300) in vtable at 0x{0:X}", i);
            if (p == target2) Console.WriteLine("target2 (0x4f2570) in vtable at 0x{0:X}", i);
        }
    }
}

