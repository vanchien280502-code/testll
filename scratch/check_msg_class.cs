using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Message ctor is at 0x2E2AA0. Let's find Message class vtable!
        // In 0x2E2AA0 or 0x16F760 (AllocateMessage), the class pointer is passed or loaded.
        // Let's check 0x16F760:
        int off = ToFileOffset(0x16F760);
        for (int i = 0; i < 30; i++) Console.Write("{0:X2} ", bytes[off + i]);
        Console.WriteLine();
    }
}

