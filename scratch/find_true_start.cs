using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4E3A74;
        int fo = rva - 0x1A00;
        // Search backward for 0xCC (int 3 padding between functions)
        for (int i = 1; i < 0x2000; i++) {
            if (bytes[fo - i] == 0xCC && (bytes[fo - i + 1] == 0x40 || bytes[fo - i + 1] == 0x48 || bytes[fo - i + 1] == 0x55)) {
                int startRva = (fo - i + 1) + 0x1A00;
                Console.WriteLine("True function start at RVA 0x{0:X} (-0x{1:X})", startRva, i - 1);
                Console.WriteLine("Bytes: " + BitConverter.ToString(bytes, fo - i + 1, 32));
                break;
            }
        }
    }
}
