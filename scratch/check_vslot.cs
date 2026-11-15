using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetFo = (0x495908 - 0x237000) + 0x235600;
        Console.WriteLine("Bytes at 495908: " + BitConverter.ToString(bytes, targetFo, 16));
        // 4C 8B 88 C0 05 00 00 = mov r9, [rax + 0x5C0]
        // FF 90 B8 05 00 00 = call [rax + 0x5B8]
        // 0x5B8 / 8 = 0xB7 (slot 183)
        Console.WriteLine("Slot 0x5B8 = index " + (0x5B8 / 8));
    }
}
