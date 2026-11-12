using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int targetRva = 0x4340;
        int count = 0;
        int textStart = ToFileOffset(0x1000);
        int textEnd = ToFileOffset(0x237000); // .text
        // In x64, call rel32 is E8 xx xx xx xx
        // target = curRva + 5 + rel32
        // rel32 = target - (curRva + 5)
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8) {
                int rva = (i - 0x235600) + 0x237000;
                int rel = BitConverter.ToInt32(bytes, i + 1);
                if (rva + 5 + rel == targetRva) {
                    Console.WriteLine("call 0x4340 at RVA 0x{0:X} (file 0x{1:X})", rva, i);
                    count++;
                }
            }
        }
        Console.WriteLine("Total calls to 0x4340: {0}", count);
    }
}

