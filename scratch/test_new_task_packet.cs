using System;
using System.Collections.Generic;

class Code64 {
    internal readonly List<byte> Bytes=new List<byte>();
    internal readonly Dictionary<string,int> Labels=new Dictionary<string,int>();
    internal readonly List<Tuple<int,string,int>> Fixups=new List<Tuple<int,string,int>>();
    internal void Emit(params byte[] b){Bytes.AddRange(b);}
    internal void Q(long v){Bytes.AddRange(BitConverter.GetBytes(v));}
    internal void Rax(long v){Emit(0x48,0xb8);Q(v);}
    internal void Rcx(long v){Emit(0x48,0xb9);Q(v);}
    internal void Call(long v){Emit(0x48,0xb8);Q(v);Emit(0xff,0xd0);}
    internal void Label(string n){Labels[n]=Bytes.Count;}
    internal void Jump(string n){Emit(0xe9);Fixups.Add(Tuple.Create(Bytes.Count,n,4));Emit(0,0,0,0);}
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
    static void Slot(Code64 a,long address,string miss,long initialize=0) {
        if(initialize!=0){
            string ready="slot_"+a.Bytes.Count;
            a.Rax(address);a.Emit(0x48,0x8b,0x00,0xa8,0x01);a.J(0x84,ready);
            a.Rcx(address);a.Call(initialize);a.Label(ready);
        }
        a.Rax(address);a.Emit(0x48,0x8b,0x00,0x48,0x85,0xc0);a.J(0x84,miss);a.Emit(0xa8,0x01);a.J(0x85,miss);
    }

