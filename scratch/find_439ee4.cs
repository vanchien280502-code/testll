using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetRva = 0x439EE4;
        int targetFo = (targetRva - 0x237000) + 0x235600;
        for (int fo = targetFo; fo > targetFo - 0x10000; fo--) {
            if (bytes[fo] == 0xC3 && bytes[fo+1] == 0xCC) {
                int startFo = fo + 1;
                while (bytes[startFo] == 0xCC) startFo++;
                int rva = (startFo - 0x235600) + 0x237000;
                Console.WriteLine("Function start at RVA 0x{0:X}:", rva);
                for (int j = 0; j < 32; j += 16) {
                    Console.WriteLine("  {0:X6}: {1}", rva + j, BitConverter.ToString(bytes, startFo + j, 16));
                }
                break;
            }
        }
    }
}
