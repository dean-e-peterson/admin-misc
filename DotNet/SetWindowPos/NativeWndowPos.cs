using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Native
{
    public static partial class NativeWindowPos
    {
        // https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499-
        public const uint ERROR_SUCCESS = 0x0; // The operation completed successfully.
        public const int MAX_CLASS_NAME = 256; // Per AI, so take with a grain of salt.

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