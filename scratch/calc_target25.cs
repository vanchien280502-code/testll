using System;

class Program {
    static void Main() {
        long rip = 0x4D8FCFL + 0x2CL;
        long disp = 0x121588DL;
        long target = rip + disp;
        Console.WriteLine("Target at line 0025: 0x{0:X}", target);
    }
}

