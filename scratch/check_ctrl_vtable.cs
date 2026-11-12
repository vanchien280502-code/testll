using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetFileOff = 0x16C0E18; // offset of 0x4D8BE0 in Controller VTable
        Console.WriteLine("Controller VTable at 0x16C0E18 has RVA 0x4D8BE0");
        // What is at 0x16C0E18 - ? or + ?
        // Slot 0x488 from what base?
        // Let's check where the controller vtable begins:
        for (int i = 0x16C0000; i < 0x16C2000; i += 8) {
            long val = BitConverter.ToInt64(b, i);
            long rva = val - 0x180000000L;
            if (rva == 0x4D8BE0) {
                Console.WriteLine("0x4D8BE0 is at file 0x" + i.ToString("X"));
            }
        }
    }
}

