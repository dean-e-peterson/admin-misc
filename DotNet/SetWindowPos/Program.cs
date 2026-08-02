using Native;

namespace SetWindowPos;

class Program
{
    static void Main(string[] args)
    {
        // NativeWindowPos.Hi();
        // NativeProcesses.TestListProcesses();

        // foreach (Process process in Process.GetProcessesByName("Explorer"))
        // {
        //     Console.WriteLine($"Process [{process.Id}] {process?.MainModule?.ModuleName}");
        //     if (process == null)
        //     {
        //         throw new Exception("Huh?");
        //     }
        //     foreach (ProcessThread thread in process.Threads)
        //     {
        //         using (thread)
        //         {
        //             Console.WriteLine($"  Thread [{thread.Id}]");
        //             bool result = NativeWindowPos.EnumThreadWindows((uint)thread.Id, EnumThreadWndProc, IntPtr.Zero);
        //             // Console.WriteLine($"EnumThreadWindows returned {result}");
        //         }
        //     }
        // }

        // Takes a window handle and resizes the window.
        IntPtr hWnd = Int32.Parse(args[0]);
        int  X = Int32.Parse(args[1]);
        int  Y = Int32.Parse(args[2]);
        int  cx = Int32.Parse(args[3]);
        int  cy = Int32.Parse(args[4]);

        User32.SetWindowPos(hWnd, X, Y, cx, cy);

        // Can be used in conjunction with the following PowerShell function.
        // # Borrowed from https://github.com/microsoft/PowerToys/blob/d2c53bf3861ed2688a1c30aafd66ea0fc0186399/.github/skills/powertoys-verification/scripts/pt-explorer-com.ps1#L14
        // function Get-PtExplorerWindows {
        //     <#
        //     .SYNOPSIS
        //     Return all open Explorer windows as Shell COM objects (with .LocationName, .Document.Folder, etc.).
        //     Returns @() if no Explorer windows are open.
        //     #>
        //     try {
        //         $shell = New-Object -ComObject Shell.Application
        //         return @($shell.Windows() | Where-Object { $_.Name -eq 'File Explorer' -or $_.FullName -match 'explorer\.exe$' })
        //     } catch { return @() }
        // }
    }

    static bool EnumThreadWndProc(IntPtr hwnd, IntPtr lParam)
    {
        string windowText = NativeWindowPos.GetWindowTextW(hwnd);
        string windowClass = NativeWindowPos.GetClassNameW(hwnd);
        Console.WriteLine($"    Window [hwnd: {hwnd}], lParam {lParam}, class '{windowClass}', text '{windowText}'");
        return true;
    }
}
