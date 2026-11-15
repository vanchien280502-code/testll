using System;

class Program {
    static void Main() {
        int instrEnd = 0x4D8F8E;
        int rel = BitConverter.ToInt32(new byte[]{0x32, 0x18, 0xCD, 0xFF}, 0);
        long target = (long)instrEnd + rel;
        Console.WriteLine("Target of 0x4D8F89 = 0x{0:X}", target);
    }
}

