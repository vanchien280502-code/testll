using System;
using System.IO;
using System.Collections.Generic;

public class Program {
    struct Sec { public string name; public int virtAddr, virtSize, rawAddr, rawSize; }
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int pe = BitConverter.ToInt32(b, 0x3C);
        int numSections = BitConverter.ToInt16(b, pe + 6);
        int optHeader = pe + 24;
        int optHeaderSize = BitConverter.ToInt16(b, pe + 20);
        int secTable = optHeader + optHeaderSize;
        List<Sec> secs = new List<Sec>();
        for (int i = 0; i < numSections; i++) {
            int s = secTable + i * 40;
            Sec sec = new Sec {
                name = System.Text.Encoding.ASCII.GetString(b, s, 8).TrimEnd('\0'),
                virtSize = BitConverter.ToInt32(b, s + 8),
                virtAddr = BitConverter.ToInt32(b, s + 12),
                rawSize = BitConverter.ToInt32(b, s + 16),
                rawAddr = BitConverter.ToInt32(b, s + 20)
            };
            secs.Add(sec);
        }

        long targetRva = 0x4D8BE0;
        long targetVa = 0x180000000L + targetRva;
        Console.WriteLine("Direct pointers to VA 0x" + targetVa.ToString("X") + ":");
        for (int i = 0; i <= b.Length - 8; i += 8) {
            if (BitConverter.ToInt64(b, i) == targetVa) {
                // Find RVA for file offset i
                foreach (var sec in secs) {
                    if (i >= sec.rawAddr && i < sec.rawAddr + sec.rawSize) {
                        int rva = (i - sec.rawAddr) + sec.virtAddr;
                        Console.WriteLine(string.Format("  file 0x{0:X} (RVA 0x{1:X} in {2})", i, rva, sec.name));
                    }
                }
            }
        }
    }
}
