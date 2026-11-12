using System;
using System.IO;

public class ScanMessages {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        Console.WriteLine("Scanning all calls to Message ctor around Item opcodes (0x70 to 0x85)...");
        for (int i = 0; i < bytes.Length - 10; i++) {
            if (bytes[i] == 0xB2 && bytes[i + 1] >= 0x70 && bytes[i + 1] <= 0x85) {
                byte op = bytes[i + 1];
                int rva = i - 0x235600 + 0x237000;
                for (int j = 2; j < 10; j++) {
                    if (bytes[i + j] == 0xE8) {
                        int rel = BitConverter.ToInt32(bytes, i + j + 1);
                        long target = (long)rva + j + 5 + rel;
                        if (target >= 0x2E2800 && target <= 0x2E2D00) {
                            Console.WriteLine("Opcode 0x{0:X2} ({0}) called at RVA 0x{1:X}", op, rva);
                        }
                    }
                }
            }
        }
    }
}
