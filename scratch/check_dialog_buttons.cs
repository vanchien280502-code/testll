using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Let's find references to action 207 or where button is created in the dialog
        // In check_slice.cs earlier, we saw 0x4970A0:
        // 0x4970AC: BA CF 00 00 00 (mov edx, 207)
        // Let's inspect 0x497090 - 0x497100
        int rva = 0x497090;
        int fo = rva - 0x1A00;
        Console.WriteLine("0x497090 - 0x497100:");
        for (int i = 0; i < 112; i += 16) {
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, fo + i, 16));
        }
    }
}

