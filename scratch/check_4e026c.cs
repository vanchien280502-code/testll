using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16EE888;
        // In il2cpp, let's find references to 0x16EE888 where static fields are accessed
        // We saw earlier:
        // RVA 0x4BC26B: static_fields offset 0x10 (16)
        // RVA 0x4BC280: static_fields offset 0x10 (16)
        // RVA 0x4E026C: static_fields offset 0x18 (24) -> Wait! What is at 0x4E026C?
        Console.WriteLine("Inspecting RVA 0x4E026C...");
        int fileOffset = (0x4E026C - 0x237000) + 0x235600;
        byte[] slice = new byte[80];
        Array.Copy(bytes, fileOffset - 20, slice, 0, 80);
        Console.WriteLine(BitConverter.ToString(slice).Replace('-', ' '));
    }
}

