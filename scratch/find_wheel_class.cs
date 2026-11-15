using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In IL2CPP 64-bit, vtable is inside Il2CppClass at offset 0x128 or 0x138
        // 0x16C1D30 is in .data
        // Let's find Il2CppClass that contains 0x16C1D30
        int vtableRva = 0x16C1D30;
        // In GameAssembly.dll, let's find the start of the Il2CppClass struct:
        // An Il2CppClass has ClassName pointer at +0x10.
        // Let's search backward from vtableRva:
        for (int off = 0x100; off <= 0x300; off += 8) {
            int candidateRva = vtableRva - off;
            int fo = candidateRva - 0x1A00; // if in .data, wait, .data FO offset!
            // In check_class.ps1 earlier:
            // RVA 0x16C12A0 in section .data: FO 0x16BE6A0
            // 0x16C12A0 - 0x16BE6A0 = 0x2C00!
            // So for .data: FO = RVA - 0x2C00!
        }
    }
}

