using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        for (int i = 0x501400; i < 0x5018A0; i++) {
            int fo = (i - 0x237000) + 0x235600;
            // sub rsp, ... or push rbp / push rbx
            if ((b[fo] == 0x48 && b[fo+1] == 0x83 && b[fo+2] == 0xEC) ||
                (b[fo] == 0x48 && b[fo+1] == 0x81 && b[fo+2] == 0xEC) ||
                (b[fo] == 0x40 && b[fo+1] == 0x55) ||
                (b[fo] == 0x40 && b[fo+1] == 0x53) ||
                (b[fo] == 0x55 && b[fo+1] == 0x57)) {
                Console.WriteLine("Candidate prologue at 0x" + i.ToString("X") + ": " + BitConverter.ToString(b, fo, 16));
            }
        }
    }
}
