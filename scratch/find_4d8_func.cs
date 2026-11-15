using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // FO of 0x16C0F10 is 0x16C0F10.
        // Let's find the start of the vtable containing 0x16C0F10!
        // In check_vtable_4e.cs:
        // 0x16C0F10 contains 0x1804E1E90.
        // Where is 0x4D8 relative to the class?
        // In IL2CPP, an object's pointer [obj] is Il2CppClass*.
        // Il2CppClass has vtable at offset 0x128 or 0x138 (or 0x140).
        // If rax is [rbx], then [rax + 0x4D8] is at offset 0x4D8 from the start of Il2CppClass!
        // Let's search backward from 0x16C0F10 to find the Il2CppClass base:
        for (int d = 0; d < 0x600; d += 8) {
            int candidate = 0x16C0F10 - d;
            long func = BitConverter.ToInt64(bytes, candidate + 0x4D8);
            long funcRva = func - 0x180000000L;
            if (funcRva > 0 && funcRva < 0x600000) {
                Console.WriteLine("Class candidate at FO 0x{0:X} (-0x{1:X}): func at +0x4D8 = RVA 0x{2:X}", candidate, d, funcRva);
            }
        }
    }
}
