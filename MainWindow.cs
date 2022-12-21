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

namespace Scheduler
{
    public partial class Form1 : Form
    {
        

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

            PopupNotifier popup = new PopupNotifier();
            popup.Image = Timer.Properties.Resources.clock;
            popup.TitleText = "Hurry!";
            popup.ContentText = "Dập máy sau 5 phút 0 giây";
            popup.Popup();

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

            string filename = string.Empty;
            string arguments = string.Empty;

            //if (radioButton1.Checked == true)
            //{
            //    filename = "shutdown.exe";
            //    arguments = "-s";
            //}

            //ProcessStartInfo startExecute = new ProcessStartInfo(filename, arguments);
            //Process.Start(startExecute);
            //MessageBox.Show("Sleep");

            Application.SetSuspendState(PowerState.Suspend, true, true);

        }

        int tick_time = 1000;
        private void start_Click(object sender, EventArgs e)
        {

            try
            {
                int period = Convert.ToInt32(this.period_box.Text);
                switch (timeUnit_box.Text)
                {
                    case "second":
                        {
                            tick_time = 1000;
                            period *= 1000;
                            
                            break;
                        }

                    case "minute":
                        {
                            tick_time = 60 * 1000;
                            period *= 60 * 1000;

                            break;
                        }

                    case "hour":
                        {
                            //period_box.Text = $"{--period}";
                            tick_time = 60 * 60 * 100;
                            period *= 60 * 60 * 1000;

                            break;
                        }
                }

                if (period <= 300000)
                {
                    PopupNotifier popup = new PopupNotifier();
                    popup.Image = Timer.Properties.Resources.clock;
                    popup.TitleText = "Hurry!";
                    popup.ContentText = $"Dập máy sau {period / 60000} phút {period / 1000} giây";
                    popup.Popup();

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

                remaining_tm.Interval = tick_time;
                remaining_tm.Start();
                label1.Text = "Thời gian còn lại:";
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }


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

        private void remaining_tm_Tick(object sender, EventArgs e)
        {
            //T1

        }
    }
}
