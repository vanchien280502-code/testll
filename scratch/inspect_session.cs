using System;
using System.IO;

public class InspectSession {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        // game + 0x16ee888 is sessionSlot in NativeBackend.cs!
        // Let's inspect the pointer / class at 0x16ee888
        int rva = 0x16ee888;
        Console.WriteLine("Offset of 0x16ee888: 0x{0:X}", rva);
    }
}

