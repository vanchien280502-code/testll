using System;
using System.IO;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int stringOffset = BitConverter.ToInt32(meta, 24);
        int stringSize = BitConverter.ToInt32(meta, 28);
        int methodsOffset = BitConverter.ToInt32(meta, 64);
        int methodsSize = BitConverter.ToInt32(meta, 68);
        int typeDefsOffset = BitConverter.ToInt32(meta, 96);
        int typeDefsSize = BitConverter.ToInt32(meta, 100);

        Console.WriteLine("stringOffset: 0x{0:X}, methodsOffset: 0x{1:X}, typeDefsOffset: 0x{2:X}", stringOffset, methodsOffset, typeDefsOffset);
        // TypeDef struct in v24.2:
        // uint nameIndex (0)
        // uint namespaceIndex (4)
        // int byvalTypeIndex (8)
        // int byrefTypeIndex (12)
        // ...
        // ushort method_count (0x30 / 48)
        // ushort methodStart (0x34 / 52)
        // Let's scan typeDefs to find which type contains method 737!
    }
}

