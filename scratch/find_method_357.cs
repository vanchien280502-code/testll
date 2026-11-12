using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        byte[] dll = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        int stringOffset = BitConverter.ToInt32(meta, 24);
        int stringSize = BitConverter.ToInt32(meta, 28);
        int methodsOffset = BitConverter.ToInt32(meta, 64);
        int methodsSize = BitConverter.ToInt32(meta, 68);

        // In il2cpp, method definitions are in methodsOffset
        // Each Il2CppMethodDefinition is 56 or 64 bytes (version 24.x = 36 or 40 bytes)
        // Let's search for method whose method pointer in metadata/dll corresponds to 0x357A00 or 0x357AB8
        Console.WriteLine("Searching for method near 0x357A00...");
        // Let's search CodeRegistration methodPointers
        // In GameAssembly.dll, let's look at pdata
    }
}

