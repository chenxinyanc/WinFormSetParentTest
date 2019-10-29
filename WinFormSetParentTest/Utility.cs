using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace WinFormSetParentTest
{

    public enum DPI_AWARENESS
    {
        DPI_AWARENESS_INVALID = -1,
        DPI_AWARENESS_UNAWARE = 0,
        DPI_AWARENESS_SYSTEM_AWARE = 1,
        DPI_AWARENESS_PER_MONITOR_AWARE = 2
    }

    public static class Utility
    {

        [DllImport("user32", SetLastError = true)]
        public static extern DPI_AWARENESS GetAwarenessFromDpiAwarenessContext(IntPtr value);

        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr SetThreadDpiAwarenessContext(IntPtr value);

        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);

        public static string TextFromHandle(IntPtr hwnd)
        {
            var sb = new StringBuilder(255);
            if (GetWindowText(hwnd, sb, sb.Capacity + 1) == 0 && Marshal.GetLastWin32Error() != 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return sb.ToString();
        }

    }

}
