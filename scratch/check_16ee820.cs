using System;
using System.IO;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"c:\Users\chien\Downloads\langla-291\TGLL_minCPU_v2\GameAssembly.dll");
        // Message class slot is 0x16EE820
        // In TaskPacketCore:
        // Slot(a, serviceSlot, "failed", initialize);
        // serviceSlot = game + 0x16EE820
        // The pointer at 0x16EE820 points to Il2CppClass of Message
        // But in static DLL, 0x16EE820 is in .data (zero initialized or class metadata)
        // Let's find references to 0x16EE820:
        Console.WriteLine("Searching for refs to 0x16EE820...");
        long targetVA = 0x180000000L + 0x16EE820;
        for (int i = 0; i < bytes.Length - 8; i += 8) {
            if (BitConverter.ToInt64(bytes, i) == targetVA) {
                int rva = (i - 0x235600) + 0x237000;
                Console.WriteLine("Ref at RVA 0x{0:X}", rva);
            }
        }
    }
}

