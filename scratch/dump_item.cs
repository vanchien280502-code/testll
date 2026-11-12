using System;
using System.IO;
using System.Text;

public class DumpAssembly {
    public static void Main() {
        string path = @"TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        int rva = 0x4e6a10;
        Console.WriteLine("Bytes at 0x{0:X}:", rva);
        for (int i = 0; i < 96; i++) {
            Console.Write("{0:X2} ", bytes[rva + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();

        // Search for call/jump to 0x4e6a10 across the whole file
        // Relative call opcode: E8 xx xx xx xx -> target = curr_addr + 5 + offset
        Console.WriteLine("Callers of 0x4e6a10:");
        for (int i = 0; i < bytes.Length - 5; i++) {
            if (bytes[i] == 0xE8 || bytes[i] == 0xE9) {
                int rel = BitConverter.ToInt32(bytes, i + 1);
                long target = (long)i + 5 + rel;
                if (target == rva) {
                    Console.WriteLine("  Call from 0x{0:X} ({1})", i, bytes[i] == 0xE8 ? "CALL" : "JMP");
                }
            }
        }
    }
}
