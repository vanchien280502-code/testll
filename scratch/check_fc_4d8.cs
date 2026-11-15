using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Let's check fcClass vtable around 0x16ee888
        // In NativeBackend.cs:
        // long fcClass = ResolvedClass(r, game + 0x16ee888);
        // fc = [fcClass.static_fields + 0x10]
        // Let's check the Il2CppClass of fc:
        // An object pointer [fc] points to its Il2CppClass!
        Console.WriteLine("Let's check what method is at vtable + 0x4D8 for fcClass!");
    }
}
