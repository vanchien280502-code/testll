using System;
using System.IO;

class Program {
    static void Main() {
        string[] files = new string[] { "AutoAllUi.ps1", "ConfigUi.ps1", "GameSettingsUi.ps1" };
        foreach (var f in files) {
            string p = Path.Combine("QLTK", f);
            if (!File.Exists(p)) continue;
            string text = File.ReadAllText(p);
            Console.WriteLine("=== " + f + " ===");
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < Math.Min(10, lines.Length); i++) {
                Console.WriteLine("  " + lines[i]);
            }
        }
    }
}
