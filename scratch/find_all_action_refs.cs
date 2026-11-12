using System;
using System.IO;

public class FindAllActionRefs {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToRva(int off) {
        if (off >= 0x400 && off < 0x235600) return (off - 0x400) + 0x1000;
        if (off >= 0x235600 && off < 0x129C000) return (off - 0x235600) + 0x237000;
        return off;
    }

    public static void Main() {
        for (int id = 2985; id <= 3005; id++) {
            byte[] b = BitConverter.GetBytes(id);
            for (int off = 0x235600; off < 0x129C000 - 4; off++) {
                if (bytes[off] == b[0] && bytes[off+1] == b[1] && bytes[off+2] == 0 && bytes[off+3] == 0) {
                    byte prev = bytes[off - 1];
                    // Common imm32 movs: BA (mov edx), B9 (mov ecx), B8 (mov eax), 41 B8 (mov r8d), 41 B9 (mov r9d), C7 (mov [mem])
                    if (prev == 0xBA || prev == 0xB9 || prev == 0xB8 || prev == 0x41 || prev == 0x68) {
                        Console.WriteLine(string.Format("Action {0} referenced at RVA 0x{1:X} (prev byte: 0x{2:X2})", id, ToRva(off), prev));
                    }
                }
            }
        }
    }
}

