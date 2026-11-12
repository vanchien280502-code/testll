using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Vtable slot index is 263.
        // Let's find all occurrences in .text where [reg + disp] is called or loaded,
        // where disp is around 263 * 8 = 2104 (0x838) or similar.
        // Let's scan for any ModR/M with disp32 where disp32 == 0x16C0E18 - vtableBase!
        // In check_class_501.cs earlier:
        // [0x16C12A8 + 00] = 0x501420
        // [0x16C0E18 + 00] = 0x4D8BE0
        // Let's find any call in .text: FF 10..FF 17 (call [reg]) or FF 90..FF 97 (call [reg + disp32])
        Console.WriteLine("Searching for indirect calls with disp32 near vtable slot...");
        int count = 0;
        int textStart = ToFileOffset(0x1000);
        int textEnd = ToFileOffset(0x237000); // end of code
        for (int i = 0; i < textEnd - 6; i++) {
            // FF 90 xx xx xx xx to FF 97 xx xx xx xx
            byte b1 = bytes[i];
            byte b2 = bytes[i+1];
            if (b1 == 0xFF && (b2 >= 0x90 && b2 <= 0x97)) {
                int disp = BitConverter.ToInt32(bytes, i + 2);
                // Check if disp could be the slot for 0x4D8BE0
                // We know slot index is 263, but what is vtable offset in class?
                // Let's check disps in range 0x800 to 0x1200
                if (disp >= 0x800 && disp <= 0x1500) {
                    // Let's print if it matches
                    // Console.WriteLine("  Call at file 0x{0:X} with disp 0x{1:X}", i, disp);
                    count++;
                }
            }
        }
        Console.WriteLine("Total indirect calls with disp in [0x800, 0x1500]: {0}", count);
    }
}

