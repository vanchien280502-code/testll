using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetRva = 0x495890;
        int targetFo = (targetRva - 0x237000) + 0x235600;
        // In 495890: call [r10 + 0x3A8]
        // Let's dump the 32 bytes before 4958A0
        Console.WriteLine("Instructions at 495870 - 4958A5:");
        for (int i = 0; i < 48; i += 16) {
            Console.WriteLine("{0:X6}: {1}", targetRva - 32 + i, BitConverter.ToString(bytes, targetFo - 32 + i, 16).Replace('-', ' '));
        }
    }
}
