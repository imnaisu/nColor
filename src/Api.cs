using System;
using System.Runtime.InteropServices;

namespace nColor
{
    public static class api
    {
        const int STD_OUTPUT_HANDLE = -11;
        const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }

        private static bool enabled = false;

        private static void EnableVirtualTerminalProcessing()
        {
            if (enabled) return;

            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            if (GetConsoleMode(handle, out int mode))
            {
                SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
                enabled = true;
            }
            else
            {
                throw new InvalidOperationException("Failed to get console mode.");
            }
        }

        public static void WriteRgbLine(int r, int g, int b, string text)
        {
            if (misc.CheckIfWindows10OrHigher() != "Windows10Detected")
                throw new PlatformNotSupportedException("This method requires Windows 10 or higher.");

            EnableVirtualTerminalProcessing();

            string rgbEscape = $"\x1b[38;2;{r};{g};{b}m";
            string resetEscape = "\x1b[0m";

            Console.WriteLine($"{rgbEscape}{text}{resetEscape}");
        }

        public static class misc
        {
            public static string CheckIfWindows10OrHigher()
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return "Windows10NotDetected";

                OSVERSIONINFOEX versionInfo = new OSVERSIONINFOEX();
                versionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                int status = RtlGetVersion(ref versionInfo);
                if (status == 0) // STATUS_SUCCESS
                {
                    if (versionInfo.dwMajorVersion >= 10)
                        return "Windows10Detected";
                }

                return "Windows10NotDetected";
            }
        }
    }
}
