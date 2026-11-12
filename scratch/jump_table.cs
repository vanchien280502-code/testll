using System;
using System.IO;

public class JumpTable {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x3A3A04;
        int fileOffset = (rva - 0x237000) + 0x235600;

        Console.WriteLine("Jump Table at RVA 0x3A3A04 (offset 0x{0:X}):", fileOffset);
        for (int i = 0; i <= 8; i++) {
            int targetRel = BitConverter.ToInt32(bytes, fileOffset + i * 4);
            // In x64: 48 03 CA FF E1 -> target = rva + targetRel or base + targetRel?
            // "48 98" (movsxd rax, eax), "8B 8C 82 04 3A 3A 00" (mov ecx, [table + rax*4]), "48 03 CA" (add rcx, rdx) or "48 03 CA" (add rcx, rdx)
            // Let's check the instruction right before:
            // "48 98 8B 8C 82 04 3A 3A 00 48 03 CA FF E1"
            Console.WriteLine("  Case {0}: 0x{1:X8} (Target RVA: 0x{2:X})", i, targetRel, (rva + targetRel));
        }
    }
}

