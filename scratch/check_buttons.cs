using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Let's check RVA 0x5018ED where 2985 and 2986 are passed to button creation
        // In check_5018.exe:
        // [rsp+0x20] = 2986 (0xBAA)
        // r9d = 2985 (0xBA9)
        // call [r10+0x528]
        Console.WriteLine("Button creation at 0x5018E0:");
        // Button 1 is 2985 ("Chơi lại")
        // Button 2 is 2986 ("Vận may")
    }
}

