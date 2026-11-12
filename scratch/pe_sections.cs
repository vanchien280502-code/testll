using System;
using System.IO;

public class PeSections {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int peOffset = BitConverter.ToInt32(bytes, 0x3C);
        short numSections = BitConverter.ToInt16(bytes, peOffset + 6);
        short optHeaderSize = BitConverter.ToInt16(bytes, peOffset + 20);
        int sectionHeadersOffset = peOffset + 24 + optHeaderSize;

        for (int i = 0; i < numSections; i++) {
            int off = sectionHeadersOffset + i * 40;
            string name = System.Text.Encoding.ASCII.GetString(bytes, off, 8).Trim('\0');
            int virtSize = BitConverter.ToInt32(bytes, off + 8);
            int virtAddr = BitConverter.ToInt32(bytes, off + 12);
            int rawSize = BitConverter.ToInt32(bytes, off + 16);
            int rawAddr = BitConverter.ToInt32(bytes, off + 20);
            Console.WriteLine(string.Format("Section {0,-8} VirtAddr: 0x{1:X8} VirtSize: 0x{2:X8} RawAddr: 0x{3:X8} RawSize: 0x{4:X8}",
                name, virtAddr, virtSize, rawAddr, rawSize));
        }
    }
}

