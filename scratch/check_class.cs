using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x16C12A0;
        // In GameAssembly.dll, where is 0x16C12A0?
        // Let's find its section:
        // In PE, find section for 0x16C12A0
        int e_lfanew = BitConverter.ToInt32(bytes, 0x3C);
        int numSections = BitConverter.ToInt16(bytes, e_lfanew + 6);
        int optHeaderSize = BitConverter.ToInt16(bytes, e_lfanew + 20);
        int sectionTable = e_lfanew + 24 + optHeaderSize;

        for (int i = 0; i < numSections; i++) {
            int sec = sectionTable + i * 40;
            string secName = Encoding.ASCII.GetString(bytes, sec, 8).Trim('\0');
            int vSize = BitConverter.ToInt32(bytes, sec + 8);
            int vAddr = BitConverter.ToInt32(bytes, sec + 12);
            int rSize = BitConverter.ToInt32(bytes, sec + 16);
            int rOffset = BitConverter.ToInt32(bytes, sec + 20);
            if (rva >= vAddr && rva < vAddr + vSize) {
                int fo = rva - vAddr + rOffset;
                Console.WriteLine("RVA 0x{0:X} in section {1}: FO 0x{2:X}", rva, secName, fo);
                // Let's inspect class name at class - 0x128 or so
                // In Il2CppClass, name pointer is at +0x10
                for (int d = 0; d < 0x200; d += 8) {
                    int classCandidate = fo - d;
                    long namePtr = BitConverter.ToInt64(bytes, classCandidate + 0x10);
                    long imgBase = 0x180000000;
                    long nameRva = namePtr - imgBase;
                    if (nameRva > 0 && nameRva < 0x2000000) {
                        // Check if it points to an ASCII string
                        // Read string
                        int nameFo = (int)nameRva - 0x1A00; // if in .text or .rdata
                        // find section of nameRva
                        for (int s = 0; s < numSections; s++) {
                            int sec2 = sectionTable + s * 40;
                            int vAddr2 = BitConverter.ToInt32(bytes, sec2 + 12);
                            int vSize2 = BitConverter.ToInt32(bytes, sec2 + 8);
                            int rOffset2 = BitConverter.ToInt32(bytes, sec2 + 20);
                            if (nameRva >= vAddr2 && nameRva < vAddr2 + vSize2) {
                                int sFo = (int)(nameRva - vAddr2 + rOffset2);
                                if (sFo >= 0 && sFo < bytes.Length - 30) {
                                    string str = "";
                                    for (int k = 0; k < 30 && bytes[sFo + k] != 0 && bytes[sFo + k] >= 32 && bytes[sFo + k] <= 126; k++) {
                                        str += (char)bytes[sFo + k];
                                    }
                                    if (str.Length >= 2) {
                                        Console.WriteLine("Offset -0x{0:X}: Class name candidate '{1}' (RVA 0x{2:X})", d, str, (rva - d));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

