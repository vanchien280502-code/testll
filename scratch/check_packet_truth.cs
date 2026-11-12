using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Message class slot is at 0x16EE820
        // In TaskPacketCore:
        // allocate: game + 0x16F760
        // serviceSlot: game + 0x16EE820
        // Let's find Message vtable
        // In GameAssembly.dll, let's look at 0x4D8FCF (Case 1 for Action 2986):
        // 0x4D8FD3: CALL 0x2E28A0
        // 0x4D8FEF: CALL 0x1AA7C0
        // 0x4D9017: CALL 0x4340 -> Session.sendMessage!
        // 0x4D9029: CALL [rax+0x298]
        Console.WriteLine("Case 1 calls 0x4340 (Session.sendMessage) first, and THEN [rax+0x298]!");
        Console.WriteLine("TaskPacketCore NEVER calls 0x4340 (Session.sendMessage)!");
    }
}

