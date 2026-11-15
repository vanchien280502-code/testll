using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x1DD900;
        int fileOffset = (rva - 0x1000) + 0x400; // wait, what section is 0x1DD900 in?
        // Let's find section
        int peOffset = BitConverter.ToInt32(bytes, 0x3C);
        int numSections = BitConverter.ToInt16(bytes, peOffset + 6);
        int optHeaderOffset = peOffset + 24;
        int optHeaderSize = BitConverter.ToInt16(bytes, peOffset + 20);
        int sectionTableOffset = optHeaderOffset + optHeaderSize;
        for (int i = 0; i < numSections; i++) {
            int secOffset = sectionTableOffset + i * 40;
            string name = System.Text.Encoding.ASCII.GetString(bytes, secOffset, 8).TrimEnd('\0');
            int virtSize = BitConverter.ToInt32(bytes, secOffset + 8);
            int virtAddr = BitConverter.ToInt32(bytes, secOffset + 12);
            int rawAddr = BitConverter.ToInt32(bytes, secOffset + 20);
            if (rva >= virtAddr && rva < virtAddr + virtSize) {
                fileOffset = (rva - virtAddr) + rawAddr;
                Console.WriteLine("Section {0}: RVA 0x{1:X} -> FileOffset 0x{2:X}", name, rva, fileOffset);
                for (int j = 0; j < 64; j += 16) {
                    Console.WriteLine("{0:X2}: {1}", j, BitConverter.ToString(bytes, fileOffset + j, 16).Replace('-', ' '));
                }
                break;
            }
        }
    }
}

