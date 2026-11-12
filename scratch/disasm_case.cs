using System;
using System.IO;

public class DisasmCase {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x4D8F69;
        int off = ToFileOffset(rva);
        Console.WriteLine("Case 0 (0x4D8F69):");
        for (int i = 0; i < 0x4D8FCF - 0x4D8F69; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
        }
        Console.WriteLine("\n");
        // Print calls
        for (int i = 0; i < 0x4D8FCF - 0x4D8F69; i++) {
            if (bytes[off + i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                int targetRva = rva + i + 5 + rel;
                Console.WriteLine("Call at RVA 0x{0:X} -> target RVA 0x{1:X}", rva + i, targetRva);
            }
        }
    }
}

