using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16EE888;
        // Check what section 0x16EE888 is in (.data or .bss)
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
                int fileOffset = (rva - virtAddr) + rawAddr;
                long initialVal = BitConverter.ToInt64(bytes, fileOffset);
                Console.WriteLine("0x16EE888 is in section {0}, file offset 0x{1:X}, initial value = 0x{2:X}", name, fileOffset, initialVal);
                break;
            }
        }
    }
}

