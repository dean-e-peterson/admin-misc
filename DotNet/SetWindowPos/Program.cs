using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
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
                    bool result = NativeWindowPos.EnumThreadWindows((uint)thread.Id, EnumThreadWndProc, IntPtr.Zero);
                    // Console.WriteLine($"EnumThreadWindows returned {result}");
                }
            }
        }
    }

    static bool EnumThreadWndProc(IntPtr hwnd, IntPtr lParam)
    {
        string windowText = NativeWindowPos.GetWindowTextW(hwnd);
        string windowClass = NativeWindowPos.GetClassNameW(hwnd);
        Console.WriteLine($"    Window [hwnd: {hwnd}], lParam {lParam}, class '{windowClass}', text '{windowText}'");
        return true;
    }
}
