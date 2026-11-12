using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int magic = BitConverter.ToInt32(meta, 0);
        int version = BitConverter.ToInt32(meta, 4);
        Console.WriteLine("Magic: 0x" + magic.ToString("X") + ", Version: " + version);
        
        int stringOffset = BitConverter.ToInt32(meta, 24);
        int stringSize = BitConverter.ToInt32(meta, 28);
        int methodsOffset = BitConverter.ToInt32(meta, 64);
        int methodsSize = BitConverter.ToInt32(meta, 68);
        
        Console.WriteLine("StringOffset: 0x" + stringOffset.ToString("X") + " Size: " + stringSize);
        Console.WriteLine("MethodsOffset: 0x" + methodsOffset.ToString("X") + " Size: " + methodsSize);
    }
}

