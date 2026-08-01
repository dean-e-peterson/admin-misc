// using Native;
using System.Diagnostics;
// using System.Collections.Generic;

// Console.WriteLine("Hello, World!");
// NativeWindowPos.Hi();
// NativeProcesses.TestListProcesses();

foreach (Process process in Process.GetProcessesByName("Explorer"))
{
    Console.WriteLine($"[{process.Id}] {process?.MainModule?.ModuleName}");
    if (process == null)
    {
        throw new Exception("Huh?");
    }
    foreach (ProcessThread thread in process.Threads)
    {
        using (thread)
        {
            Console.WriteLine($"  ({thread.Id})");
        }
    }
}