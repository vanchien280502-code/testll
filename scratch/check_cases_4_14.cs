using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int[] cases = new int[] { 0x4D9101, 0x4D9177, 0x4D91ED, 0x4D9216, 0x4D928C, 0x4D9302, 0x4D9378, 0x4D93EE, 0x4D9464, 0x4D94DA, 0x4D9519 };
        for (int i = 0; i < cases.Length; i++) {
            int rva = cases[i];
            int fileOff = (rva - 0x237000) + 0x235600;
            Console.WriteLine(string.Format("Case {0,2} (ID {1}) at RVA 0x{2:X}: {3}", i + 4, 2985 + i + 4, rva, BitConverter.ToString(b, fileOff, 20).Replace('-', ' ')));
        }
    }
}

