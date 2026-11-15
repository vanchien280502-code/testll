using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In 0x4E40B1:
        // mov rax, [rbx]
        // call [rax + 0x4D8]
        // rax is the vtable pointer (first 8 bytes of rbx, or Il2CppClass)
        // Since offset is +0x4D8:
        // Let's find what function 0x4D8 is:
        // In find_4d8_calls.cs, we saw many callers calling call [rax + 0x4D8]!
        // For example:
        // call [rax + 0x4D8] at RVA 0x4E40B1
        // Let's disassemble from 0x4E40B1 - 10 to 0x4E40B1 + 20:
        int rva = 0x4E40B1;
        int fo = rva - 0x1A00;
        Console.WriteLine("Bytes around 0x4E40B1:");
        Console.WriteLine(BitConverter.ToString(bytes, fo - 16, 32));
    }
}
