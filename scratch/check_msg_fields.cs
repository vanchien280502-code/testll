using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x2E28A0;
        int off = ToFileOffset(rva);
        // In check_2e28a0_full.cs:
        // 0050: 8B D7 (mov edx, edi) -> sub-opcode!
        // 0052: 48 8B CB (mov rcx, rbx) -> message!
        // 0055: 4D 8B 81 30 02 00 00 (mov r8, [r9 + 0x230])
        // 005C: 41 FF 91 28 02 00 00 (call [r9 + 0x228]) -> WriteByte(sub-opcode)
        Console.WriteLine("0x2E28A0 writes byte at +0x50 to the message!");
    }
}

