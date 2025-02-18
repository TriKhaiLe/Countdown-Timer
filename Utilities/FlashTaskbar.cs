using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
        private static System.Windows.Forms.Timer flashTimer;
        private static int[] flashPattern = { 700, 500, 300, 200, 300, 500, 700 };
        private static int flashIndex = 0;
        private static Form targetForm;
        private static bool isFlashing = false;

        public static void StartFlashing(Form form)
        {
            targetForm = form;
            if (flashTimer == null)
            {
                flashTimer = new System.Windows.Forms.Timer();
                flashTimer.Tick += new EventHandler(FlashStep);
            }
            flashIndex = 0;
            flashTimer.Interval = flashPattern[flashIndex]; // Lấy thời gian đầu tiên
            flashTimer.Start();
        }

        private static void FlashStep(object sender, EventArgs e)
        {
            if (targetForm == null) return;

            FLASHWINFO fInfo = new FLASHWINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = targetForm.Handle,
                dwFlags = isFlashing ? 0 : FLASHW_ALL, // Nhấp nháy ON/OFF
                uCount = 1,
                dwTimeout = 0
            };
            FlashWindowEx(ref fInfo);

            isFlashing = !isFlashing; // Đảo trạng thái (Bật ↔ Tắt)

            flashIndex = (flashIndex + 1) % flashPattern.Length; // Chuyển sang nhịp tiếp theo
            flashTimer.Interval = flashPattern[flashIndex]; // Cập nhật thời gian
        }

        public static void StopFlashing()
        {
            if (flashTimer != null)
            {
                flashTimer.Stop();
            }
            isFlashing = false;
        }
    }
}
