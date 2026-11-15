using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Look at 0x16ee820 (serviceSlot / Message class)
        int rva = 0x16ee820;
        int fileOffset = (rva - 0x129B000) + 0x1298400; // section .data
        long tokenOrClass = BitConverter.ToInt64(bytes, fileOffset);
        Console.WriteLine(string.Format("Class at 0x16ee820: 0x{0:X16}", tokenOrClass));

        // Find references to 0x16ee820 to find vtable initialization or static fields
        for (int i = 0; i < bytes.Length - 7; i++) {
            if (bytes[i] == 0x48 && bytes[i+1] == 0x8D && (bytes[i+2] & 0xC7) == 0x05) {
                int disp = BitConverter.ToInt32(bytes, i + 3);
                int rvaSource = i - 0x235600 + 0x237000;
                if ((long)rvaSource + 7 + disp == rva) {
                    Console.WriteLine(string.Format("LEA ref to 0x16EE820 at RVA 0x{0:X}", rvaSource));
                }
            }
        }
    }
}

