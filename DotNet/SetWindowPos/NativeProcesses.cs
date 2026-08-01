using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

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
        // public const int TH32CS_SNAPTHREAD = 0x00000004;
        public const long INVALID_HANDLE_VALUE = -1;

        // Functions accepting structures like PROCESSENTRY32 either require an unsafe fixed char[]
        // in the PROCESSENTRY32 declaration so the LibraryImport attribute AOT marshaller can deal
        // with it, or the functions using PROCESSENTRY32 must use the DllImport attribute and the
        // runtime marshaller.  Otherwise you get a build error:
        // error SYSLIB1051: The type 'Native.NativeProcesses.PROCESSENTRY32' is not supported by
        // source-generated P/Invokes. The generated source will not handle marshalling of parameter 'lppe'.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PROCESSENTRY32W
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAX_PATH)]
            public string szExeFile;

            // Factory suggested by DeepSeek
            public static PROCESSENTRY32W Create()
            {
                return new PROCESSENTRY32W() {
                    dwSize = (uint)Marshal.SizeOf<PROCESSENTRY32W>(),
                    szExeFile = String.Empty, // Avoid declaring it nullable.
                };
            }
        }

        // [StructLayout(LayoutKind.Sequential)]
        // public struct THREADENTRY32 {
        //     uint dwSize;
        //     uint cntUsage;
        //     uint th32ThreadID;
        //     uint th32OwnerProcessID;
        //     int tpBasePri;
        //     int tpDeltaPri;
        //     uint dwFlags;
        //
        //     public static THREADENTRY32 Create()
        //     {
        //         return new THREADENTRY32
        //         {
        //             dwSize = (uint)Marshal.SizeOf<THREADENTRY32>();
        //
        //         };
        //     }
        // }

        public const int MAX_PATH = 260;
        
        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32FirstW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32NextW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        // https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499-
        public const int ERROR_NO_MORE_FILES = 0x12;

        public static void TestListProcesses()
        {
            int lastError;

            IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (hSnapshot == INVALID_HANDLE_VALUE)
            {
                lastError = Marshal.GetLastPInvokeError();
                throw new Exception($"Error {lastError} calling CreateToolhelp32Snapshot(): " + Marshal.GetPInvokeErrorMessage(lastError));
            }

            try
            {
                PROCESSENTRY32W lppe = PROCESSENTRY32W.Create();

                if (!Process32FirstW(hSnapshot, ref lppe))
                {
                    lastError = Marshal.GetLastPInvokeError();
                    if (lastError == ERROR_NO_MORE_FILES)
                    {
                        Console.WriteLine("No processes at all?  Odd.");
                    }
                    else
                    {
                        throw new Exception($"Error {lastError} calling Process32FirstW(): " + Marshal.GetPInvokeErrorMessage(lastError));
                    }
                }

                do
                {
                    ProcessProcessEntry(lppe);
                }
                while (Process32NextW(hSnapshot, ref lppe));

                lastError = Marshal.GetLastPInvokeError();
                if (lastError == ERROR_NO_MORE_FILES)
                {
                    Console.WriteLine("No more processes.  Cool.");
                }
                else
                {
                    throw new Exception($"Error {lastError} calling Process32NextW(): " + Marshal.GetPInvokeErrorMessage(lastError));
                }

            }
            finally
            {
                if (!CloseHandle(hSnapshot))
                {
                    throw new Exception("Exception calling CloseHandle(): " + Marshal.GetPInvokeErrorMessage(Marshal.GetLastPInvokeError()));
                }
            }
        }

        private static void ProcessProcessEntry(PROCESSENTRY32W entry)
        {
            Console.WriteLine($"Process id [{entry.th32ProcessID}], name {entry.szExeFile}");
        }
    }
}