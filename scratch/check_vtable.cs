using System;
using System.IO;

class Program {
    static void Main() {
        // Let's find what function pointer is at vtable offset 0x298 of Message class
        // In il2cpp, vtable is at class + 0x138 or similar, or methods are in metadata
        // Let's check 0x258F7B again:
        // call [r8 + 0x298]
        // Look at the caller at 0x258F48:
        // class = 0x16EE820
        // alloc = 0x16F760 -> returns instance in rax
        // ctor = 0x2E2AA0
        // then:
        // mov r8, [rbx] -> r8 is klass! (in IL2CPP, an object's first pointer is Il2CppClass* klass!)
        // then: mov rdx, [r8 + 0x2A0] -> MethodInfo*
        // then: mov rcx, rbx -> object instance
        // then: call qword ptr [r8 + 0x298] -> virtual method slot 0!
        Console.WriteLine("In IL2CPP x64: [instance] = klass.");
        Console.WriteLine("klass + 0x138 is the start of the vtable, or vtable slots start at 0x138/0x140.");
        Console.WriteLine("Offset 0x298 = 664. Let's see how many 8-byte slots from 0x138 (312): (664 - 312) / 8 = 44.");
    }
}

