using System;
using System.Collections.Generic;

class Code64 {
    internal readonly List<byte> Bytes=new List<byte>();
    internal readonly Dictionary<string,int> Labels=new Dictionary<string,int>();
    internal readonly List<Tuple<int,string,int>> Fixups=new List<Tuple<int,string,int>>();
    internal void Emit(params byte[] b){Bytes.AddRange(b);}
    internal void Q(long v){Bytes.AddRange(BitConverter.GetBytes(v));}
    internal void Rax(long v){Emit(0x48,0xb8);Q(v);}
    internal void Label(string n){Labels[n]=Bytes.Count;}
    internal void J(byte op,string n){Emit(0x0f,op);Fixups.Add(Tuple.Create(Bytes.Count,n,4));Emit(0,0,0,0);}
    internal byte[] Finish(){
        foreach(var f in Fixups){
            int target=Labels[f.Item2];
            int rel=target-(f.Item1+f.Item3);
            byte[] b=BitConverter.GetBytes(rel);
            for(int i=0;i<f.Item3;i++)Bytes[f.Item1+i]=b[i];
        }
        return Bytes.ToArray();
    }
}

class Program {
    static void Main() {
        Code64 a = new Code64();
        long actionData = 0x1234567812345678L;
        long returnTarget = 0x1804D8BF4L;
        byte[] prefix = { 0x44, 0x89, 0x4c, 0x24, 0x20, 0x44, 0x89, 0x44, 0x24, 0x18, 0x48, 0x89, 0x54, 0x24, 0x10, 0x48, 0x89, 0x4c, 0x24, 0x08 };

        a.Emit(0x50,0x51,0x52,0x9c);
        a.Rax(actionData);
        a.Emit(0x4c,0x89,0x40,0x08);
        a.Emit(0x4c,0x89,0x48,0x10);
        a.Emit(0x48,0x89,0x48,0x18);
        a.Emit(0x89,0x10);
        a.Emit(0x48,0x85,0xd2);
        a.J(0x84,"skip_action");
        a.Emit(0x48,0x81,0xfa,0,0,1,0);
        a.J(0x82,"skip_action");
        a.Emit(0x8b,0x4a,0x18);
        a.Emit(0x89,0x08);
        a.Label("skip_action");
        a.Emit(0xf0,0xff,0x40,0x20);
        a.Emit(0x9d,0x5a,0x59,0x58);
        a.Emit(prefix);
        a.Emit(0xff,0x25,0,0,0,0);
        a.Q(returnTarget);

        byte[] bytes = a.Finish();
        Console.WriteLine("ActionHook bytes length: " + bytes.Length);
    }
}

