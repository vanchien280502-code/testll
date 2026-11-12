using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int rva = 0x4D91ED;
        int off = ToFileOffset(rva);
        // Print 40 bytes:
        // 48-8B-0D-8C-43-21-01 -> mov rcx, [rip + 0x121438c] (rva + 7 + 0x121438c = 0x16ED580)
        // 83-B9-E0-00-00-00-00 -> cmp dword ptr [rcx+E0h], 0
        // 75-05                -> jne +5
        // E8-BE-15-CD-FF       -> call 0x1AA7C0
        // 33-C9                -> xor ecx, ecx
        // E8-37-05-D9-FF       -> call 0x269740
        // 33-C9                -> xor ecx, ecx
        // E8-C0-0E-D9-FF       -> call 0x26A0D0
        // 90                   -> nop
        // E9-77-0C-00-00       -> jmp 0x4D9EA0

        Console.WriteLine("Case 6 starts at 0x{0:X}", rva);
        for (int i = 0; i < 40; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
        }
        Console.WriteLine();
    }
}

