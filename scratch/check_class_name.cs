using System;
using System.IO;

class CheckClassName {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int off = (0x16ED588 - 0x237000) + 0x235600;
        long classPtr = BitConverter.ToInt64(bytes, off);
        Console.WriteLine("Class pointer at 0x16ED588: 0x{0:X}", classPtr);
        // Let's check other slots:
        int[] slots = new int[] { 0x16ED588, 0x16ED748, 0x16EE178, 0x16F6A98, 0x16F0198 };
        foreach (int s in slots) {
            int o = (s - 0x237000) + 0x235600;
            long ptr = BitConverter.ToInt64(bytes, o);
            Console.WriteLine("Slot 0x{0:X}: 0x{1:X}", s, ptr);
        }
    }
}

