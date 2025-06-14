using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChroniClock.Utilities
{
    public class FlashTaskbar
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

        // Remove custom timer and pattern logic

        public static void StartFlashing(Form form)
        {
            if (form == null) return;
            FLASHWINFO fInfo = new FLASHWINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = form.Handle,
                dwFlags = FLASHW_ALL, // Use default Windows flashing
                uCount = uint.MaxValue, // Flash until window comes to foreground
                dwTimeout = 0
            };
            FlashWindowEx(ref fInfo);
        }

        public static void StopFlashing()
        {
            // No need to keep state or timer, just stop flashing on the last target form if needed
            // This method is kept for compatibility, but you may want to pass the form as a parameter
            // For now, do nothing
        }

        public static void StopFlashing(Form form)
        {
            if (form == null) return;
            FLASHWINFO fInfo = new FLASHWINFO
            {
                cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = form.Handle,
                dwFlags = FLASHW_STOP, // Stop flashing
                uCount = 0,
                dwTimeout = 0
            };
            FlashWindowEx(ref fInfo);
            form.Activate();
        }
    }
}
