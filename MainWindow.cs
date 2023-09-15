using System;
using System.Windows.Forms;
using System.IO;
using Tulpep.NotificationWindow;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        // milisecond
        int _period = 0;
        char _option = '0'; // 0 la "Nhac lai"
        char _alertLatch = '0'; // bien latch de su kien ko dien ra nhieu lan
        char _soundLatch = '0';
        double _pomo = 0;
        string[] _todayData;
        MediaPlayer _mediaPlayer = new MediaPlayer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 204, 0);
            radioButton1.Checked = true;
            timeUnit_box.Text = "minute";
            period_box.Text = "25";
            reset_btn.Enabled = false;

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = projectDirectory + "\\Timer\\Resources\\countdown-sound.wav";

            _mediaPlayer.Open(new Uri(path));
            _mediaPlayer.Volume = 0.1;

            string today = DateTime.Now.ToString("dd/MM/yyyy");

            // doc du lieu neu file ton tai
            if (File.Exists("today.txt"))
                _todayData = File.ReadAllLines("today.txt");

            // khoi tao du lieu neu ko ton tai
            else
            {
                _todayData = new string[] { today, "0" };
                File.WriteAllLines("today.txt", _todayData);
            }

            // neu da qua ngay moi
            if (_todayData[0] != today)
            {
                File.AppendAllLines("history.txt", _todayData);

                _todayData = new string[] { today, "0" };
                File.WriteAllLines("today.txt", _todayData);
            }

            // lay so pomo
            if (!double.TryParse(_todayData[1], out _pomo))
                MessageBox.Show("Data pomo bị lỗi!");

            date_lb.Text = $"Hôm nay {_todayData[0]}, đã học {_todayData[1]} pomo(es)";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_period <= 0)
            {
                _mediaPlayer.Stop();
                _timer.Stop();
                _pauseTimer.Start();

                _todayData[1] = _pomo.ToString("F1");
                date_lb.Text = $"Hôm nay {_todayData[0]}, đã học {_todayData[1]} pomo(es)";
                File.WriteAllLines("today.txt", _todayData);

                // reset noi dung form
                lb_periodBox.Text = "Thời gian hẹn giờ:";
                lb_periodBox.ForeColor = Color.Black;
                lb_periodBox.BackColor = default;

                period_box.Text = "25";
                timeUnit_box.Text = "minute";
                radioButton1.Checked = true;

                start_btn.Enabled = true;
                postpone_btn.Enabled = true;
                plus_btn.Enabled = true;
                subtract_btn.Enabled = true;

                // reset fields
                _period = 0;
                _alertLatch = '0';
                _soundLatch = '0';

                if (_option == '0')
                {
                    this.WindowState = FormWindowState.Normal;
                    reset_btn.Enabled = false;
                }
                else
                {
                    _option = '0';
                    Application.SetSuspendState(PowerState.Suspend, true, true);
                    //MessageBox.Show("Boom");
                }

                return;
            }

            // phat am thanh dem nguoc khi con 10 giay
            if (_period <= 10000 && _soundLatch == '0')
            {
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
                if (radioButton2.Checked == true)
                {
                    radioButton1.Checked = true;

                    Application.SetSuspendState(PowerState.Hibernate, true, true);
                    //MessageBox.Show("Boom");
                    return;
                }

                // lay button
                Button btn = (Button)sender;
                if (btn == null)
                    return;

                if (btn == postpone_btn)
                {
                    _option = '0';
                    reset_btn.Enabled = true;
                }
                else
                {
                    _option = '1';

                }
                // lay interval
                _period = GetPeriod();
                if (_period == -1)
                    return;

                _pomo += _period / 1000.0 / 60.0 / 25.0;

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
                _pauseTimer.Stop();

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
            _timer.Stop();
            _pauseTimer.Start();
            if (MessageBox.Show("Are you sure you want to Exit?",
                                "Hey!",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information) == DialogResult.No
                                || _option == '1') // khong thoat duoc khi "Bat dau"
            {
                e.Cancel = true;

                if (_period > 0)
                {
                    _timer.Start();
                    _pauseTimer.Stop();
                }
                return;
            }

            // neu thoat, tinh lai so pomo da dung
            _pomo -= _period / 1000.0 / 60.0 / 25.0;
            _todayData[1] = _pomo.ToString("F1");
            File.WriteAllLines("today.txt", _todayData);
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
                btn.BackColor = Color.Red;
        }

        private void ChangeColor_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == null)
                return;

            btn.BackColor = Color.White;
        }

        private void reset_btn_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            _pauseTimer.Start();

            if (MessageBox.Show("Are you sure you want to Reset?",
                        "Hey!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
            {
                _timer.Start();
                _pauseTimer.Stop();
                return;
            }

            // tinh lai so pomo da dung
            _pomo -= _period / 1000.0 / 60.0 / 25.0;
            _todayData[1] = _pomo.ToString("F1");
            File.WriteAllLines("today.txt", _todayData);

            // reset noi dung form
            date_lb.Text = $"Hôm nay {_todayData[0]}, đã học {_todayData[1]} pomo(es)";
            lb_periodBox.Text = "Thời gian hẹn giờ:";
            lb_periodBox.ForeColor = Color.Black;
            lb_periodBox.BackColor = default;
            period_box.Text = "25";
            timeUnit_box.Text = "minute";
            radioButton1.Checked = true;

            // reset button
            start_btn.Enabled = true;
            postpone_btn.Enabled = true;
            plus_btn.Enabled = true;
            subtract_btn.Enabled = true;
            reset_btn.Enabled = false;

            // reset fields
            _period = 0;
            _option = '0';
            _alertLatch = '0';
            _soundLatch = '0';
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            // add duration in minute from outer_time_box to pomo
            try
            {
                double duration = Convert.ToDouble(outer_time_box.Text);
                _pomo += duration / 25.0;
                _todayData[1] = _pomo.ToString("F1");
                File.WriteAllLines("today.txt", _todayData);

                date_lb.Text = $"Hôm nay {_todayData[0]}, đã học {_todayData[1]} pomo(es)";
                outer_time_box.Text = "";
            }
            catch
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin");
            }

        }

        // verify input just receiving positive integer
        private void period_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void _pauseTimer_Tick(object sender, EventArgs e)
        {
            // Minimize ứng dụng
            this.WindowState = FormWindowState.Minimized;

            // Hiển thị lại ứng dụng từ trạng thái minimize
            this.WindowState = FormWindowState.Normal;

        }
    }
}
