using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Native
{
    public static partial class NativeWindowPos
    {
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

        // [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool EnumThreadWndProc(IntPtr hwnd, IntPtr lParam);

        public static bool Test_EnumThreadWndProc(IntPtr hwnd, IntPtr lParam)
        {
            Console.WriteLine($"{hwnd}: {lParam}");
            return true;
        }

        public static void Hi()
        {
            Console.WriteLine("Hi");
            Console.WriteLine(FindWindowW(null, "Explorer"));
        }
    }
}