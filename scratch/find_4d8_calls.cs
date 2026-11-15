using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // FF 90 D8 04 00 00 -> call [rax + 0x4D8]
        byte[] pat = new byte[] { 0xFF, 0x90, 0xD8, 0x04, 0x00, 0x00 };
        for (int i = 0; i < bytes.Length - 6; i++) {
            bool m = true;
            for (int j = 0; j < pat.Length; j++) {
                if (bytes[i + j] != pat[j]) { m = false; break; }
            }
            if (m) {
                int rva = i + 0x1A00;
                Console.WriteLine("call [rax + 0x4D8] at RVA 0x{0:X}", rva);
            }
        }
    }
}
