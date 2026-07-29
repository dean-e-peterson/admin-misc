using System.Runtime.InteropServices;

public static partial class SetWindowPos
{
    // BOOL GetWindowRect(
    //   [in]  HWND   hWnd,
    //   [out] LPRECT lpRect
    // );

    public struct Rect
    {
        int left;
        int top;
        int right;
        int bottom;
    }

    [LibraryImport("User32.dll", StringMarshalling = StringMarshalling.Utf16)]
    internal static partial IntPtr FindWindowW(string? lpClassName, string? lpWindowName);

    [LibraryImport("User32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

    public static void Hi()
    {
        Console.WriteLine("Hi");
        Console.WriteLine(FindWindowW(null, "Explorer"));
    }
}