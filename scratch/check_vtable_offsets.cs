using System;
using System.IO;

public class CheckVtableOffsets {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Let's inspect instructions in Message methods
        // Message class methods:
        // writeByte, writeShort, writeInt, writeUTF, send, etc.
        // Let's find Message_ctor at 0x2E28A0
        int ctorOffset = (0x2E28A0 - 0x237000) + 0x235600;
        Console.WriteLine("Message_ctor at 0x2E28A0 (offset 0x{0:X}):", ctorOffset);
        for (int i = 0; i < 60; i++) {
            Console.Write("{0:X2} ", bytes[ctorOffset + i]);
        }
        Console.WriteLine();
    }
}

