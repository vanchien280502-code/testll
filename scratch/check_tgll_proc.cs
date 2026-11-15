using System;
using System.Diagnostics;

class Program {
    static void Main() {
        var procs = Process.GetProcessesByName("TheGioiLangLa");
        Console.WriteLine("TheGioiLangLa count: " + procs.Length);
        foreach (var p in procs) {
            Console.WriteLine("PID: " + p.Id);
        }
    }
}
