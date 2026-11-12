using System;
using System.IO;

class InspectF4E {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0xF4E9B0;
        int len = 80;
        int off = ToFileOffset(rva);

        Console.WriteLine("At RVA 0xF4E9B0:");
        for (int i = 0; i < len; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
        for (int i = 0; i < len; i++) {
            if (bytes[off + i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("Call at 0x{0:X} -> 0x{1:X}", rva + i, rva + i + 5 + rel);
            }
            if (bytes[off + i] == 0xB2) {
                Console.WriteLine("mov dl, 0x{0:X2} ({0}) at 0x{1:X}", bytes[off + i + 1], rva + i);
            }
        }
    }
}

