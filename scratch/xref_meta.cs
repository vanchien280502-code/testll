using System;
using System.IO;

public class XrefMeta {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // 0x15A84F0 is file offset. In .rdata, RVA is offset - 0x235600 + 0x237000? Let's check sections.
        // Wait, PE section headers:
        int peHeader = BitConverter.ToInt32(bytes, 0x3C);
        int numSections = BitConverter.ToInt16(bytes, peHeader + 6);
        int optHeaderSize = BitConverter.ToInt16(bytes, peHeader + 20);
        int secStart = peHeader + 24 + optHeaderSize;

        int metaRva = 0;
        for (int i = 0; i < numSections; i++) {
            int entry = secStart + i * 40;
            int virtSize = BitConverter.ToInt32(bytes, entry + 8);
            int virtAddr = BitConverter.ToInt32(bytes, entry + 12);
            int rawSize = BitConverter.ToInt32(bytes, entry + 16);
            int rawAddr = BitConverter.ToInt32(bytes, entry + 20);
            if (0x15A84F0 >= rawAddr && 0x15A84F0 < rawAddr + rawSize) {
                metaRva = (0x15A84F0 - rawAddr) + virtAddr;
                Console.WriteLine("String is in section " + i + ", RVA: 0x" + metaRva.ToString("X"));
                break;
            }
        }

        // Search for RIP-relative references: 48 8D 0D / 15 / etc. [disp32]
        // disp32 = metaRva - (next_instr_rva)
        for (int i = 0; i < numSections; i++) {
            int entry = secStart + i * 40;
            int virtSize = BitConverter.ToInt32(bytes, entry + 8);
            int virtAddr = BitConverter.ToInt32(bytes, entry + 12);
            int rawSize = BitConverter.ToInt32(bytes, entry + 16);
            int rawAddr = BitConverter.ToInt32(bytes, entry + 20);

            for (int off = rawAddr; off < rawAddr + rawSize - 7; off++) {
                // Check lea rcx/rdx/r8, [rip+disp32] -> 48 8D (0D|15|05)
                if (bytes[off] == 0x48 && bytes[off+1] == 0x8D && (bytes[off+2] == 0x0D || bytes[off+2] == 0x15 || bytes[off+2] == 0x05)) {
                    int disp = BitConverter.ToInt32(bytes, off + 3);
                    int currentRva = (off - rawAddr) + virtAddr;
                    int targetRva = currentRva + 7 + disp;
                    if (targetRva == metaRva) {
                        Console.WriteLine("Found reference at RVA: 0x" + currentRva.ToString("X") + " (file: 0x" + off.ToString("X") + ")");
                    }
                }
            }
        }
    }
}

