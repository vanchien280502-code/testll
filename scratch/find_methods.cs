using System;
using System.IO;

public class FindClassMethods {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int fileOffset = 0x16C4DD8; // Vtable slot of 0x3A0170

        Console.WriteLine("Methods in vtable of class at 0x{0:X}:", fileOffset);
        for (int i = -0x80; i <= 0x80; i += 8) {
            long va = BitConverter.ToInt64(bytes, fileOffset + i);
            if (va >= 0x180000000L && va <= 0x182000000L) {
                int rva = (int)(va - 0x180000000L);
                Console.WriteLine("  Slot {0:X3}: RVA 0x{1:X}", i, rva);
            }
        }
    }
}

