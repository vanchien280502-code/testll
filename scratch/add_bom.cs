using System;
using System.IO;
using System.Text;

class Program {
    static void Main() {
        foreach (string path in new string[] { @"QLTK\NativeBackend.cs", @"QLTK\LuckyWheelUi.ps1", @"QLTK\LuckyWheelTests.cs", @"QLTK\LuckyWheelUiTests.ps1" }) {
            if (File.Exists(path)) {
                string content = File.ReadAllText(path, Encoding.UTF8);
                File.WriteAllText(path, content, new UTF8Encoding(true));
                Console.WriteLine("Done BOM: " + path);
            }
        }
    }
}
