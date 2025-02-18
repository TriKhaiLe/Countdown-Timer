using ChroniTask;
using System;
using System.IO;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Tulpep.NotificationWindow;
using ChroniClock.Utilities;

namespace ChroniClock
{
    public class PomodoroTimer
    {
        private readonly MainWindow _form;
        private const double PomodoroUnit = 25.0;
        private const int OneSecond = 1000;

        private int _remainingMilliseconds;
        private double _pomodoroCount = 0;
        public string[] _todayData;

        public PomodoroTimer(MainWindow form)
        {
            _form = form;
        }

        public void LoadTodayData()
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            _todayData = File.Exists("today.txt") ? File.ReadAllLines("today.txt") : new[] { today, "0" };

            if (_todayData[0] != today)
            {
                File.AppendAllLines("history.txt", _todayData);
                _todayData = new[] { today, "0" };
                File.WriteAllLines("today.txt", _todayData);
            }

            if (!double.TryParse(_todayData[1], out _pomodoroCount))
            {
                MessageBox.Show("Error reading Pomodoro data!");
            }
        }

        public void UpdateUI()
        {
            _form.date_lb.Text = $"Today {_todayData[0]}, completed {_todayData[1]} pomodoros";
        }

        public void HandleTimerTick()
        {
            if (_remainingMilliseconds <= 0)
            {
                _form._mediaPlayer.Play();
                CompletePomodoroCycle();
                return;
            }

            UpdateTimerDisplay();
            _remainingMilliseconds -= OneSecond;
        }

        private void UpdateTimerDisplay()
        {
            _form.period_box.Text = TimeSpan.FromMilliseconds(_remainingMilliseconds).ToString();
        }

        private void CompletePomodoroCycle()
        {
            StopAllTimers();
            UpdatePomodoroCount();
            ResetFormFields();
            FlashTaskbar.StartFlashing(_form);
        }

        private void StopAllTimers()
        {
            _form._timer.Stop();
            _form._pauseTimer.Start();
        }

        private void UpdatePomodoroCount()
        {
            double lastPomo = Convert.ToDouble(_todayData[1]);
            _pomodoroCount -= _remainingMilliseconds / 1000.0 / 60.0 / PomodoroUnit;
            double newAmount = _pomodoroCount - lastPomo;
            _form.lbRecentAmount.Text = newAmount.ToString("F1");
            _todayData[1] = _pomodoroCount.ToString("F1");
            _form.date_lb.Text = $"Today {_todayData[0]}, completed {_todayData[1]} pomodoros";
            File.WriteAllLines("today.txt", _todayData);
        }

        private void ResetFormFields()
        {
            _form.lb_periodBox.Text = "Set Timer:";
            _form.lb_periodBox.ForeColor = Color.Black;
            _form.period_box.Text = "45";

            _form.start_btn.Enabled = true;
            _form.plus_btn.Enabled = true;
            _form.subtract_btn.Enabled = true;
            _form.add_btn.Enabled = true;

            _form.reset_btn.Enabled = false;
            _remainingMilliseconds = 0;
        }

        public void HibernateSystem()
        {
            _form.checkBoxHibernate.Checked = false;
            Application.SetSuspendState(PowerState.Hibernate, true, true);
        }


        public void StartPomodoro()
        {
            FlashTaskbar.StopFlashing();
            _form.reset_btn.Enabled = true;
            InitializePomodoroTimer();
        }

        private void InitializePomodoroTimer()
        {
            _remainingMilliseconds = GetPeriodInMilliseconds();
            if (_remainingMilliseconds == -1) return;

            _pomodoroCount += _remainingMilliseconds / 1000.0 / 60.0 / PomodoroUnit;
            DisableFormControls();

            _form.lb_periodBox.Text = "Time remaining:";
            _form.lb_periodBox.ForeColor = Color.Red;

            _form._timer.Interval = OneSecond;
            _form._timer.Start();
            _form._pauseTimer.Stop();

            _form.WindowState = FormWindowState.Minimized;
        }

        private int GetPeriodInMilliseconds()
        {
            try
            {
                int result = Convert.ToInt32(_form.period_box.Text) * OneSecond;
                //int result = Convert.ToInt32(_form.period_box.Text) * 60 * OneSecond;
                return result;
            }
            catch
            {
                MessageBox.Show("Invalid input data. Please check again.");
                return -1;
            }
        }

        private void DisableFormControls()
        {
            _form.start_btn.Enabled = false;
            _form.plus_btn.Enabled = false;
            _form.subtract_btn.Enabled = false;
            _form.add_btn.Enabled = false;
        }

        public void HandleFormClosing(FormClosingEventArgs e)
        {
            StopAllTimers();

            if (MessageBox.Show("Are you sure you want to Exit?", "Hey!", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information) 
                == 
                DialogResult.No)
            {
                e.Cancel = true;
                ResumePomodoroTimer();
                return;
            }

            UpdatePomodoroCount();
        }

        public void ResetPomodoro()
        {
            StopAllTimers();
            if (MessageBox.Show("Are you sure you want to Reset?", "Hey!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                ResumePomodoroTimer();
                return;
            }

            UpdatePomodoroCount();
            ResetFormFields();
        }

        private void ResumePomodoroTimer()
        {
            if (_remainingMilliseconds > 0)
            {
                _form._timer.Start();
                _form._pauseTimer.Stop();
            }
        }


        public void AddExternalPomodoroTime()
        {
            try
            {
                double duration = Convert.ToDouble(_form.outer_time_box.Text);
                _pomodoroCount += duration / PomodoroUnit;
                _todayData[1] = _pomodoroCount.ToString("F1");
                File.WriteAllLines("today.txt", _todayData);

                UpdateUI();
                _form.outer_time_box.Text = "";
            }
            catch
            {
                MessageBox.Show("Invalid input data. Please check again.");
            }
        }

    }
}
