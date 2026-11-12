using System;
using System.IO;
using System.Collections.Generic;

public class AnalyzeOpcodes {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        string[] lines = File.ReadAllLines(@"scratch\all_opcodes.txt");
        foreach (string line in lines) {
            // RVA 0x258F5F -> Opcode: 0xA1 (161)
            var parts = line.Split(' ');
            if (parts.Length < 5) continue;
            int rva = Convert.ToInt32(parts[1], 16);
            int opcode = Convert.ToInt32(parts[4], 16);
            int off = ToFileOffset(rva);

            // Scan next 150 bytes for vtable calls
            // vtable offsets on Message:
            // 0x218 / 0x220: writeByte
            // 0x228 / 0x230: writeInt / writeBoolean?
            // 0x248 / 0x250: writeShort
            // 0x268 / 0x270: writeUTF
            // 0x298 / 0x2A0: flush / send
            var writes = new List<string>();
            for (int i = 0; i < 150; i++) {
                int cur = off + i;
                if (bytes[cur] == 0xFF && (bytes[cur+1] == 0x90 || bytes[cur+1] == 0x91 || bytes[cur+1] == 0x92)) {
                    int disp = BitConverter.ToInt32(bytes, cur + 2);
                    if (disp == 0x218 || disp == 0x220) writes.Add("writeByte");
                    else if (disp == 0x228 || disp == 0x230) writes.Add("writeInt");
                    else if (disp == 0x248 || disp == 0x250) writes.Add("writeShort");
                    else if (disp == 0x268 || disp == 0x270) writes.Add("writeUTF");
                    else if (disp == 0x298 || disp == 0x2A0) { writes.Add("flush"); break; }
                }
            }
            Console.WriteLine(string.Format("RVA 0x{0:X} | Opcode {1,3} (0x{1:X2}) | Flow: {2}", rva, opcode, string.Join(" -> ", writes)));
        }
    }
}

