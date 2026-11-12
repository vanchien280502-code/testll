using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static void Main() {
        // Earlier we saw:
        // [0x16C0E18 + 000] = 0x1804D8BE0 (RVA 0x4D8BE0)
        // [0x16C12A8 + 000] = 0x180501420 (RVA 0x501420)
        // The difference is 0x16C12A8 - 0x16C0E18 = 0x490 bytes.
        // What is at 0x16C0E18 + (0x528 - ???)?
        // Where is slot 0?
        // In il2cpp, vtable is at [class + 0x140] or [class + 0x138].
        // When calling [r10 + 0x528], r10 is [rsi] which is the Il2CppClass*!
        // So offset 0x528 from Il2CppClass* is [vtable + (0x528 - 0x140?)]!
        Console.WriteLine("Let's check around 0x16C0E18:");
        // If [r10 + 0x490?] is 0x4D8BE0 or what?
    }
}

