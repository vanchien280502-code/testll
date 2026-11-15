using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // In 0x374AD0:
        // mov rcx, [rax + rcx*8 + 0x20] (button)
        // cmp [rcx + 0x14], ebp (actionId)
        // jne next
        // call [button.vtable + 0x438]
        // Let's find callers of performAction or how button is checked:
        // Let's see: in 0x496FA8:
        // mov edx, 207
        // mov rcx, rbx (dialog)
        // call 0x374AD0
        Console.WriteLine("Button check");
    }
}

