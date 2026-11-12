using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4D8FCF;
        int off = ToFileOffset(rva);
        // Let's decode bytes accurately
        // 0000: 33 D2 (xor edx, edx)
        // 0002: B1 9A (mov cl, 0x9a)
        // 0004: E8 C8 98 E0 FF (call 0x2E28A0)
        // 0009: 48 8B D8 (mov rbx, rax)
        // 000C: 4C 8B 05 A6 58 21 01 (mov r8, [rip + 0x12158A6]) -> 0x4D8FCF + 0x13 + 0x12158A6 = 0x16EE888
        // 0013: 41 83 B8 E0 00 00 00 00 (cmp dword ptr [r8+0xE0], 0)
        // 001B: 75 0F (jne +0x0F) -> 0x002C
        // 001D: 49 8B C8 (mov rcx, r8)
        // 0020: E8 CC 17 CD FF (call 0x1DD900)
        // 0025: 4C 8B 05 8D 58 21 01 (mov r8, [rip + 0x121588D]) -> 0x4D8FCF + 0x2C + 0x121588D = 0x16EE888
        // 002C: 48 85 DB (test rbx, rbx)
        // 002F: 0F 84 1F 1B 00 00 (jz +0x1B1F)
        // 0035: 4D 8B 80 B8 00 00 00 (mov r8, [r8+0xB8])
        // 003C: B9 14 00 00 00 (mov ecx, 0x14)
        // 0041: 4D 8B 40 18 (mov r8, [r8+0x18])
        // 0045: 48 8B D3 (mov rdx, rbx)
        // 0048: E8 24 B3 B2 FF (call 0x4340)
        // 004D: 48 8B 03 (mov rax, [rbx])
        // 0050: 48 8B 90 A0 02 00 00 (mov rdx, [rax+0x2A0])
        // 0057: 48 8B CB (mov rcx, rbx)
        // 005A: FF 90 98 02 00 00 (call [rax+0x298])
        Console.WriteLine("Target of 0x4D8FCF + 0x48: 0x{0:X}", 0x4D8FCF + 0x4D + BitConverter.ToInt32(bytes, off + 0x49));
    }
}

