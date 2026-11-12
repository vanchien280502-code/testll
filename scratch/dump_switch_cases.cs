using System;
using System.IO;

public class DumpSwitchCases {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int tableRva = 0x3A38E0;
        int fileOffset = ToFileOffset(tableRva);
        Console.WriteLine("Jump table at RVA 0x3A38E0 (file 0x" + fileOffset.ToString("X") + "):");
        for (int i = 0; i <= 0x48; i++) {
            int caseRva = BitConverter.ToInt32(bytes, fileOffset + i * 4);
            Console.WriteLine(string.Format("Case {0,2} (0x{0:X2}): RVA 0x{1:X}", i, caseRva));
        }
    }
}

