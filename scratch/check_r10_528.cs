using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // In check_5018.cs, at 0x5018C7:
        // mov r10, [rsi]
        // call qword ptr [r10 + 0x528]
        // What class is rsi?
        // rsi is MainController (fcClass = 0x16EE888).
        // Let's find the vtable of fcClass.
        // In check_vtable.cs earlier:
        // Pointer at 0x16C0E18 was 0x1804D8BE0 (0x4D8BE0).
        // Let's find the base of fcClass's vtable.
        // In il2cpp, what offset in Il2CppClass is the vtable?
        // Let's look at 0x16C0E18 - 0x528 or similar.
        // If [r10 + 0x528] was called, r10 is Il2CppClass*!
        // In Il2CppClass, offset 0x528 is inside the class or vtable!
        // Let's check what is at file offset of (class + 0x528)!
        // What is the address of fcClass in GameAssembly.dll?
        // At 0x16EE888 in .data is a pointer to fcClass in BSS, initialized at runtime!
        Console.WriteLine("Done");
    }
}

