using System.Drawing;

namespace Scheduler
{
    internal class UIHelper
    {
        public static void InitializeUI(Form1 form)
        {
            form.BackColor = Color.FromArgb(255, 204, 0);
            form.period_box.Text = "45";
            form.reset_btn.Enabled = false;
        }

    }
}