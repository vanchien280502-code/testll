using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Let's inspect Session class (at game + 0x16EE888)
        // In GameAssembly.dll, let's look at 0x4340:
        // 0x4340 is interface dispatch:
        // 48 8B 02: mov rax, [rdx] (vtable of object)
        // 4D 8B D0: mov r10, r8 (instance)
        // ...
        // In il2cpp, what concrete class implements Session?
        // Let's search for calls to Message.send or search for Session in strings!
        Console.WriteLine("Done");
    }
}

