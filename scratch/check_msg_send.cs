using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // Look at TaskPacket in NativeBackend.cs line 4248:
        // game+0x16ee888 is Session class slot!
        // game+0x4340 is Session.sendMessage thunk!
        // In TaskPacketCore:
        // a.Emit(0xb9, 0x14, 0, 0, 0); // mov ecx, 20
        // a.Emit(0x48, 0x89, 0xf2); // mov rdx, rsi (Message)
        // a.Call(sendMessageThunk); // 0x4340
        Console.WriteLine("0x4340 is sendMessage!");
    }
}

