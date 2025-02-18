using System;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using ChroniClock;
using ChroniClock.Utilities;

namespace ChroniTask
{
    public partial class MainWindow : Form
    {
        public PomodoroTimer _pomodoroTimer;
        public MediaPlayerService _mediaPlayer;
        private bool _isPanelExternalTimeCollapsed = false; 

        public MainWindow()
        {
            InitializeComponent();
            _pomodoroTimer = new PomodoroTimer(this);
            _mediaPlayer = new MediaPlayerService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UIHelper.InitializeUI(this);
            LoadAndInitializeDayData();
            panelExternalTime_DoubleClick(sender, e);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _pomodoroTimer.HandleTimerTick();
        }

        private void start_Click(object sender, EventArgs e)
        {
            _pomodoroTimer.StartPomodoro();
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
            FlashTaskbar.StartFlashing(this);
        }

        private void plus_btn_Click(object sender, EventArgs e)
        {
            try
            {
                int period = Convert.ToInt32(this.period_box.Text);
                period += 5;
                period_box.Text = period.ToString();
            }
            catch {
                MessageBox.Show("Something went wrong!");
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
                MessageBox.Show("Something went wrong!");
            }

        }

        private void ChangeColor_MouseHover(object sender, EventArgs e)
        {
            start_btn.BackColor = Color.LimeGreen;
        }


        private void ChangeColor_MouseLeave(object sender, EventArgs e)
        {
            start_btn.BackColor = Color.White;
        }

        // verify input just receiving positive integer
        private void period_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void checkBoxHibernate_MouseDown(object sender, MouseEventArgs e)
        {
            if (_timer.Enabled)
            {
                MessageBox.Show("Please stop timer before hibernating.");
                return;
            }

            if (checkBoxHibernate.Checked && 
                e.Button == MouseButtons.Right)
                _pomodoroTimer.HibernateSystem();

        }

        private void date_lb_DoubleClick(object sender, EventArgs e)
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            UIHelper.InitializeUI(this);
            if (_pomodoroTimer._todayData[0] != today)
                LoadAndInitializeDayData();
        }

        private void LoadAndInitializeDayData()
        {
            // Load today's data
            _pomodoroTimer.LoadTodayData();

            // Update the UI based on today's data
            _pomodoroTimer.UpdateUI();
        }

        private void date_lb_MouseHover(object sender, EventArgs e)
        {
            // show some signals to user
            _toolTip.Show("Double click to refresh", date_lb);
        }

        private void panelExternalTime_DoubleClick(object sender, EventArgs e)
        {
            if (_isPanelExternalTimeCollapsed)
            {
                this.Height += panelExternalTime.Height;
                panelExternalTime.Visible = true;
            }
            else
            {
                this.Height -= panelExternalTime.Height;
                panelExternalTime.Visible = false;
            }
            _isPanelExternalTimeCollapsed = !_isPanelExternalTimeCollapsed;

        }

        private void lbRecentAmount_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                Clipboard.SetText(lbRecentAmount.Text);
                _toolTip.Show("Copied to clipboard", lbRecentAmount);
            }

        }
    }
}
