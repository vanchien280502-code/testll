using System;

class Program {
    static void Main() {
        int instrIp = 0x2DB340 - 32 + 3; // wait, what was the offset in slice?
        // In check_2db340.cs:
        // slice was from fileOffset - 32.
        // 0000 was at fileOffset - 32.
        // 0003 had E8 E8 A0 D2 FF.
        // So instr is at (0x2DB340 - 32) + 3 = 0x2DB323.
        int nextIp = 0x2DB323 + 5;
        int rel = BitConverter.ToInt32(new byte[]{0xE8, 0xA0, 0xD2, 0xFF}, 0);
        long target = (long)nextIp + rel;
        Console.WriteLine("Target at 0003 = 0x{0:X}", target);
    }
}

