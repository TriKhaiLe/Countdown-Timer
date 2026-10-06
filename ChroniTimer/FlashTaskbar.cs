using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChroniTimer
{
    /// <summary>
    /// Provides Win32 taskbar flashing to attract user attention.
    /// </summary>
    public static class FlashTaskbar
    {
        [DllImport("user32.dll")]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        private const uint FLASHW_ALL = 3;
        private const uint FLASHW_STOP = 0;

        public static void StartFlashing(Form form)
        {
            if (form == null || form.IsDisposed) return;
            FLASHWINFO fInfo = new FLASHWINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = form.Handle,
                dwFlags = FLASHW_ALL,
                uCount = uint.MaxValue,
                dwTimeout = 0
            };
            FlashWindowEx(ref fInfo);
        }

        public static void StopFlashing(Form form)
        {
            if (form == null || form.IsDisposed) return;
            FLASHWINFO fInfo = new FLASHWINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = form.Handle,
                dwFlags = FLASHW_STOP,
                uCount = 0,
                dwTimeout = 0
            };
            FlashWindowEx(ref fInfo);
            form.Activate();
        }
    }
}
