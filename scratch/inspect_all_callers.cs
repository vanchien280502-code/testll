using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] callers = new int[] { 0x496FB0, 0x497375, 0x4974C3 };
        foreach (int rva in callers) {
            int fo = rva - 0x1A00;
            Console.WriteLine("--- Caller RVA 0x{0:X} ---", rva);
            // Print 32 bytes before and after call
            Console.WriteLine("Before: " + BitConverter.ToString(bytes, fo - 32, 32));
            Console.WriteLine("At/After: " + BitConverter.ToString(bytes, fo, 32));
        }
    }
}

