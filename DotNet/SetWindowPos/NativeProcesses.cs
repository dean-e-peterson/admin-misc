using System.Runtime.InteropServices;

namespace Native
{
    // https://learn.microsoft.com/en-us/windows/win32/toolhelp/taking-a-snapshot-and-viewing-processes
    public static partial class NativeProcesses
    {
        [LibraryImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool CloseHandle(IntPtr hObject);

        // Returns handle to snapshot that should be closed wth CloseHandle().
        [LibraryImport("Kernel32.dll", SetLastError = true)]
        public static partial IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);
        public const int TH32CS_SNAPPROCESS = 0x00000002;
        public const long INVALID_HANDLE_VALUE = -1;

        // error SYSLIB1051: The type 'Native.NativeProcesses.PROCESSENTRY32' is not supported by source-generated P/Invokes. The generated source will not handle marshalling of parameter 'lppe'. (https://learn.microsoft.com/dotnet/fundamentals/syslib-diagnostics/syslib1051)
        // [StructLayout(LayoutKind.Sequential)]
        // public struct PROCESSENTRY32
        // {
        //     public uint dwSize;
        //     public uint cntUsage;
        //     public uint th32ProcessID;
        //     public UIntPtr th32DefaultHeapID;
        //     public uint th32ModuleID;
        //     public uint cntThreads;
        //     public uint th32ParentProcessID;
        //     public int pcPriClassBase;
        //     public uint dwFlags;
        //     [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)] public string szExeFile;
        // }
        // public const int MAX_PATH = 260;
        //
        // [LibraryImport("Kernel32.dll", SetLastError = true)]
        // [return: MarshalAs(UnmanagedType.Bool)]
        // public static partial bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);
    }
}