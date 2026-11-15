using System;
using System.IO;

class Program {
    static void Main() {
        var info = new FileInfo(@"QLTK\logs\59d76774-6a7f-4bc7-9225-5d22758a3b19.jsonl");
        Console.WriteLine("LastWriteTimeUtc: " + info.LastWriteTimeUtc);
        Console.WriteLine("Length: " + info.Length);
    }
}
