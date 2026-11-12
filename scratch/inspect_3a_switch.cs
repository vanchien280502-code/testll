using System;
using System.IO;

public class Inspect3ASwitch {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int startRva = 0x3A0170;
        int endRva = 0x3A3A28;
        int off = ToFileOffset(startRva);
        int len = endRva - startRva;

        Console.WriteLine("Scanning 0x3A0170 - 0x3A3A28 for switch / comparisons with edx or rdx...");
        // Usually cmp edx, XX or sub edx, XX or switch table
        for (int i = 0; i < len - 10; i++) {
            int cur = off + i;
            // 83 FA xx (cmp edx, imm8) or 81 FA xx xx xx xx (cmp edx, imm32)
            if (bytes[cur] == 0x83 && bytes[cur+1] == 0xFA) {
                int val = bytes[cur+2];
                Console.WriteLine(string.Format("RVA 0x{0:X}: cmp edx, {1} (0x{1:X2})", startRva + i, val));
            } else if (bytes[cur] == 0x81 && bytes[cur+1] == 0xFA) {
                int val = BitConverter.ToInt32(bytes, cur+2);
                Console.WriteLine(string.Format("RVA 0x{0:X}: cmp edx, {1} (0x{1:X})", startRva + i, val));
            }
        }
    }
}

