using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // In il2cpp, an Il2CppClass structure has:
        // offset 0x00: Il2CppClass* / Il2CppImage*
        // offset 0x10: const char* name
        // offset 0x18: const char* namespaze
        // ...
        // vtable is at the end of Il2CppClass.
        // Let's search backward from 0x16C12A8 for valid pointers to names in .rdata.
        int vtableOff = 0x16C12A8;
        Console.WriteLine("Scanning backward from 0x16C12A8:");
        for (int i = 0x8; i <= 0x300; i += 8) {
            long p = BitConverter.ToInt64(bytes, vtableOff - i);
            if (p >= 0x180000000L && p < 0x182000000L) {
                int rva = (int)(p - 0x180000000L);
                int fileOff = ToFileOffset(rva);
                if (fileOff > 0 && fileOff < bytes.Length - 100) {
                    // Check if string
                    string s = "";
                    for (int k = 0; k < 40; k++) {
                        byte b = bytes[fileOff + k];
                        if (b >= 32 && b <= 126) s += (char)b;
                        else if (b == 0) break;
                        else { s = ""; break; }
                    }
                    if (s.Length > 2) {
                        Console.WriteLine("  [-0x{0:X3}] pointer to string: \"{1}\" at RVA 0x{2:X}", i, s, rva);
                    }
                }
            }
        }
    }
}

