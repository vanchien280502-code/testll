using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In IL2CPP, Il2CppClass has vtable at offset 0x138 or 0x140?
        // Let's find pointers to 0x1816C12A0 or around 0x1816C1000
        long imageBase = 0x180000000;
        long vtable = imageBase + 0x16C12A0;
        Console.WriteLine("Searching for references to vtable 0x{0:X}...", vtable);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(bytes, i);
            if (p == vtable) {
                Console.WriteLine("Vtable ref at FO 0x{0:X} (RVA 0x{1:X})", i, i + 0x1A00);
            }
        }
    }
}

