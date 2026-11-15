using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In 0x374AD0:
        // mov rax, [rcx] (vtable)
        // call [rax + 0x438]
        // Let's find pointers in .data that could be vtables with 0x438 offset
        // But even better: let's check what 0x497410 does:
        Console.WriteLine("0x497410 is at RVA 0x497410");
        Console.WriteLine("0x374AD0 is at RVA 0x374AD0");
    }
}

