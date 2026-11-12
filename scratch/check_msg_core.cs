using System;
using System.IO;

public class CheckMsgVtableOffsets {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Message ctor at 0x2E28A0
        // In TaskPacketCore:
        // allocate: game + 0x16F760 (il2cpp_object_new)
        // serviceSlot: game + 0x16EE820 (Message class)
        // Let's inspect class at 0x16EE820!
        int rva = 0x16EE820;
        Console.WriteLine("Message class slot at RVA 0x{0:X}", rva);

        // Let's search in TaskPacketCore usages in NativeBackend.cs
        // How is payloadType 5 used currently in the codebase?
    }
}

