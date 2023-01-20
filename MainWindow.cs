using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Tulpep.NotificationWindow;
using Timer;
using System.Media;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        private int period = 0;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            timeUnit_box.Text = "minute";
            period_box.Text = "25";
        }

        private void timer_alert(object sender, EventArgs e)
        {
            shutdown_tm.Stop();
            shutdown_tm.Tick -= timer_alert;

            PopupAlert();

            shutdown_tm.Interval = 300000;
            shutdown_tm.Tick += timer_shutdown;
            shutdown_tm.Start();
        }

        private void timer_shutdown(object sender, EventArgs e)
        {
            shutdown_tm.Stop();
            shutdown_tm.Tick -= timer_shutdown;
            label1.Text = "Thời gian hẹn giờ:";
            period_box.Text = "25";

            Application.SetSuspendState(PowerState.Suspend, true, true);

        }

        private void PopupAlert()
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Timer.Properties.Resources.clock;
            popup.TitleText = "Hurry!";
            popup.Popup();
        }

        private void start_Click(object sender, EventArgs e)
        {
            try
            {
                period = GetPeriod();

                if (period <= 300000)
                {
                    PopupAlert();

                    shutdown_tm.Interval = period;
                    shutdown_tm.Tick += timer_shutdown;
                    shutdown_tm.Start();
                }
                else
                {
                    shutdown_tm.Interval = period - 300000;
                    shutdown_tm.Tick += timer_alert;
                    shutdown_tm.Start();
                }

                AnnounceWindow announceWindow = new AnnounceWindow();
                announceWindow.ShowDialog();
                this.WindowState = FormWindowState.Minimized;
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }
        }

        private int GetPeriod()
        {
            int period = Convert.ToInt32(this.period_box.Text);
            switch (timeUnit_box.Text)
            {
                case "second":
                    {
                        period *= 1000;
                        break;
                    }

                case "minute":
                    {
                        period *= 60 * 1000;
                        break;
                    }

                case "hour":
                    {
                        period *= 60 * 60 * 1000;
                        break;
                    }
            }

            return period;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Exit?",
                        "Hey!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void postpone_btn_Click(object sender, EventArgs e)
        {
            try
            {
                shutdown_tm.Interval = GetPeriod();
                shutdown_tm.Tick += timer_postpone;
                shutdown_tm.Start();

                postpone_btn.Enabled = false;
                this.WindowState = FormWindowState.Minimized;
            }
            catch
            {

            }
        }

        private void timer_postpone(object sender, EventArgs e)
        {
            shutdown_tm.Stop();
            shutdown_tm.Tick -= timer_postpone;

            postpone_btn.Enabled = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void plus_btn_Click(object sender, EventArgs e)
        {
            try
            {
                int period = Convert.ToInt32(this.period_box.Text);
                period += 5;
                period_box.Text = period.ToString();
            }
            catch
            {

            }

        }

        private void subtract_btn_Click(object sender, EventArgs e)
        {
            try
            {
                int period = Convert.ToInt32(this.period_box.Text);
                if (period - 5 > 0)
                {
                    period -= 5;
                    period_box.Text = period.ToString();
                }

            }
            catch
            {

            }

        }
    }
}
