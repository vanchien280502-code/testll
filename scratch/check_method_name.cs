using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Search 4 bytes 0x004D8F60 or 0x004D8FB1
        byte[] pat = new byte[] { 0x60, 0x8F, 0x4D, 0x00, 0x00, 0x00, 0x00, 0x00 };
        for (int i = 0; i < bytes.Length - 8; i++) {
            if (bytes[i] == 0x60 && bytes[i+1] == 0x8F && bytes[i+2] == 0x4D && bytes[i+3] == 0x00) {
                Console.WriteLine(string.Format("Found 0x4D8F60 ref at file offset 0x{0:X}", i));
            }
        }
    }
}
