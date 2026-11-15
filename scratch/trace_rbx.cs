using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // At 0x4E4090:
        // What is rbx?
        // Let's trace backward from 0x4E4070 in 0x4E3A74 function:
        // Where did rbx come from in this function?
        int rva = 0x4E3A74;
        int fo = rva - 0x1A00;
        Console.WriteLine("Tracing rbx from function start 0x4E3A74...");
        // Let's dump the prologue of 0x4E3A74:
        for (int i = 0; i < 96; i += 16) {
            Console.WriteLine("0x{0:X6}: {1}", rva + i, BitConverter.ToString(bytes, fo + i, 16));
        }
    }
}
