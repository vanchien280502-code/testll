using System;
using System.IO;
using System.Text;

public class FindMetaLoad {
    public static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        byte[] target = Encoding.ASCII.GetBytes("global-metadata.dat");
        for (int i = 0; i < bytes.Length - target.Length; i++) {
            bool found = true;
            for (int j = 0; j < target.Length; j++) {
                if (bytes[i + j] != target[j]) { found = false; break; }
            }
            if (found) {
                Console.WriteLine(string.Format("Found 'global-metadata.dat' at offset 0x{0:X}", i));
            }
        }
    }
}

