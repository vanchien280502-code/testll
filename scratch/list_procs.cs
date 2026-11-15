using System;
using System.Diagnostics;

class Program {
    static void Main() {
        foreach (var p in Process.GetProcesses()) {
            try {
                if (p.ProcessName.ToLower().Contains("tgll") || p.ProcessName.ToLower().Contains("langla") || p.ProcessName.ToLower().Contains("game") || p.MainWindowTitle.Length > 0) {
                    Console.WriteLine("PID: {0}, Name: {1}, Title: {2}", p.Id, p.ProcessName, p.MainWindowTitle);
                }
            } catch {}
        }
    }
}
