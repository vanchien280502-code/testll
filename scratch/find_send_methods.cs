using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] meta = File.ReadAllBytes(@"TGLL_minCPU_v2\TheGioiLangLa_Data\il2cpp_data\Metadata\global-metadata.dat");
        int stringOffset = BitConverter.ToInt32(meta, 24);
        int stringSize = BitConverter.ToInt32(meta, 28);
        int methodsOffset = BitConverter.ToInt32(meta, 64);
        int methodsSize = BitConverter.ToInt32(meta, 68);

        Console.WriteLine("Searching for method names containing 'send' or 'Message' or 'Packet' or 'Click'...");
        string[] targets = new[] { "sendMessage", "send", "WriteByte", "performAction", "actionPerform", "click", "Click" };
        int end = stringOffset + stringSize;
        int curr = stringOffset;
        while (curr < end) {
            int len = 0;
            while (curr + len < end && meta[curr + len] != 0) len++;
            if (len > 0) {
                string s = Encoding.UTF8.GetString(meta, curr, len);
                foreach (var t in targets) {
                    if (s.Equals(t, StringComparison.OrdinalIgnoreCase)) {
                        Console.WriteLine("Found method name '{0}' at string offset 0x{1:X}", s, curr - stringOffset);
                    }
                }
            }
            curr += len + 1;
        }
    }
}

