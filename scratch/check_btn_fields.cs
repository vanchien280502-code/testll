using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In 0x374AD0:
        // mov rax, [rcx] -> vtable
        // mov rdx, [rax + 0x440]
        // call [rax + 0x438]
        // Let's find callers or implementations of [vtable + 0x438]
        // In GameAssembly.dll, let's look at what function is at vtable + 0x438 for button classes:
        // Let's search for references to button fields or text:
        // In NativeBackend.cs line 5530:
        // child + 0x80 is action ID on child controls (different control type)
        // Here on dialog.buttons (+0x128), button + 0x14 is action ID!
        Console.WriteLine("Button action ID offset: 0x14");
    }
}

