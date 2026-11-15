using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Earlier in check_vtable_slice.cs:
        // Offset 0 (0x16C1D20): RVA 0x496C30
        // Offset +16 (0x16C1D30): RVA 0x374AD0
        // Where is 0x5C8 from?
        // In 0x497455:
        // mov rax, [rbx] -> rax is the vtable pointer!
        // call [rax + 0x5C8]
        // Let's find the vtable base for this class:
        // In IL2CPP, an object's first 8 bytes [obj] points to Il2CppClass.
        // And [rax + 0x5C8] means the vtable entry is at Il2CppClass + 0x5C8!
        // Let's find what is at (ClassBase + 0x5C8):
        // If vtable is around 0x16C1D20:
        Console.WriteLine("Let's check pointers around 0x16C1D20 + 0x5C8 or similar");
    }
}

