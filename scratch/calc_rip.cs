using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4D8F70;
        int fileOffset = (rva - 0x237000) + 0x235600;
        for (int i = 0; i < 40; i++) {
            if (bytes[fileOffset + i] == 0x4C && bytes[fileOffset + i + 1] == 0x8B && bytes[fileOffset + i + 2] == 0x05) {
                int disp = BitConverter.ToInt32(bytes, fileOffset + i + 3);
                int instrRva = rva + i;
                long target = (long)instrRva + 7 + disp;
                Console.WriteLine(string.Format("Instr at RVA 0x{0:X}: target = 0x{1:X}", instrRva, target));
            }
        }
    }
}
