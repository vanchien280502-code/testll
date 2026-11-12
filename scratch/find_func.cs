using System;
using System.IO;

class Program {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    static void Main() {
        // Let's scan backwards from 0x4D8BE0 for function prologue (sub rsp / push rbx / etc.)
        for (int rva = 0x4D8BE0; rva >= 0x4D8000; rva--) {
            int off = ToFileOffset(rva);
            if (bytes[off] == 0x48 && bytes[off+1] == 0x89 && bytes[off+2] == 0x5C && bytes[off+3] == 0x24) { // mov [rsp+...], rbx
                Console.WriteLine("Possible func start: 0x{0:X}", rva);
            }
            if (bytes[off] == 0x40 && bytes[off+1] == 0x53 && bytes[off+2] == 0x48 && bytes[off+3] == 0x83 && bytes[off+4] == 0xEC) { // push rbx; sub rsp, xx
                Console.WriteLine("Func start (push rbx; sub rsp): 0x{0:X}", rva);
                break;
            }
        }
    }
}

