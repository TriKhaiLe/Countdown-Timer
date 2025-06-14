using System;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using ChroniClock;
using ChroniClock.Utilities;
using System.Drawing;

namespace ChroniTask
{
    public partial class MainWindow : Form
    {
        public PomodoroTimer _pomodoroTimer;
        public MediaPlayerService _mediaPlayer;
        private bool _isPanelExternalTimeCollapsed = false;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public MainWindow()
        {
            InitializeComponent();
            _pomodoroTimer = new PomodoroTimer(this);
            _mediaPlayer = new MediaPlayerService();
            InitializeTrayIcon();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UIHelper.InitializeUI(this);
            LoadAndInitializeDayData();
            panelExternalTime_DoubleClick(sender, e);
        }

        private void InitializeTrayIcon()
        {
            // Khởi tạo ContextMenuStrip
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Start/Reset", null, start_Click);

            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                ContextMenuStrip = trayMenu,
                Visible = true,
                Text = "ChroniClock"
            };

            trayIcon.DoubleClick += (s, e) =>
            {
                ShowInTaskbar = true;
                WindowState = FormWindowState.Normal;
            };
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _pomodoroTimer.HandleTimerTick();
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (_pomodoroTimer.IsRunning)
            {
                _pomodoroTimer.ResetPomodoro();
            }
            else
            {
                _pomodoroTimer.StartPomodoro();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pomodoroTimer.HandleFormClosing(e);
        }

        public ToolTip _toolTip = new ToolTip();

        // For drag-and-drop support
        private Point _dragStartPoint;
        private bool _isDragging = false;

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
                lbRecentAmount.Visible = false;
                Clipboard.SetText(lbRecentAmount.Text);
                lbRecentAmount.Visible = true;
            }

        }

        private void lbRecentAmount_MouseHover(object sender, EventArgs e)
        {
            _toolTip.Show("Right click to copy\nDrag to export", lbRecentAmount);
        }

        private void lbRecentAmount_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragStartPoint = e.Location;
                _isDragging = true;
            }
        }

        private void lbRecentAmount_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.Button == MouseButtons.Left)
            {
                // Start drag if moved enough pixels 
                if (Math.Abs(e.X - _dragStartPoint.X) > SystemInformation.DragSize.Width / 2 ||
                    Math.Abs(e.Y - _dragStartPoint.Y) > SystemInformation.DragSize.Height / 2)
                {
                    _isDragging = false;
                    if (!string.IsNullOrEmpty(lbRecentAmount.Text))
                    {
                        lbRecentAmount.DoDragDrop(lbRecentAmount.Text, DragDropEffects.Copy);
                    }
                }
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
            
        }

        private void trackBarTime_Scroll(object sender, EventArgs e)
        {
            int step = 5;
            int newValue = (trackBarTime.Value / step) * step; // Làm tròn về bội số gần nhất
            trackBarTime.Value = newValue;
            period_box.Text = trackBarTime.Value.ToString();
        }
    }
}
