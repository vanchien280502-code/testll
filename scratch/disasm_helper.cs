using System;
using System.IO;

public class DisasmHelper {
    static byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
    static int ToFileOffset(int rva) { return (rva - 0x237000) + 0x235600; }

    public static void Main() {
        int callRva = 0x4D8FB1;
        int callOff = ToFileOffset(callRva);
        int disp = BitConverter.ToInt32(bytes, callOff + 1);
        int targetRva = callRva + 5 + disp;
        Console.WriteLine(string.Format("Call at 0x{0:X} -> Target RVA: 0x{1:X}", callRva, targetRva));

        int targetOff = ToFileOffset(targetRva);
        Console.WriteLine("Disassembling target 0x" + targetRva.ToString("X") + ":");
        for (int i = 0; i < 80; i++) {
            Console.Write("{0:X2} ", bytes[targetOff + i]);
            if ((i + 1) % 16 == 0) Console.WriteLine();
        }
        Console.WriteLine();
    }
}

