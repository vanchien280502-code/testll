using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Message class slot is 0x16EE820
        // In check_2e28a0_full.cs:
        // mov rcx, [rip + ...] -> 0x16EE820
        // Let's find the vtable of Message
        // Pointer at 0x16EE820 in .data
        long msgClassPtr = BitConverter.ToInt64(bytes, ToFileOffset(0x16EE820));
        Console.WriteLine("msgClassPtr in file: 0x{0:X}", msgClassPtr);
    }
}
