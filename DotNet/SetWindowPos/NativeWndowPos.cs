using System.Runtime.InteropServices;
using System.Text;

namespace Native
{
    public static partial class NativeWindowPos
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
        public static partial bool SetWindowPos(
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
                throw new Exception($"Error {lastError} calling SetWindowPos(): " + Marshal.GetPInvokeErrorMessage(lastError));
            }
        }

        // https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499-
        private const uint ERROR_SUCCESS = 0x0; // The operation completed successfully.
        private const int MAX_CLASS_NAME = 256; // Per AI, so take with a grain of salt.

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [LibraryImport("User32.dll", StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr FindWindowW(string? lpClassName, string? lpWindowName);

        [LibraryImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [LibraryImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool EnumThreadWindows(uint dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);

        public delegate bool EnumThreadWndProc(IntPtr hwnd, IntPtr lParam);

        // public static bool Test_EnumThreadWndProc(IntPtr hwnd, IntPtr lParam)
        // {
        //     Console.WriteLine($"{hwnd}: {lParam}");
        //     return true;
        // }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextW(IntPtr hwnd, StringBuilder lpString /*out string lpString*/, int nMaxCount);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int GetWindowTextLengthW(IntPtr hwnd);

        public static string GetWindowTextW(IntPtr hwnd)
        {
            int textLength = GetWindowTextLengthW(hwnd);
            var sbWindowText = new StringBuilder(textLength + 1);
            if (GetWindowTextW(hwnd, sbWindowText, sbWindowText.Capacity) == 0)
            {
                int lastError = Marshal.GetLastPInvokeError();
                if (lastError != NativeWindowPos.ERROR_SUCCESS)
                {
                    throw new Exception("Error {lastError} calling GetWindowTextW(): " + Marshal.GetPInvokeErrorMessage(lastError));
                }
            }
            return sbWindowText.ToString();
        }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetClassNameW(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

        public static string GetClassNameW(IntPtr hwnd)
        {
            var sbClassName = new StringBuilder(MAX_CLASS_NAME);
            if (GetWindowTextW(hwnd, sbClassName, sbClassName.Capacity) == 0)
            {
                int lastError = Marshal.GetLastPInvokeError();
                if (lastError != NativeWindowPos.ERROR_SUCCESS)
                {
                    throw new Exception("Error {lastError} calling GetClassNameW(): " + Marshal.GetPInvokeErrorMessage(lastError));
                }
            }
            return sbClassName.ToString();
        }      

        public static void Hi()
        {
            Console.WriteLine("Hi");
            Console.WriteLine(FindWindowW(null, "Explorer"));
        }
    }
}