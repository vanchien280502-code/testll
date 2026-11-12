using System;
using System.Collections.Generic;

class Code64 {
    internal readonly List<byte> Bytes=new List<byte>();
    internal void Emit(params byte[] b){Bytes.AddRange(b);}
    internal void Q(long v){Bytes.AddRange(BitConverter.GetBytes(v));}
    internal void Rax(long v){Emit(0x48,0xb8);Q(v);}
    internal byte[] Finish(){return Bytes.ToArray();}
}

class Program {
    static void Main() {
        Code64 a = new Code64();
        long actionData = 0x1234567812345678L;
        long returnTarget = 0x1804D8BF4L;
        byte[] prefix = { 0x44, 0x89, 0x4c, 0x24, 0x20, 0x44, 0x89, 0x44, 0x24, 0x18, 0x48, 0x89, 0x54, 0x24, 0x10, 0x48, 0x89, 0x4c, 0x24, 0x08 };
        
        a.Emit(0x50,0x51,0x52,0x9c);
        a.Rax(actionData);
        a.Emit(0x89,0x10);
        a.Emit(0x4c,0x89,0x40,0x08);
        a.Emit(0x4c,0x89,0x48,0x10);
        a.Emit(0x48,0x89,0x48,0x18);
        a.Emit(0xf0,0xff,0x40,0x20);
        a.Emit(0x9d,0x5a,0x59,0x58);
        a.Emit(prefix);
        a.Emit(0xff,0x25,0,0,0,0);
        a.Q(returnTarget);
        
        byte[] bytes = a.Finish();
        Console.WriteLine("Length: " + bytes.Length);
        Console.WriteLine(BitConverter.ToString(bytes).Replace('-', ' '));
    }
}

