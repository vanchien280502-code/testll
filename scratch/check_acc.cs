using System;
using System.IO;
using System.Diagnostics;

class Program {
    static void Main() {
        // Let's find process by trying OpenProcess on PIDs from 1000 to 65000
        // Or check running accounts in QLTK
        string logFile = @"QLTK\logs\59d76774-6a7f-4bc7-9225-5d22758a3b19.jsonl";
        Console.WriteLine("Reading log file: " + logFile);
        // Let's check accounts.json to see the account id
        if (File.Exists(@"QLTK\accounts.json")) {
            string json = File.ReadAllText(@"QLTK\accounts.json");
            Console.WriteLine("accounts.json length: " + json.Length);
        }
    }
}
