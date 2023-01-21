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
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        // milisecond
        private int period = 0;
        MediaPlayer mediaPlayer = new MediaPlayer();

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 204, 0); 
            timeUnit_box.Text = "minute";
            period_box.Text = "25";
        }

        private void timerAlert_Tick(object sender, EventArgs e)
        {
            shutdown_tm.Stop();
            shutdown_tm.Tick -= timerAlert_Tick;

            PopupAlert();

            shutdown_tm.Interval = 300000;
            shutdown_tm.Tick += timerShutdown_Tick;
            shutdown_tm.Start();
        }
        

        private void timerShutdown_Tick(object sender, EventArgs e)
        {
            shutdown_tm.Stop();
            shutdown_tm.Tick -= timerShutdown_Tick;

            AfterTick();

            Application.SetSuspendState(PowerState.Suspend, true, true);
            //MessageBox.Show("Boom");

        }

        private void timerPostpone_Tick(object sender, EventArgs e)
        {
            shutdown_tm.Stop();
            shutdown_tm.Tick -= timerPostpone_Tick;

            AfterTick();

            this.WindowState = FormWindowState.Normal;
        }

        private void AfterTick()
        {
            lb_periodBox.Text = "Thời gian hẹn giờ:";
            lb_periodBox.ForeColor = Color.Black;
            lb_periodBox.BackColor = default;

            period_box.Text = "25";
            timeUnit_box.Text = "minute";

            start_btn.Enabled = true;
            postpone_btn.Enabled = true; 
            plus_btn.Enabled = true;
            subtract_btn.Enabled = true;

        }

        private void display_tm_Tick(object sender, EventArgs e)
        {
            period -= 1000;

            if (period <= 10000)
            {
                // phat am thanh dem nguoc
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string path = projectDirectory + "\\Timer\\Resources\\countdown-sound.wav";
                mediaPlayer.Open(new Uri(path));
                mediaPlayer.Volume = 0.1;
                mediaPlayer.Play();
            }

            if (period <= 0)
            {
                mediaPlayer.Stop();
                display_tm.Stop();
                return;
            }

            period_box.Text = TimeSpan.FromSeconds(period / 1000).ToString();
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
                // lay interval
                period = GetPeriod();

                // them tick event
                if (period <= 300000)
                {
                    PopupAlert();

                    shutdown_tm.Interval = period;
                    shutdown_tm.Tick += timerShutdown_Tick;
                    shutdown_tm.Start();
                }
                else
                {
                    shutdown_tm.Interval = period - 300000;
                    shutdown_tm.Tick += timerAlert_Tick;
                    shutdown_tm.Start();
                }

                AfterClick();
                this.WindowState = FormWindowState.Minimized;
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }
        }


        private void postpone_btn_Click(object sender, EventArgs e)
        {
            try
            {
                // lay interval
                period = GetPeriod();
                shutdown_tm.Interval = period;

                // them tick event
                shutdown_tm.Tick += timerPostpone_Tick;
                shutdown_tm.Start();

                AfterClick();
                this.WindowState = FormWindowState.Minimized;
            }
            catch
            {

            }
        }

        private void AfterClick()
        {
            // vo hieu hoa 4 nut
            start_btn.Enabled = false;
            postpone_btn.Enabled = false;
            plus_btn.Enabled = false;
            subtract_btn.Enabled = false;

            // hien thi thoi gian con lai
            lb_periodBox.Text = "Thời gian còn lại:";
            lb_periodBox.ForeColor = Color.Red;
            lb_periodBox.BackColor = Color.White;
            display_tm.Interval = 1000;
            display_tm.Start();
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


        private void ChangeColor_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == null)
                return;

            if (btn.Text == "Nhắc lại")
                btn.BackColor = Color.LimeGreen;
            else
                btn.BackColor = Color.Orange;
        }

        private void ChangeColor_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == null)
                return;

            btn.BackColor = Color.White;
        }
    }
}
