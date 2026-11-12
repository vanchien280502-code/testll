using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // Message class is at 0x16ee820 or similar? Let's check NativeBackend.cs line 4248
        // game+0x16ee820 is Message class slot!
        // In file offset:
        int classSlotRva = 0x16EE820;
        int off = (classSlotRva - 0x237000) + 0x235600;
        Console.WriteLine("Message class slot at 0x{0:X}: {1}", classSlotRva, BitConverter.ToString(bytes, off, 8));
    }
}

