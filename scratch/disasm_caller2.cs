using System;
using System.IO;

public class DisasmCaller2 {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        int rva = 0x4C7FD3;
        int fileOffset = (rva - 0x237000) + 0x235600;

        int start = fileOffset;
        for (int i = 0; i < 400; i++) {
            if (bytes[start] == 0xC3 && (bytes[start + 1] == 0xCC || bytes[start + 1] == 0x90 || bytes[start + 1] == 0x48 || bytes[start + 1] == 0x40 || bytes[start + 1] == 0x55)) {
                start = start + 1;
                while (bytes[start] == 0xCC || bytes[start] == 0x90) start++;
                break;
            }
            start--;
        }

        int funcRva = start - 0x235600 + 0x237000;
        Console.WriteLine("Function start estimated at RVA 0x{0:X} (offset 0x{1:X})", funcRva, start);
        int len = (fileOffset - start) + 80;
        Console.WriteLine("Bytes ({0}):", len);
        for (int i = 0; i < len; i++) {
            int cur = start + i;
            if (cur == fileOffset) Console.Write("[CALL>>] ");
            Console.Write("{0:X2} ", bytes[cur]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

