using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Let's check the distance between 0x16C1D30 and other entries:
        // In 0x497440:
        // mov rax, [rbx]
        // call [rax + 0x5C8]
        // If rax is [rbx], rax is the Il2CppClass pointer of the dialog instance!
        // So at offset 0x5C8 from Il2CppClass is a function pointer!
        // If 0x16C1D30 is in the vtable of that same Il2CppClass,
        // what is the Il2CppClass address?
        // Let's check offsets in .data:
        // FO of 0x16C1D30 is 0x16C0330.
        // If 0x5C8 is in the vtable, let's look at what class has 0x16C1D30:
        for (int classFo = 0x16C0330 - 0x1000; classFo < 0x16C0330; classFo += 8) {
            long p5c8 = BitConverter.ToInt64(bytes, classFo + 0x5C8);
            long rva5c8 = p5c8 - 0x180000000L;
            if (rva5c8 > 0 && rva5c8 < 0x600000) {
                // Check if this class has 0x374AD0 at some offset
                for (int v = 0x100; v < 0x800; v += 8) {
                    long pv = BitConverter.ToInt64(bytes, classFo + v);
                    if (pv - 0x180000000L == 0x374AD0) {
                        Console.WriteLine("Found Class at FO 0x{0:X}: 0x374AD0 at +0x{1:X}, 0x5C8 func = RVA 0x{2:X}", classFo, v, rva5c8);
                    }
                }
            }
        }
    }
}

