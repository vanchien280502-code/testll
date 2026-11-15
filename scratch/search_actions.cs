using System;
using System.IO;

class SearchActions {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    static void Search(int actionId) {
        Console.WriteLine("Searching for {0} (0x{0:X})...", actionId);
        byte[] b = BitConverter.GetBytes(actionId);
        for (int i = 0; i < bytes.Length - 5; i++) {
            if ((bytes[i] == 0xBA || bytes[i] == 0xB9) &&
                bytes[i+1] == b[0] && bytes[i+2] == b[1] && bytes[i+3] == b[2] && bytes[i+4] == b[3]) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("  Found mov reg, {0} at RVA 0x{1:X}", actionId, rva);
            }
            if (bytes[i] == 0x41 && (bytes[i+1] == 0xB8 || bytes[i+1] == 0xB9) &&
                bytes[i+2] == b[0] && bytes[i+3] == b[1] && bytes[i+4] == b[2] && bytes[i+5] == b[3]) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("  Found mov r8/r9, {0} at RVA 0x{1:X}", actionId, rva);
            }
            // cmp ..., imm32: 81 ...
            if (bytes[i] == 0x81 && bytes[i+2] == b[0] && bytes[i+3] == b[1] && bytes[i+4] == b[2] && bytes[i+5] == b[3]) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("  Found cmp ..., {0} at RVA 0x{1:X}", actionId, rva);
            }
        }
    }

    static void Main() {
        Search(5002);
        Search(2002);
        Search(207);
        Search(382);
    }
}
