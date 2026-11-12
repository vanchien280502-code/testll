using System;
using System.IO;

public class DumpControllerVtable {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    public static void Main() {
        long imageBase = 0x180000000;
        int start = 0x16C0D00;
        int count = 120;
        Console.WriteLine("Controller VTable at 0x{0:X}:", start);
        for (int i = 0; i < count; i++) {
            int off = start + i * 8;
            long val = BitConverter.ToInt64(bytes, off);
            long rva = val - imageBase;
            if (val >= imageBase && rva < 0x2000000) {
                Console.WriteLine(string.Format("Slot {0,3} (0x{1:X}): RVA 0x{2:X}", i, off, rva));
            }
        }
    }
}

