using System;
using System.IO;
using System.Text;

public class FindWheelClass {
    public static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        byte[] dll = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

        // Search metadata method definitions
        int methodsOffset = BitConverter.ToInt32(meta, 0x40); // 64
        int methodsSize = BitConverter.ToInt32(meta, 0x44); // 68
        int stringOffset = BitConverter.ToInt32(meta, 0x18);

        Console.WriteLine("MethodsOffset: 0x{0:X}, Size: 0x{1:X}", methodsOffset, methodsSize);
        // Each method definition in IL2CPP v24/v27:
        // struct Il2CppMethodDefinition { int nameIndex; int declaringType; int returnType; int parameterStart; ... }
        // Let's find methods pointing to or near 0x501350 / 0x5018A0 / 0x4D8BE0
        // Wait, CodeRegistration methodPointers are in GameAssembly.dll!
        
        // Let's search for the pointers 0x180501350 or 0x1805018A0 or 0x1804D8BE0 in GameAssembly.dll
        long imageBase = 0x180000000;
        long target1 = imageBase + 0x501350;
        long target2 = imageBase + 0x5018A0;
        long target3 = imageBase + 0x4D8BE0;

        byte[] p1 = BitConverter.GetBytes(target1);
        byte[] p2 = BitConverter.GetBytes(target2);
        byte[] p3 = BitConverter.GetBytes(target3);

        for (int i = 0; i < dll.Length - 8; i += 8) {
            long val = BitConverter.ToInt64(dll, i);
            if (val == target1) Console.WriteLine("target1 (0x501350) ref at DLL offset 0x{0:X}", i);
            if (val == target2) Console.WriteLine("target2 (0x5018A0) ref at DLL offset 0x{0:X}", i);
            if (val == target3) Console.WriteLine("target3 (0x4D8BE0) ref at DLL offset 0x{0:X}", i);
        }
    }
}

