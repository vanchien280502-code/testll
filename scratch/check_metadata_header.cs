using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int magic = BitConverter.ToInt32(meta, 0);
        int version = BitConverter.ToInt32(meta, 4);
        Console.WriteLine("Magic: 0x{0:X}, Version: {1}", magic, version);

        // Print first 20 header entries (offset, size)
        for (int i = 0; i < 25; i++) {
            int off = BitConverter.ToInt32(meta, 8 + i * 8);
            int sz = BitConverter.ToInt32(meta, 12 + i * 8);
            Console.WriteLine("Entry {0}: offset 0x{1:X}, size {2}", i, off, sz);
        }
    }
}

