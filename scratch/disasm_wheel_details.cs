using System;
using System.IO;

public class DisasmWheelDetails {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        // Look for string literal or constant loads in 0x500FF0 - 0x50141B
        int off1 = ToFileOffset(0x500FF0);
        int len1 = 0x50141B - 0x500FF0;
        Console.WriteLine("Constants/Calls in Function 1 (0x500FF0 - 0x50141B):");
        for (int i = 0; i < len1 - 7; i++) {
            // Check for mov edx, imm32 (BA xx xx xx xx)
            if (bytes[off1 + i] == 0xBA) {
                int val = BitConverter.ToInt32(bytes, off1 + i + 1);
                if (val > 0 && val < 50000) Console.WriteLine(string.Format("  RVA 0x{0:X}: mov edx, {1} (0x{1:X})", 0x500FF0 + i, val));
            }
            // Check for mov r8d, imm32 (41 B8 xx xx xx xx)
            if (bytes[off1 + i] == 0x41 && bytes[off1 + i + 1] == 0xB8) {
                int val = BitConverter.ToInt32(bytes, off1 + i + 2);
                if (val > 0 && val < 50000) Console.WriteLine(string.Format("  RVA 0x{0:X}: mov r8d, {1} (0x{1:X})", 0x500FF0 + i, val));
            }
        }

        Console.WriteLine("\nConstants/Calls in Function 2 (0x501420 - 0x501997):");
        int off2 = ToFileOffset(0x501420);
        int len2 = 0x501997 - 0x501420;
        for (int i = 0; i < len2 - 7; i++) {
            if (bytes[off2 + i] == 0xBA) {
                int val = BitConverter.ToInt32(bytes, off2 + i + 1);
                if (val > 0 && val < 50000) Console.WriteLine(string.Format("  RVA 0x{0:X}: mov edx, {1} (0x{1:X})", 0x501420 + i, val));
            }
            if (bytes[off2 + i] == 0x41 && bytes[off2 + i + 1] == 0xB8) {
                int val = BitConverter.ToInt32(bytes, off2 + i + 2);
                if (val > 0 && val < 50000) Console.WriteLine(string.Format("  RVA 0x{0:X}: mov r8d, {1} (0x{1:X})", 0x501420 + i, val));
            }
        }
    }
}

