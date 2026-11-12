using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int target = 2991; // 0xBAF
        byte b0 = (byte)(target & 0xFF);
        byte b1 = (byte)((target >> 8) & 0xFF);

        for (int i = 0; i < bytes.Length - 4; i++) {
            if (bytes[i] == b0 && bytes[i+1] == b1 && bytes[i+2] == 0 && bytes[i+3] == 0) {
                int rva = i - 0x235600 + 0x237000;
                Console.WriteLine("Found 2991 (0xBAF) at file offset 0x{0:X}, RVA 0x{1:X}, prev byte: {2:X2} {3:X2}", i, rva, bytes[i-2], bytes[i-1]);
            }
        }
    }
}

