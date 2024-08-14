using System;
using System.Windows.Forms;
using System.IO;
using Tulpep.NotificationWindow;
using Color = System.Drawing.Color;
using Timer;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        public PomodoroTimer _pomodoroTimer;
        public MediaPlayer _mediaPlayer;

        public Form1()
        {
            InitializeComponent();
            _pomodoroTimer = new PomodoroTimer(this);
            _mediaPlayer = new MediaPlayer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UIHelper.InitializeUI(this);
            _mediaPlayer.InitializeMediaPlayer();
            _pomodoroTimer.LoadTodayData();
            _pomodoroTimer.UpdateUI();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _pomodoroTimer.HandleTimerTick();
        }

        private void start_Click(object sender, EventArgs e)
        {
            _pomodoroTimer.StartPomodoro(sender);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pomodoroTimer.HandleFormClosing(e);
        }

        public ToolTip _toolTip = new ToolTip();

        private void reset_btn_Click(object sender, EventArgs e)
        {
            _pomodoroTimer.ResetPomodoro();
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            _pomodoroTimer.AddExternalPomodoroTime();
        }

        private void _pauseTimer_Tick(object sender, EventArgs e)
        {
            // Minimize ứng dụng
            this.WindowState = FormWindowState.Minimized;

            // Hiển thị lại ứng dụng từ trạng thái minimize
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

        // verify input just receiving positive integer
        private void period_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

    }
}
