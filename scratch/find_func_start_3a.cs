using System;
using System.IO;

public class FindFuncStart {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }
    static int ToRva(int off) { return (off - 0x235600) + 0x237000; }

    public static void Main() {
        int off = ToFileOffset(0x3A0904);
        // scan backwards for prologue: 48 89 5C 24 or 40 53 or 48 83 EC or 55 48 89 E5
        for (int i = 0; i < 0x2000; i++) {
            int cur = off - i;
            if ((bytes[cur] == 0x40 && bytes[cur+1] == 0x53 && bytes[cur+2] == 0x48 && bytes[cur+3] == 0x83 && bytes[cur+4] == 0xEC) ||
                (bytes[cur] == 0x48 && bytes[cur+1] == 0x89 && bytes[cur+2] == 0x5C && bytes[cur+3] == 0x24) ||
                (bytes[cur] == 0x48 && bytes[cur+1] == 0x83 && bytes[cur+2] == 0xEC && bytes[cur-1] == 0xCC)) {
                Console.WriteLine("Prologue found at RVA: 0x" + ToRva(cur).ToString("X") + " (distance -0x" + i.ToString("X") + ")");
                break;
            }
        }
    }
}

