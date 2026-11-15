using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In GameAssembly.dll, let's find references to 0x180000000 + 0x4E4093 or where fcClass vtable is:
        // fcClass token is 0x20006141 at 0x16ee888
        // Let's find pointers in .data that have method implementations at 0x4D8
        // Notice in find_4d8_calls.cs: call [rax + 0x4D8]
        // rax is the vtable of rcx!
        Console.WriteLine("Searching for vtables with offset 0x4D8...");
    }
}
