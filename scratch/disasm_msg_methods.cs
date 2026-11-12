using System;
using System.IO;

public class DisasmMsgMethods {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Message class slot is at 0x16EE820
        // In TaskPacketCore:
        // allocate is at game + 0x16F760 (il2cpp_object_new)
        // ctor is at game + 0x2E2AA0 (or 0x2E28A0)

        // Let's inspect 0x4E6A10 again:
        // 48 8B 03             mov rax, [rbx]
        // 4C 8B 80 50 02 00 00 mov r8, [rax+250h]
        // 8B 57 20             mov edx, [rdi+20h]
        // 48 8B CB             mov rcx, rbx
        // FF 90 48 02 00 00    call qword ptr [rax+248h] ; writeShort

        // 48 8B 03             mov rax, [rbx]
        // 4C 8B 80 20 02 00 00 mov r8, [rax+220h]
        // 40 0F B6 D6          movzx edx, sil
        // 48 8B CB             mov rcx, rbx
        // FF 90 18 02 00 00    call qword ptr [rax+218h] ; writeByte(sbyte) or writeByte(byte)

        Console.WriteLine("Offset 0x248: method at 0x248, methodInfo at 0x250");
        Console.WriteLine("Offset 0x218: method at 0x218, methodInfo at 0x220");
        Console.WriteLine("Offset 0x228: method at 0x228, methodInfo at 0x230");
        Console.WriteLine("Offset 0x298: method at 0x298, methodInfo at 0x2A0");

        // Notice: in il2cpp vtable, each virtual method has:
        // methodPtr at offset N
        // methodInfo at offset N+8!
        // So:
        // Method A: ptr at 0x218, info at 0x220
        // Method B: ptr at 0x228, info at 0x230
        // Method C: ptr at 0x238, info at 0x240
        // Method D: ptr at 0x248, info at 0x250
        // Method E: ptr at 0x258, info at 0x260
        // ...
        // Method J: ptr at 0x298, info at 0x2A0
    }
}

