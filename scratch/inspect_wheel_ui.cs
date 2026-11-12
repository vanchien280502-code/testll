using System;
using System.IO;

public class InspectWheelUI {
    static byte[] dll = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        // Let's inspect references to 0x501350 and 0x5018A0
        long imageBase = 0x180000000;
        long target1 = imageBase + 0x501350;
        long target2 = imageBase + 0x5018A0;

        Console.WriteLine("Searching for references in dll...");
        for (int i = 0; i < dll.Length - 8; i += 8) {
            long p = BitConverter.ToInt64(dll, i);
            if (p == target1 || p == target2) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Ref to 0x{0:X} at DLL offset 0x{1:X} (RVA 0x{2:X})", p - imageBase, i, rva);
            }
        }
    }
}

