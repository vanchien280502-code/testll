using System;
using System.IO;

public class FindMetaBytes {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        byte[] p = { 0xF0, 0xA6, 0x5A }; // 0x15AA6F0
        for (int i = 0; i < bytes.Length - 3; i++) {
            if (bytes[i] == p[0] && bytes[i+1] == p[1] && bytes[i+2] == p[2]) {
                Console.WriteLine("Found at offset 0x{0:X}", i);
            }
        }
    }
}

