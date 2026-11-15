using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int targetRva = 0x4A56F6;
        int targetFo = (targetRva - 0x237000) + 0x235600;
        // Search backwards for mov rdi, ...
        for (int fo = targetFo; fo > targetFo - 0x500; fo--) {
            // mov rdi, [rbp + ...] or mov rdi, rax
            if (bytes[fo] == 0x48 && (bytes[fo+1] == 0x8B || bytes[fo+1] == 0x89) && (bytes[fo+2] & 0x38) == 0x38) {
                int rva = (fo - 0x235600) + 0x237000;
                Console.WriteLine("Instruction at 0x{0:X}: {1}", rva, BitConverter.ToString(bytes, fo, 8));
            }
        }
    }
}
