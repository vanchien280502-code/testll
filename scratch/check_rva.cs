using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4340;
        int off = (rva - 0x1000) + 0x400; // .text section offset usually (rva - 0x1000 + 0x400) or check PE
        // Let's find PE sections
        int pe = BitConverter.ToInt32(b, 0x3C);
        int numSections = BitConverter.ToInt16(b, pe + 6);
        int optHeader = pe + 24;
        int optHeaderSize = BitConverter.ToInt16(b, pe + 20);
        int secTable = optHeader + optHeaderSize;
        
        for (int i = 0; i < numSections; i++) {
            int sec = secTable + i * 40;
            string name = System.Text.Encoding.ASCII.GetString(b, sec, 8).TrimEnd('\0');
            int virtSize = BitConverter.ToInt32(b, sec + 8);
            int virtAddr = BitConverter.ToInt32(b, sec + 12);
            int rawSize = BitConverter.ToInt32(b, sec + 16);
            int rawAddr = BitConverter.ToInt32(b, sec + 20);
            if (rva >= virtAddr && rva < virtAddr + virtSize) {
                int fileOff = (rva - virtAddr) + rawAddr;
                Console.WriteLine("RVA 0x" + rva.ToString("X") + " in section " + name + " -> file 0x" + fileOff.ToString("X"));
                Console.WriteLine(BitConverter.ToString(b, fileOff, 32).Replace('-', ' '));
                return;
            }
        }
        Console.WriteLine("Section not found for 0x" + rva.ToString("X"));
    }
}

