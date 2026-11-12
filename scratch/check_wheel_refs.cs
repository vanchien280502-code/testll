using System;
using System.IO;

public class CheckWheelRefs {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        // Check static class references in Function 1 and 2
        for (int rva = 0x500FF0; rva < 0x501997; rva++) {
            int off = ToFileOffset(rva);
            // 48 8D 0D disp32 / 48 8B 05 disp32
            if (bytes[off] == 0x48 && (bytes[off+1] == 0x8D || bytes[off+1] == 0x8B) && (bytes[off+2] == 0x0D || bytes[off+2] == 0x05 || bytes[off+2] == 0x15)) {
                int disp = BitConverter.ToInt32(bytes, off + 3);
                int targetRva = rva + 7 + disp;
                Console.WriteLine(string.Format("RVA 0x{0:X}: ref to 0x{1:X}", rva, targetRva));
            }
        }
    }
}

