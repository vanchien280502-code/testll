using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int textEnd = bytes.Length - 10;
        for (int i = 0; i < textEnd - 6; i++) {
            byte b1 = bytes[i];
            byte b2 = bytes[i+1];
            if (b1 == 0xFF && (b2 >= 0x90 && b2 <= 0x97)) {
                int disp = BitConverter.ToInt32(bytes, i + 2);
                if (disp >= 0x800 && disp <= 0x1500) {
                    int rva = (i - 0x235600) + 0x237000;
                    Console.WriteLine("Call at file 0x{0:X} (RVA 0x{1:X}) with disp 0x{2:X}", i, rva, disp);
                }
            }
        }
    }
}

