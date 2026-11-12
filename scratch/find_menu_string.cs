using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        string str = "Chọn loại vòng quay";
        byte[] target = Encoding.UTF8.GetBytes(str);

        for (int i = 0; i < meta.Length - target.Length; i++) {
            bool found = true;
            for (int j = 0; j < target.Length; j++) {
                if (meta[i + j] != target[j]) { found = false; break; }
            }
            if (found) {
                Console.WriteLine("Found '{0}' at metadata offset 0x{1:X}", str, i);
            }
        }
    }
}

