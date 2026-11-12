using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // find pointers to 0x16C3A18 or start of vtable
        // vtable start is somewhere around 0x16C3000
        for (int i = 0; i <= b.Length - 8; i += 8) {
            long val = BitConverter.ToInt64(b, i);
            long rva = val - 0x180000000L;
            if (rva >= 0x16C3000 && rva <= 0x16C4000) {
                // vtable pointer in an Il2CppClass or instance
                Console.WriteLine(string.Format("Pointer to 0x{0:X} at file 0x{1:X}", rva, i));
            }
        }
    }
}

