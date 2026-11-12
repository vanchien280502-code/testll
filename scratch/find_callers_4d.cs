using System;
using System.IO;

public class FindCallers4D {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToRva(int off) {
        if (off >= 0x400 && off < 0x235600) return (off - 0x400) + 0x1000;
        if (off >= 0x235600 && off < 0x129C000) return (off - 0x235600) + 0x237000;
        return off;
    }

    public static void Main() {
        int targetRva = 0x4D8BE0;
        // Search in il2cpp section
        for (int off = 0x235600; off < 0x129C000 - 5; off++) {
            if (bytes[off] == 0xE8) {
                int disp = BitConverter.ToInt32(bytes, off + 1);
                int rva = ToRva(off);
                if (rva + 5 + disp == targetRva) {
                    Console.WriteLine("Found call at RVA 0x" + rva.ToString("X"));
                }
            }
        }
        // Also search for pointer (vtable)
        long targetVa = 0x180000000L + targetRva;
        byte[] p = BitConverter.GetBytes(targetVa);
        for (int off = 0x129C000; off < bytes.Length - 8; off += 8) {
            bool found = true;
            for (int j = 0; j < 8; j++) if (bytes[off + j] != p[j]) { found = false; break; }
            if (found) {
                Console.WriteLine("Found pointer at raw offset 0x" + off.ToString("X"));
            }
        }
    }
}

