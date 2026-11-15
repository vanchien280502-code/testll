using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int vtableRva = 0x16C1D30;
        int foData = 0x2C00;
        for (int off = 0x50; off <= 0x500; off += 8) {
            int classRva = vtableRva - off;
            int fo = classRva - foData;
            if (fo < 0 || fo >= bytes.Length - 0x200) continue;
            long namePtr = BitConverter.ToInt64(bytes, fo + 0x10);
            long nameRva = namePtr - 0x180000000L;
            if (nameRva > 0 && nameRva < 0x2000000) {
                // Read string at nameRva
                int nameFo = (int)nameRva - 0x1A00; // if in .rdata
                if (nameFo >= 0 && nameFo < bytes.Length - 50) {
                    string str = "";
                    for (int k = 0; k < 50 && bytes[nameFo + k] != 0 && bytes[nameFo + k] >= 32 && bytes[nameFo + k] <= 126; k++) {
                        str += (char)bytes[nameFo + k];
                    }
                    if (str.Length >= 2) {
                        Console.WriteLine("Found Class at RVA 0x{0:X} (vtable offset +0x{1:X}): '{2}'", classRva, off, str);
                    }
                }
            }
        }
    }
}

