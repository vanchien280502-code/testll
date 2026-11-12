using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // In Message constructor 0x2E2AA0:
        // Message class slot is 0x16EE820.
        // Let's find Message method pointers in metadata or find Message.send method
        byte[] meta = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int stringOffset = BitConverter.ToInt32(meta, 24);
        int stringSize = BitConverter.ToInt32(meta, 28);
        int methodsOffset = BitConverter.ToInt32(meta, 64);
        int methodsSize = BitConverter.ToInt32(meta, 68);

        // Find "send" in method names
        Console.WriteLine("Searching for methods named 'send'...");
        for (int i = 0; i < methodsSize / 36; i++) {
            int off = methodsOffset + i * 36; // or 40 or 44
            // let's check method names
        }
    }
}

