using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int targetRva = 0x4DAC08;
        for (int rva = 0x4D8BE0; rva < 0x4DAC08; rva++) {
            int off = ToFileOffset(rva);
            for (int i = 1; i <= 4; i++) {
                int disp = BitConverter.ToInt32(bytes, off + i);
                if (rva + i + 4 + disp == targetRva) {
                    Console.WriteLine("Found rip-rel ref at 0x{0:X}, opcode: {1:X2}", rva, bytes[off]);
                }
            }
        }
    }
}

