using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x504BF6; // Wait! In find_action_ids: "Found 2986 at file 0x504BF6" -> what is the RVA?
        // file 0x504BF6: let's convert file offset to RVA
        // .text starts at raw 0x400 (virt 0x1000) or let's use the exact section formula
        int fileOff = 0x504BF6;
        int rvaExact = (fileOff - 0x235600) + 0x237000;
        Console.WriteLine(string.Format("At file 0x{0:X} (RVA 0x{1:X}):", fileOff, rvaExact));
        for (int i = -32; i < 64; i += 16) {
            Console.WriteLine(string.Format("file 0x{0:X}: {1}", fileOff + i, BitConverter.ToString(b, fileOff + i, 16).Replace('-', ' ')));
        }
    }
}

