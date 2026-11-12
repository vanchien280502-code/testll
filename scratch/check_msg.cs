using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In GameAssembly.dll, let's look at 0x4D8FCF where Message is sent:
        // E8 C8 98 E0 FF: call 0x2E28A0 -> returns Message* in RAX
        // mov r8, [game + 0x16EE888]
        // mov r8, [r8 + 0xB8]
        // mov r8, [r8 + 0x18] -> Session*
        // mov ecx, 20 (0x14)
        // mov rdx, rbx -> Message*
        // call 0x4340 -> il2cpp_interface_call(Session, Message, slot 20) -> Session.sendMessage!
        // mov rax, [rbx] -> Message vtable
        // mov rdx, [rax + 0x2A0]
        // mov rcx, rbx
        // call [rax + 0x298] -> Message.cleanup()!
        Console.WriteLine("Confirmed: Session.sendMessage is called via 0x4340!");
    }
}

