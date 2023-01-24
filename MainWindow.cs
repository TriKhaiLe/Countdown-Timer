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
        private int _period = 0;
        private char _option = '0';
        private char _alertLatch = '0';
        private char _soundLatch = '0';
        MediaPlayer _mediaPlayer = new MediaPlayer();

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

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_period <= 0)
            {
                _mediaPlayer.Stop();
                _timer.Stop();

                // reset noi dung cua so
                lb_periodBox.Text = "Thời gian hẹn giờ:";
                lb_periodBox.ForeColor = Color.Black;
                lb_periodBox.BackColor = default;

                period_box.Text = "25";
                timeUnit_box.Text = "minute";

                start_btn.Enabled = true;
                postpone_btn.Enabled = true;
                plus_btn.Enabled = true;
                subtract_btn.Enabled = true;

                // reset fields
                _alertLatch = '0';
                _soundLatch = '0';

                if (_option == '0')
                    this.WindowState = FormWindowState.Normal;
                else
                    Application.SetSuspendState(PowerState.Suspend, true, true);
                //MessageBox.Show("Boom");

                return;
            }

            // phat am thanh dem nguoc khi con 10 giay
            if (_period <= 10000 && _soundLatch == '0')
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string path = projectDirectory + "\\Timer\\Resources\\countdown-sound.wav";

                _mediaPlayer.Open(new Uri(path));
                _mediaPlayer.Volume = 0.1;
                _mediaPlayer.Play();
                _soundLatch = '1';
            }

            // thong bao khi con 5p
            if (_period <= 300000 && _alertLatch == '0')
            {
                PopupAlert();
                _alertLatch = '1';
            }

            period_box.Text = TimeSpan.FromSeconds(_period / 1000).ToString();

            _period -= 1000;
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
                // lay button
                Button btn = (Button)sender;
                if (btn == null)
                    return;

                if (btn == postpone_btn)
                    _option = '0';
                else
                    _option = '1';

                // lay interval
                _period = GetPeriod();
                if (_period == -1)
                    return;

                _timer.Interval = _period;

                // vo hieu hoa 4 nut
                start_btn.Enabled = false;
                postpone_btn.Enabled = false;
                plus_btn.Enabled = false;
                subtract_btn.Enabled = false;

                // hien thi "Thoi gian con lai"
                lb_periodBox.Text = "Thời gian còn lại:";
                lb_periodBox.ForeColor = Color.Red;
                lb_periodBox.BackColor = Color.White;

                // setup timer
                _timer.Interval = 1000;
                _timer.Start();

                this.WindowState = FormWindowState.Minimized;
            }
            catch
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin");
            }
        }


        private int GetPeriod()
        {
            try
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

                    default:
                        {
                            MessageBox.Show("Vui lòng kiểm tra lại thông tin");
                            return -1;
                        }
                }
                return period;
            }
            catch
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin");
                return -1;
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

        private void plus_btn_Click(object sender, EventArgs e)
        {
            try
            {
                int period = Convert.ToInt32(this.period_box.Text);
                period += 5;
                period_box.Text = period.ToString();
            }
            catch { }
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