    static byte[] TaskPacketCore(long data,long serviceSlot,long allocate,long ctor,long initialize,long sessionSlot=0,long sendMessageThunk=0,long factory90=0,long factory91=0){
        Code64 a=new Code64();
        a.Emit(0x53,0x56,0x48,0x83,0xec,0x28,0x48,0xbb);a.Q(data);
        a.Emit(0x83,0x3b,1);a.J(0x85,"return");a.Emit(0xc7,0x03,2,0,0,0);

        if(factory90!=0){
            a.Emit(0x8b,0x43,4); // mov eax, [rbx+4] (opcode)
            a.Emit(0x3d,0x90,0,0,0); // cmp eax, 144 (0x90)
            a.J(0x85,"normal_packet"); // jne normal_packet

            // Send Message(144) with payload byte
            a.Emit(0x8a,0x4b,12); // mov cl, [rbx+12] (payload)
            a.Emit(0x33,0xd2);    // xor edx, edx
            a.Call(factory90);    // 0x2E29A0
            a.Emit(0x48,0x85,0xc0);a.J(0x84,"failed");
            a.Emit(0x4c,0x8b,0x00); // mov r8, [rax] (vtable)
            a.Emit(0x48,0x89,0xc1); // mov rcx, rax (Message)
            a.Emit(0x49,0x8b,0x90,0xa0,2,0,0); // mov rdx, [r8+0x2a0]
            a.Emit(0x41,0xff,0x90,0x98,2,0,0); // call [r8+0x298] (send 144)

            // If factory91 is provided and payload2 >= 0, send Message(145) with payload2 byte
            if(factory91!=0){
                a.Emit(0x83,0x7b,16,0); // cmp dword ptr [rbx+16], 0
                a.J(0x8c,"wheel_done"); // jl wheel_done (if payload2 < 0)
                a.Emit(0x8a,0x4b,16);   // mov cl, [rbx+16] (payload2)
                a.Emit(0x33,0xd2);      // xor edx, edx
                a.Call(factory91);      // 0x2E2A20
                a.Emit(0x48,0x85,0xc0);a.J(0x84,"wheel_done");
                a.Emit(0x4c,0x8b,0x00); // mov r8, [rax]
                a.Emit(0x48,0x89,0xc1); // mov rcx, rax
                a.Emit(0x49,0x8b,0x90,0xa0,2,0,0); // mov rdx, [r8+0x2a0]
                a.Emit(0x41,0xff,0x90,0x98,2,0,0); // call [r8+0x298] (send 145)
            }
            a.Label("wheel_done");
            a.Emit(0xc7,0x03,0,0,0,0); // state = 0 (done)
            a.Jump("done");
            a.Label("normal_packet");
        }

        Slot(a,serviceSlot,"failed",initialize);a.Emit(0x48,0x89,0xc1);a.Call(allocate);
        a.Emit(0x48,0x85,0xc0);a.J(0x84,"failed");
        a.Emit(0x48,0x89,0xc6,0x48,0x89,0xc1,0x8b,0x53,4,0x45,0x33,0xc0);a.Call(ctor);
        a.Emit(0x83,0x7b,8,0);a.J(0x84,"send");
        a.Emit(0x83,0x7b,8,2);a.J(0x84,"write_short");
        a.Emit(0x83,0x7b,8,3);a.J(0x84,"turnin_item");
        a.Emit(0x83,0x7b,8,4);a.J(0x84,"two_shorts");
        a.Emit(0x83,0x7b,8,5);a.J(0x84,"short_byte");
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x30,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x28,2,0,0);
        a.Jump("send");
        a.Label("write_short");
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
        a.Jump("send");
        a.Label("turnin_item");
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x30,2,0,0,0x48,0x89,0xf1,0xba,1,0,0,0,0xff,0x90,0x28,2,0,0);
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
        a.Jump("send");
        a.Label("two_shorts");
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,16,0xff,0x90,0x48,2,0,0);
        a.Jump("send");
        a.Label("short_byte");
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
        a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x20,2,0,0,0x48,0x89,0xf1,0x8b,0x53,16,0xff,0x90,0x18,2,0,0);
        a.Label("send");
        if(sessionSlot!=0&&sendMessageThunk!=0){
            a.Rax(sessionSlot);
            a.Emit(0x48,0x8b,0x00);
            a.Emit(0x48,0x85,0xc0);a.J(0x84,"cleanup");
            if(initialize!=0){
                string ready="init_ready_"+a.Bytes.Count;
                a.Emit(0x83,0xb8,0xe0,0,0,0,0);
                a.J(0x85,ready);
                a.Emit(0x48,0x89,0xc1);
                a.Call(initialize);
                a.Rax(sessionSlot);a.Emit(0x48,0x8b,0x00);
                a.Emit(0x48,0x85,0xc0);a.J(0x84,"cleanup");
                a.Label(ready);
            }
            a.Emit(0x48,0x8b,0x80,0xb8,0,0,0);
            a.Emit(0x48,0x85,0xc0);a.J(0x84,"cleanup");
            a.Emit(0x4c,0x8b,0x40,0x18);
            a.Emit(0x4d,0x85,0xc0);a.J(0x84,"cleanup");
            a.Emit(0xb9,0x14,0,0,0);
            a.Emit(0x48,0x89,0xf2);
            a.Call(sendMessageThunk);
        }
        a.Label("cleanup");
        a.Emit(0x48,0x8b,0x06,0x48,0x8b,0x90,0xa0,2,0,0,0x48,0x89,0xf1,0xff,0x90,0x98,2,0,0,0xc7,0x03,0,0,0,0);
        a.Jump("done");a.Label("failed");a.Emit(0xc7,0x03,3,0,0,0);
        a.Label("done");a.Label("return");a.Emit(0x48,0x83,0xc4,0x28,0x5e,0x5b,0xc3);return a.Finish();
    }

    static void Main() {
        byte[] bytes = TaskPacketCore(0x1000, 0x2000, 0x3000, 0x4000, 0x5000, 0x6000, 0x7000, 0x8000, 0x9000);
        Console.WriteLine("Generated bytes length: " + bytes.Length);
    }
}

