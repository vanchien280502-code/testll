using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int baseOff = 0x16C0E18 - 0x488;
        Console.WriteLine("If [rax + 0x488] is 0x16C0E18, then rax = 0x" + baseOff.ToString("X"));
        for (int i = 0; i <= 0x490; i += 8) {
            long val = BitConverter.ToInt64(b, baseOff + i);
            long rva = val - 0x180000000L;
            if (i == 0x488) {
                Console.WriteLine(string.Format("Slot 0x{0:X}: 0x{1:X} (0x4D8BE0!)", i, rva));
            }
        }
    }
}

