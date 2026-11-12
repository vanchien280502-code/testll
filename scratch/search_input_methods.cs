using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        byte[] bytes = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Search for string "GetMouseButton" or "get_mousePosition" or "touchCount" in GameAssembly
        string[] methods = new[] { "GetMouseButton", "get_mousePosition", "get_touchCount", "GetTouch" };
        foreach (var m in methods) {
            byte[] pat = Encoding.ASCII.GetBytes(m);
            int count = 0;
            for (int i = 0; i < bytes.Length - pat.Length; i++) {
                bool found = true;
                for (int j = 0; j < pat.Length; j++) {
                    if (bytes[i + j] != pat[j]) { found = false; break; }
                }
                if (found) {
                    Console.WriteLine("Found \"{0}\" at offset 0x{1:X}", m, i);
                    count++;
                }
            }
        }
    }
}

