using System;
using System.IO;

class Program {
    static void Main() {
        string metaPath = @"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat";
        byte[] meta = File.ReadAllBytes(metaPath);
        int sanity = BitConverter.ToInt32(meta, 0);
        int version = BitConverter.ToInt32(meta, 4);
        int stringLiteralOffset = BitConverter.ToInt32(meta, 8);
        int stringLiteralCount = BitConverter.ToInt32(meta, 12);
        int stringLiteralDataOffset = BitConverter.ToInt32(meta, 16);
        int stringLiteralDataCount = BitConverter.ToInt32(meta, 20);
        int stringOffset = BitConverter.ToInt32(meta, 24);
        int stringCount = BitConverter.ToInt32(meta, 28);
        int typeDefOffset = BitConverter.ToInt32(meta, 0x60);
        int typeDefCount = BitConverter.ToInt32(meta, 0x64);

        Console.WriteLine(string.Format("Sanity: 0x{0:X}, Version: {1}", sanity, version));
        Console.WriteLine(string.Format("TypeDefOffset: 0x{0:X}, Count: {1}", typeDefOffset, typeDefCount));

        // In il2cpp, token 0x20006141:
        // Table index = (0x20006141 >> 24) = 0x20 = TypeDef (table 2) or TypeRef
        // Let's check token & 0xFFFFFF = 0x6141 = 24897
        int tokenIdx = (0x20006141 >> 1) & 0x0FFFFFFF; // or (token - 1)
        Console.WriteLine("Token raw: 0x20006141");
    }
}

