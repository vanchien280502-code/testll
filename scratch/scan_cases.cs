using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4D8BE0;
        int len = 0x500;
        int off = ToFileOffset(rva);

        Console.WriteLine("Scanning 0x4D8BE0 for jump table or cmp with 2985/2986 (0xBA9/0xBAA):");
        for (int i = 0; i < len; i++) {
            int cur = rva + i;
            // check for 2985
            if (i + 4 <= len) {
                int val = BitConverter.ToInt32(bytes, off + i);
                if (val == 2985 || val == 2986 || val == 0xBA9 || val == 0xBAA) {
                    Console.WriteLine("Found {0} at 0x{1:X}", val, cur);
                }
            }
        }
    }
}

