using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x357AB0;
        int off = (rva - 0x237000) + 0x235600;
        // Let's print each instruction with its address
        // 0x357AB8: E8 21 06 00 00 -> call 0x3580DE
        // 0x357ABD: EB 26 -> jmp 0x357AE5
        // 0x357ABF: 33 D2 -> xor edx, edx
        // 0x357AC1: B1 0D -> mov cl, 0x0D
        // 0x357AC3: E8 D6 AE F8 FF -> call 0x2E29A0
        // 0x357AC8: 48 85 C0 -> test rax, rax
        // 0x357ACB: 0F 84 9D 01 00 00 -> jz 0x357C6E
        // 0x357AD1: 4C 8B 00 -> mov r8, [rax]
        // 0x357AD4: 48 8B C8 -> mov rcx, rax
        // 0x357AD7: 49 8B 90 A0 02 00 00 -> mov rdx, [r8 + 0x2a0]
        // 0x357ADE: 41 FF 90 98 02 00 00 -> call [r8 + 0x298]
        // 0x357AE5: ??
        Console.WriteLine("At 0x357AE5: {0:X2} {1:X2}", bytes[off + (0x357AE5 - 0x357AB0)], bytes[off + (0x357AE6 - 0x357AB0)]);
    }
}

