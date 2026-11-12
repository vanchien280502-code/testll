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
    static byte[] SendHook(long actionData, long returnTarget) {
        Code64 a = new Code64();
        // Save volatile registers and flags:
        a.Emit(0x50, 0x51, 0x52, 0x41, 0x50, 0x41, 0x51, 0x41, 0x52, 0x41, 0x53, 0x9c);

        // rdx is Message*
        a.Rax(actionData);
        // Safely check rdx:
        a.Emit(0x48, 0x85, 0xd2); // test rdx, rdx
        a.J(0x84, "skip");
        a.Emit(0x48, 0x81, 0xfa, 0, 0, 1, 0); // cmp rdx, 0x10000
        a.J(0x82, "skip"); // jb skip

        // Read opcode at [rdx + 0x10]:
        a.Emit(0x0f, 0xb6, 0x4a, 0x10); // movzx ecx, byte ptr [rdx + 0x10]
        a.Emit(0x89, 0x08);             // mov [rax], ecx

        // Clear payload fields:
        a.Emit(0xc7, 0x40, 0x04, 0, 0, 0, 0); // mov dword ptr [rax+4], 0
        a.Emit(0xc7, 0x40, 0x08, 0, 0, 0, 0); // mov dword ptr [rax+8], 0
        a.Emit(0xc7, 0x40, 0x0c, 0, 0, 0, 0); // mov dword ptr [rax+12], 0

        // Check if message has stream at [rdx + 0x20]:
        a.Emit(0x4c, 0x8b, 0x42, 0x20); // mov r8, [rdx + 0x20]
        a.Emit(0x4d, 0x85, 0xc0);       // test r8, r8
        a.J(0x84, "inc_count");

        // [stream + 0x10] is buffer
        a.Emit(0x4d, 0x8b, 0x40, 0x10); // mov r8, [r8 + 0x10]
        a.Emit(0x4d, 0x85, 0xc0);       // test r8, r8
        a.J(0x84, "inc_count");

        // [buffer + 0x18] is length (int)
        a.Emit(0x41, 0x8b, 0x48, 0x18); // mov ecx, [r8 + 0x18]
        a.Emit(0x89, 0x48, 0x04);       // mov [rax + 4], ecx
        a.Emit(0x85, 0xc9);             // test ecx, ecx
        a.J(0x84, "inc_count");

        // Read up to 8 payload bytes at [r8 + 0x20]:
        a.Emit(0x49, 0x8b, 0x48, 0x20); // mov rcx, [r8 + 0x20]
        a.Emit(0x48, 0x89, 0x48, 0x08); // mov [rax + 8], rcx

        a.Label("inc_count");
        a.Emit(0xf0, 0xff, 0x40, 0x20); // lock inc dword ptr [rax + 32]

        a.Label("skip");
        // Restore registers:
        a.Emit(0x9d, 0x41, 0x5b, 0x41, 0x5a, 0x41, 0x59, 0x41, 0x58, 0x5a, 0x59, 0x58);

        // Stolen 16 bytes of 0x4340:
        // 48 8B 02        mov rax, [rdx]
        // 4D 8B D0        mov r10, r8
        // 44 0F B7 C9     movzx r9d, cx
        // 4C 8B DA        mov r11, rdx
        // 4D 03 C9        add r9, r9
        a.Emit(0x48, 0x8B, 0x02, 0x4D, 0x8B, 0xD0, 0x44, 0x0F, 0xB7, 0xC9, 0x4C, 0x8B, 0xDA, 0x4D, 0x03, 0xC9);

        // Jump back to 0x4340 + 16:
        a.Emit(0xff, 0x25, 0, 0, 0, 0);
        a.Q(returnTarget);

        return a.Finish();
    }

    static void Main() {
        byte[] code = SendHook(0x12345678, 0x180004350L);
        Console.WriteLine("SendHook length: " + code.Length);
    }
}

