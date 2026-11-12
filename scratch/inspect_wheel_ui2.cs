using System;
using System.IO;

public class InspectWheelUI2 {
    static byte[] dll = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");

    public static void Main() {
        long imageBase = 0x180000000;
        long target1 = imageBase + 0x501350;
        long target2 = imageBase + 0x5018A0;

        byte[] b1 = BitConverter.GetBytes(target1);
        byte[] b2 = BitConverter.GetBytes(target2);

        for (int i = 0; i < dll.Length - 8; i++) {
            if (dll[i] == b1[0] && dll[i+1] == b1[1] && dll[i+2] == b1[2]) {
                long p = BitConverter.ToInt64(dll, i);
                if (p == target1) Console.WriteLine("target1 at 0x{0:X}", i);
                if (p == target2) Console.WriteLine("target2 at 0x{0:X}", i);
            }
        }
    }
}

