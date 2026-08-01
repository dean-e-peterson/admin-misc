using System.Runtime.InteropServices;

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

        // ####
        // BOOL EnumThreadWindows(
        //     uint dwThreadId,
        //     WNDENUMPROC lpfn,
        //     short lParam
        // );

        public static void Hi()
        {
            Console.WriteLine("Hi");
            Console.WriteLine(FindWindowW(null, "Explorer"));
        }
    }
}