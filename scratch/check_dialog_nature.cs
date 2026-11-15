using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Let's check callers of 0x374AD0 again:
        // RVA 0x496FB0
        // RVA 0x497375
        // RVA 0x4974C3
        // Notice all 3 callers are inside class methods of this dialog!
        Console.WriteLine("0x374AD0 callers are all inside the Lucky Wheel class itself!");
    }
}

