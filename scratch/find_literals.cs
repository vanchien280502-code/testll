using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int strLitOffset = BitConverter.ToInt32(meta, 0x08);
        int strLitSize = BitConverter.ToInt32(meta, 0x0C);
        int strLitDataOffset = BitConverter.ToInt32(meta, 0x10);

        int count = strLitSize / 8;
        string[] targets = new string[] {
            "Vòng Quay", "Chọn loại", "Thử vận may", "Chơi lại", "Vận may", "Vé vận may", "vòng quay"
        };

        for (int i = 0; i < count; i++) {
            int len = BitConverter.ToInt32(meta, strLitOffset + i * 8);
            int dataOff = BitConverter.ToInt32(meta, strLitOffset + i * 8 + 4);
            if (len > 0 && dataOff >= 0 && (strLitDataOffset + dataOff + len) <= meta.Length) {
                string s = Encoding.UTF8.GetString(meta, strLitDataOffset + dataOff, len);
                foreach (var t in targets) {
                    if (s.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0) {
                        Console.WriteLine("Literal [{0}]: {1}", i, s);
                        break;
                    }
                }
            }
        }
    }
}

