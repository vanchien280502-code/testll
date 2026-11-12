using System;
using System.IO;

public class InspectClassStruct {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int baseOff = 0x16C2700;
        Console.WriteLine("Dumping 0x16C2700 - 0x16C2880:");
        for (int off = baseOff; off < 0x16C2880; off += 8) {
            long ptr = BitConverter.ToInt64(bytes, off);
            long rva = ptr - 0x180000000L;
            Console.WriteLine(string.Format("0x{0:X}: 0x{1:X16} (RVA 0x{2:X})", off, ptr, rva));
        }
    }
}

