using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x105430;
        int fileOffset = (rva - 0x1000) + 0x400; // text section
        // Find section
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
                break;
            }
        }
        Console.WriteLine("0x105430:");
        for (int i = 0; i < 48; i++) {
            Console.Write("{0:X2} ", bytes[fileOffset + i]);
        }
        Console.WriteLine();
    }
}

