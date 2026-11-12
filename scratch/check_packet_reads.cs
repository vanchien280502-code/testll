using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Read calls in 0x474C00 - 0x476800
        for (int rva = 0x474C00; rva < 0x476800; rva++) {
            int off = ToFileOffset(rva);
            if (bytes[off] == 0xE8) {
                int rel = BitConverter.ToInt32(bytes, off + 1);
                int target = rva + 5 + rel;
                // check target
                if (target == 0x2E28A0 || target == 0x2E2AA0) {
                    Console.WriteLine("CALL 0x{0:X} from 0x{1:X}", target, rva);
                }
            }
        }
    }
}

