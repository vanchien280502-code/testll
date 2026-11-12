using System;
using System.IO;

class Program {
    static void Main() {
        string path = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll";
        byte[] bytes = File.ReadAllBytes(path);
        // In 0x2E28A0 / 0x2E29A0:
        // 0056: 4D 8B 81 30 02 00 00 mov r8, [r9 + 0x230]
        // 005D: 41 FF 91 28 02 00 00 call [r9 + 0x228]
        // Let's find Message class vtable in memory or find where 0x2E28A0 / 0x2E29A0 are in g_MethodPointers
        // In check_msg_vtable.cs, let's look at Message methods
        // Message has methods:
        // WriteByte, WriteShort, WriteInt, WriteUTF, etc.
        Console.WriteLine("Searching for DataOutputStream methods...");
    }
}

