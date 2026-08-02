param(
    [Parameter(Mandatory=$True)]
    [string]$folder
)

$Source = @"
using System;
using System.Runtime.InteropServices;
namespace Native
{
    public static class User32
    {
        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
        private static readonly IntPtr HWND_BOTTOM = (IntPtr)(1);
        private static readonly IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
        private static readonly IntPtr HWND_TOP = (IntPtr)(0);
        private static readonly IntPtr HWND_TOPMOST = (IntPtr)(-1);
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_ASYNCWINDOWPOS = 0x4000;

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int  X,
            int  Y,
            int  cx,
            int  cy,
            uint uFlags
        );

        public static void SetWindowPos(IntPtr hWnd, int X, int Y, int cx, int cy)
        {
            // Ignored if SWP_NOZORDER is in uFlags.
            IntPtr hWndInsertAfter = HWND_TOP;
            uint uFlags = SWP_NOZORDER | SWP_NOACTIVATE;
            if (!SetWindowPos(hWnd, hWndInsertAfter, X, Y, cx, cy, uFlags))
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new Exception("Error " + lastError + " calling SetWindowPos().");
            }
        }
    }
}
"@

Add-Type -TypeDefinition $Source

# Borrowed from https://github.com/microsoft/PowerToys/blob/d2c53bf3861ed2688a1c30aafd66ea0fc0186399/.github/skills/powertoys-verification/scripts/pt-explorer-com.ps1#L14
function Get-PtExplorerWindows {
    <#
    .SYNOPSIS
    Return all open Explorer windows as Shell COM objects (with .LocationName, .Document.Folder, etc.).
    Returns @() if no Explorer windows are open.
    #>
    try {
        $shell = New-Object -ComObject Shell.Application
        return @($shell.Windows() | Where-Object { $_.Name -eq 'File Explorer' -or $_.FullName -match 'explorer\.exe$' })
    } catch { return @() }
}

$localUri = (New-Object System.Uri $folder).AbsoluteUri
# Write-Host $localUri

$windows = Get-PtExplorerWindows
# Write-Output $windows.Count
foreach ($window in $windows) {
    # Write-Output "-----------------"
    # Write-Output $window | Format-List
    if ($window.LocationURL -eq $localUri)
    {
        [Native.User32]::SetWindowPos($window.HWND, 30, 14, 1000, 500)
    }
}
