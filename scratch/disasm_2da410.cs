using System;
using System.IO;

public class Disasm2DA410 {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int rva = 0x2DA410 - 0x80;
        int off = ToFileOffset(rva);
        Console.WriteLine("Around 0x2DA410:");
        for (int i = 0; i < 250; i++) {
            Console.Write("{0:X2} ", bytes[off + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine("\n");
        // Print calls and mov ecx / edx
        for (int i = 0; i < 250; i++) {
            int cur = rva + i;
            if (bytes[off + i] == 0xB9) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov ecx, {1} (0x{1:X})", cur, val);
            }
            if (bytes[off + i] == 0xBA) {
                int val = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: mov edx, {1} (0x{1:X})", cur, val);
            }
            if (bytes[off + i] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + i + 1);
                Console.WriteLine("  0x{0:X}: Call -> 0x{1:X}", cur, cur + 5 + rel);
            }
        }
    }
}

