using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        int targetRva = 0x4D8F69;
        // Search in the function 0x4D8BE0 - 0x4DAC44
        for (int rva = 0x4D8BE0; rva < 0x4DAC44; rva++) {
            int off = ToFileOffset(rva);
            // check jmp/jcc
            if (bytes[off] == 0xE9 || bytes[off] == 0x0F && (bytes[off+1] & 0xF0) == 0x80) {
                int dispOff = bytes[off] == 0xE9 ? 1 : 2;
                int disp = BitConverter.ToInt32(bytes, off + dispOff);
                int dest = rva + dispOff + 4 + disp;
                if (dest == targetRva) {
                    Console.WriteLine("Direct jump from 0x{0:X}", rva);
                }
            }
            // check 32-bit offset relative or absolute
            int val = BitConverter.ToInt32(bytes, off);
            if (val == targetRva || val == (targetRva - 0x4D8BE0)) {
                Console.WriteLine("Table entry at 0x{0:X}: {1:X}", rva, val);
            }
        }
    }
}

