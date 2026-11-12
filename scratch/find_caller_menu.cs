using System;
using System.IO;

public class FindCallerMenu {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x3A3099;
        // Search callers of 0x3A3099
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8 || bytes[i] == 0xE9) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                int rvaSource = i - 0x235600 + 0x237000;
                long target = (long)rvaSource + 5 + rel;
                if (target == rva) {
                    Console.WriteLine("Caller of 0x{0:X} from RVA 0x{1:X}", rva, rvaSource);
                }
            }
        }
    }
}

