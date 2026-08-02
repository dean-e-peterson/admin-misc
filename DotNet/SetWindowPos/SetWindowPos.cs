using System.Runtime.InteropServices;
namespace Native
{
    public static partial class User32
    {
        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
        private const IntPtr HWND_BOTTOM = 1;
        private const IntPtr HWND_NOTOPMOST = -2;
        private const IntPtr HWND_TOP = 0;
        private const IntPtr HWND_TOPMOST = -1;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_ASYNCWINDOWPOS = 0x4000;

        [LibraryImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetWindowPos(
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
                int lastError = Marshal.GetLastPInvokeError();
                throw new Exception("Error " + lastError + " calling SetWindowPos(): " + Marshal.GetPInvokeErrorMessage(lastError));
            }
        }
    }
}

