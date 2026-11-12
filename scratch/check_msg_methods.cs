using System;
using System.IO;

public class MessageMethods {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Message ctor at RVA 0x2E28A0
        // Let's find Message class vtable
        // Search for xref to 0x2E28A0 in .rdata or .data
        long vaCtor = 0x180000000L + 0x2E28A0;
        byte[] p = BitConverter.GetBytes(vaCtor);
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            bool m = true;
            for (int j = 0; j < 8; j++) if (bytes[i + j] != p[j]) { m = false; break; }
            if (m) {
                Console.WriteLine("Message_ctor pointer found at offset 0x{0:X}", i);
                // Dump vtable entries around this
                for (int slot = 0x200; slot <= 0x2B0; slot += 8) {
                    long slotVa = BitConverter.ToInt64(bytes, i - 0x120 + slot); // approximate
                    // Let's print
                }
            }
        }
    }
}

