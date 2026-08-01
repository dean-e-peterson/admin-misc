using System.Diagnostics;
using Native;

namespace SetWindowPos;

class Program
{
    static void Main(string[] args)
    {
        // NativeWindowPos.Hi();
        // NativeProcesses.TestListProcesses();

        foreach (Process process in Process.GetProcessesByName("Explorer"))
        {
            Console.WriteLine($"Process [{process.Id}] {process?.MainModule?.ModuleName}");
            if (process == null)
            {
                throw new Exception("Huh?");
            }
            foreach (ProcessThread thread in process.Threads)
            {
                using (thread)
                {
                    Console.WriteLine($"  Thread [{thread.Id}]");
                    NativeWindowPos.EnumThreadWindows((uint)thread.Id, EnumThreadWndProc, IntPtr.Zero);
                }
            }
        }
    }

    static bool EnumThreadWndProc(IntPtr hwnd, IntPtr lParam)
    {
        Console.WriteLine($"    Window [hwnd: {hwnd}], lParam {lParam})");
        return true;
    }
}
