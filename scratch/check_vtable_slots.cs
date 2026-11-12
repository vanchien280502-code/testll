using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int startFileOff = 0x16C1220 - 0x80;
        // Let's find the start of the vtable of this class
        // Look for 0x16C1... entries
        Console.WriteLine("Slots from 0x500 to 0x560:");
        // In il2cpp, vtable is at [class + 0x128] or instance vtable starts at offset 0
        // Let's dump around 0x16C12A8 + 0x528 - 8
        int slotOff = 0x16C12A8 + 0x528 - 8; // if 0x16C12A8 is slot 1
        // Let's see what is at 0x16C12A8 + i * 8
        for (int i = 0x500; i <= 0x550; i += 8) {
            // let's check
        }
    }
}

