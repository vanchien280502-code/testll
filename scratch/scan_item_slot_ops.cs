using System;
using System.IO;

public class ScanItemSlotOps {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // We know item.slot is at offset 0x20 of Item object.
        // Let's find instructions "8B 57 20" or "8B 47 20" or "8B 53 20" (mov edx, [reg+20h])
        // followed shortly by call writeShort (or vtable call offset 0x248)
        Console.WriteLine("Scanning functions using item.slot (offset 0x20)...");
        for (int i = 0; i < bytes.Length - 10; i++) {
            if ((bytes[i] == 0x8B && (bytes[i+1] == 0x57 || bytes[i+1] == 0x53 || bytes[i+1] == 0x56) && bytes[i+2] == 0x20) ||
                (bytes[i] == 0x0F && bytes[i+1] == 0xBF && (bytes[i+2] == 0x57 || bytes[i+2] == 0x53 || bytes[i+2] == 0x56) && bytes[i+3] == 0x20)) {
                // Check if nearby there is Message opcode (B2 xx)
                int rva = i - 0x235600 + 0x237000;
                for (int j = -40; j < 40; j++) {
                    int pos = i + j;
                    if (pos >= 0 && pos < bytes.Length - 5) {
                        if (bytes[pos] == 0xB2 && bytes[pos+1] >= 0x60 && bytes[pos+1] <= 0x90) {
                            Console.WriteLine("Found Opcode 0x{0:X2} ({0}) near RVA 0x{1:X}", bytes[pos+1], rva);
                            break;
                        }
                    }
                }
            }
        }
    }
}

