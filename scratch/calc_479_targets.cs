using System;

class Program {
    static void Main() {
        int rva1 = 0x47978B;
        int next1 = rva1 + 7;
        int disp1 = BitConverter.ToInt32(new byte[]{0xF6, 0x50, 0x27, 0x01}, 0);
        Console.WriteLine("Target 1 = 0x{0:X}", next1 + disp1);

        int rva2 = 0x479798;
        int next2 = rva2 + 7;
        int disp2 = BitConverter.ToInt32(new byte[]{0xEA, 0x50, 0x27, 0x01}, 0);
        Console.WriteLine("Target 2 = 0x{0:X}", next2 + disp2);
    }
}

