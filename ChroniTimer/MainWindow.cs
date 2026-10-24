using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ChroniTimer
{
    public partial class MainWindow : Form
    {
        // ── Services ────────────────────────────────────────────────
        private readonly DataManager _dataManager;
        private readonly TimerEngine _timerEngine;
        private readonly SoundService _soundService;

        // ── UI State ────────────────────────────────────────────────
        private (Color Start, Color End) _currentGradient;
        public (Color Start, Color End) CurrentGradient => _currentGradient;
        private PaintEventHandler? _gradientPaintHandler;
        private bool _isPanelExternalCollapsed = true;
        private List<GradientColor> _savedColors = new();

        // ── Gradient palette ────────────────────────────────────────
        private static readonly Random _rng = new();
        private static readonly (Color Start, Color End, string Name)[] Gradients =
        {
            (Color.FromArgb(30, 30, 60),    Color.FromArgb(80, 50, 120),   "Deep Cosmos"),
            (Color.FromArgb(20, 30, 48),    Color.FromArgb(36, 59, 85),    "Midnight Ocean"),
            (Color.FromArgb(44, 62, 80),    Color.FromArgb(52, 152, 219),  "Azure Depths"),
            (Color.FromArgb(15, 32, 39),    Color.FromArgb(32, 58, 67),    "Dark Teal"),
            (Color.FromArgb(25, 25, 50),    Color.FromArgb(100, 60, 150),  "Violet Dusk"),
            (Color.FromArgb(35, 37, 38),    Color.FromArgb(65, 67, 69),    "Charcoal Mist"),
            (Color.FromArgb(26, 42, 58),    Color.FromArgb(56, 90, 120),   "Steel Blue"),
            (Color.FromArgb(40, 20, 60),    Color.FromArgb(140, 50, 100),  "Berry Night"),
            (Color.FromArgb(10, 25, 47),    Color.FromArgb(30, 80, 130),   "Deep Navy"),
            (Color.FromArgb(20, 40, 30),    Color.FromArgb(50, 110, 80),   "Forest Shadow"),
        };

        // ── Timers ──────────────────────────────────────────────────
        private System.Windows.Forms.Timer _tickTimer = null!;
        private System.Windows.Forms.Timer _pauseFlashTimer = null!;

        // ── Tray ────────────────────────────────────────────────────
        private NotifyIcon _trayIcon = null!;
        private ContextMenuStrip _trayMenu = null!;

        // ── Controls (created in code for modern look) ──────────────
        private Label _lblTitle = null!;
        private Label _lblDate = null!;
        private Label _lblTimerDisplay = null!;
        private Label _lblStatus = null!;
        private Label _lblRecentAmount = null!;
        private TrackBar _trackBarHours = null!;
        private Label _lblSliderValue = null!;
        private Button _btnStart = null!;
        private CheckBox _chkHibernate = null!;
        private Panel _panelExternal = null!;
        private Label _lblExternalTitle = null!;
        private TextBox _txtExternalHours = null!;
        private Button _btnAddExternal = null!;
        private ToolTip _toolTip = null!;
        private Button _btnSaveColor = null!;


        public MainWindow()
        {
            InitializeComponent();

            _dataManager = new DataManager();
            _dataManager.DataFolderChanged += OnDataFolderChanged;
            _timerEngine = new TimerEngine(_dataManager);
            _soundService = new SoundService();

            SetupModernUI();
            SetupTimers();
            SetupTrayIcon();
            _savedColors = _dataManager.LoadSavedColors();
            ApplyRandomGradient();
            RefreshDateLabel();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(400, 450);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ChroniTimer";
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(30, 30, 60);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            this.FormClosing += MainWindow_FormClosing;
            this.Resize += MainWindow_Resize;
            this.ResumeLayout(false);
        }

        // ═════════════════════════════════════════════════════════════
        //  MODERN UI SETUP
        // ═════════════════════════════════════════════════════════════

        private void SetupModernUI()
        {
            _toolTip = new ToolTip { BackColor = Color.FromArgb(40, 40, 70), ForeColor = Color.White };

            // ── Title ───────────────────────────────────────────────
            _lblTitle = new Label
            {
                Text = "⏱ ChroniTimer",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 220, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.Transparent
            };

            // ── Date / stats label ──────────────────────────────────
            _lblDate = new Label
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(160, 180, 220),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 24,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            _lblDate.MouseDoubleClick += LblDate_MouseDoubleClick;
            _toolTip.SetToolTip(_lblDate, "Left double-click: refresh  |  Right double-click: random gradient");

            // ── Timer display ───────────────────────────────────────
            _lblTimerDisplay = new Label
            {
                Text = "00:00:00",
                Font = new Font("Consolas", 36F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 220, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Transparent
            };

            // ── Status ──────────────────────────────────────────────
            _lblStatus = new Label
            {
                Text = "Set your target hours ↓",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(180, 200, 230),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 22,
                BackColor = Color.Transparent
            };

            // ── Slider panel ────────────────────────────────────────
            var sliderPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.Transparent,
                Padding = new Padding(30, 5, 30, 5)
            };

            _trackBarHours = new TrackBar
            {
                Minimum = 1,        // 5m
                Maximum = 20,       // 100m
                Value = 2,          // 10m default
                TickFrequency = 1,
                LargeChange = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 60)
            };
            _trackBarHours.Scroll += TrackBarHours_Scroll;

            _lblSliderValue = new Label
            {
                Text = "10 minutes",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 200, 100),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Width = 55,
                BackColor = Color.Transparent
            };

            sliderPanel.Controls.Add(_trackBarHours);

            // ── Button row ──────────────────────────────────────────
            var buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                ColumnCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 5, 20, 5)
            };
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            _btnStart = CreateModernButton("▶  Start", Color.FromArgb(50, 180, 120));
            _btnStart.Click += BtnStart_Click;
            _btnStart.MouseEnter += (s, e) => _btnStart.BackColor = Color.FromArgb(70, 210, 150);
            _btnStart.MouseLeave += (s, e) =>
            {
                _btnStart.BackColor = _timerEngine.IsRunning
                    ? Color.FromArgb(220, 80, 60)
                    : Color.FromArgb(50, 180, 120);
            };

            _chkHibernate = new CheckBox
            {
                Text = "💤 Hibernate",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(180, 180, 210),
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Appearance = Appearance.Button,
                FlatStyle = FlatStyle.Flat
            };
            _chkHibernate.FlatAppearance.BorderSize = 1;
            _chkHibernate.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 120);
            _chkHibernate.FlatAppearance.CheckedBackColor = Color.FromArgb(60, 60, 100);
            _chkHibernate.MouseDown += ChkHibernate_MouseDown;
            _toolTip.SetToolTip(_chkHibernate, "Check, then right-click to hibernate");

            buttonPanel.Controls.Add(new Label { BackColor = Color.Transparent }, 0, 0);
            buttonPanel.Controls.Add(_btnStart, 1, 0);
            buttonPanel.Controls.Add(_chkHibernate, 2, 0);

            // ── Save color / AI Colors / Stats / Settings action row ──
            var actionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
                ColumnCount = 4,
                BackColor = Color.Transparent,
                Padding = new Padding(12, 2, 12, 2)
            };
            actionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            actionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            actionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            actionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            _btnSaveColor = CreateModernButton("💾 Save", Color.FromArgb(70, 60, 110));
            _btnSaveColor.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _btnSaveColor.Click += BtnSaveColor_Click;
            _toolTip.SetToolTip(_btnSaveColor, "Save current gradient to your favorites");

            var btnAiColors = CreateModernButton("✨ AI", Color.FromArgb(60, 50, 100));
            btnAiColors.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnAiColors.Click += BtnAiColors_Click;
            _toolTip.SetToolTip(btnAiColors, "Get AI-suggested background colors from Gemini");

            var btnStats = CreateModernButton("📊 Stats", Color.FromArgb(50, 70, 100));
            btnStats.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnStats.Click += BtnStats_Click;
            _toolTip.SetToolTip(btnStats, "View your time tracking history");

            var btnSettings = CreateModernButton("⚙ Settings", Color.FromArgb(45, 60, 95));
            btnSettings.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnSettings.Click += BtnSettings_Click;
            _toolTip.SetToolTip(btnSettings, "Settings: data storage location and manage saved colors");

            actionPanel.Controls.Add(_btnSaveColor, 0, 0);
            actionPanel.Controls.Add(btnAiColors, 1, 0);
            actionPanel.Controls.Add(btnStats, 2, 0);
            actionPanel.Controls.Add(btnSettings, 3, 0);

            // ── Recent amount ───────────────────────────────────────
            _lblRecentAmount = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 230, 180),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            _lblRecentAmount.Click += LblRecentAmount_Click;
            _lblRecentAmount.DoubleClick += (s, e) => ToggleExternalPanel();
            _toolTip.SetToolTip(_lblRecentAmount, "Right-click to copy  |  Double-click to toggle external time");

            // ── External time panel (collapsible) ───────────────────
            _panelExternal = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = Color.Transparent,
                Visible = false,
                Padding = new Padding(30, 5, 30, 5)
            };

            _lblExternalTitle = new Label
            {
                Text = "Add External Hours",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(160, 180, 220),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 22
            };

            var externalInputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 35,
                ColumnCount = 2,
                BackColor = Color.Transparent
            };
            externalInputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            externalInputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            _txtExternalHours = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                BackColor = Color.FromArgb(40, 40, 75),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Center,
                Dock = DockStyle.Fill
            };
            _txtExternalHours.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    e.Handled = true;
            };

            _btnAddExternal = CreateModernButton("+ Add", Color.FromArgb(80, 130, 200));
            _btnAddExternal.Click += BtnAddExternal_Click;

            externalInputPanel.Controls.Add(_txtExternalHours, 0, 0);
            externalInputPanel.Controls.Add(_btnAddExternal, 1, 0);

            _panelExternal.Controls.Add(externalInputPanel);
            _panelExternal.Controls.Add(_lblExternalTitle);

            // ── Assembly: add to form (reverse order for Dock.Top) ──
            this.Controls.Add(_panelExternal);
            this.Controls.Add(_lblRecentAmount);
            this.Controls.Add(actionPanel);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(_lblSliderValue);
            this.Controls.Add(sliderPanel);
            this.Controls.Add(_lblStatus);
            this.Controls.Add(_lblTimerDisplay);
            this.Controls.Add(_lblDate);
            this.Controls.Add(_lblTitle);
        }

        private Button CreateModernButton(string text, Color bgColor)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = bgColor,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // ═════════════════════════════════════════════════════════════
        //  GRADIENT PAINTING
        // ═════════════════════════════════════════════════════════════

        private void ApplyRandomGradient()
        {
            // Build combined pool: built-in gradients + saved colors
            var allGradients = new List<(Color Start, Color End)>();
            foreach (var g in Gradients)
                allGradients.Add((g.Start, g.End));
            foreach (var sc in _savedColors)
                allGradients.Add((Color.FromArgb(sc.StartR, sc.StartG, sc.StartB),
                                  Color.FromArgb(sc.EndR, sc.EndG, sc.EndB)));

            var pick = allGradients[_rng.Next(allGradients.Count)];
            ApplyGradient(pick.Start, pick.End);
        }

        private void ApplyGradient(Color start, Color end)
        {
            _currentGradient = (start, end);

            if (_gradientPaintHandler != null)
                this.Paint -= _gradientPaintHandler;

            _gradientPaintHandler = (s, e) =>
            {
                if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
                {
                    using var brush = new LinearGradientBrush(
                        ClientRectangle, start, end, 45F);
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            };
            this.Paint += _gradientPaintHandler;
            this.Invalidate();
        }

        /// <summary>
        /// Public entry point so the ColorPickerWindow can apply a gradient.
        /// </summary>
        public void ApplyGradientFromExternal(Color start, Color end)
        {
            ApplyGradient(start, end);
        }

        // ═════════════════════════════════════════════════════════════
        //  TIMERS
        // ═════════════════════════════════════════════════════════════

        private void SetupTimers()
        {
            _tickTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _tickTimer.Tick += TickTimer_Tick;

            _pauseFlashTimer = new System.Windows.Forms.Timer { Interval = 120000 }; // 2 minutes
            _pauseFlashTimer.Tick += (s, e) => FlashTaskbar.StartFlashing(this);
            _pauseFlashTimer.Start(); // always running – flashes when paused > 2min
        }

        private void TickTimer_Tick(object? sender, EventArgs e)
        {
            if (!_timerEngine.IsRunning) return;

            var elapsed = _timerEngine.Elapsed;
            _lblTimerDisplay.Text = elapsed.ToString(@"hh\:mm\:ss");

            // Progress color shift: starts cyan → turns orange near target → red when over
            double progress = elapsed.TotalHours / _timerEngine.TargetHours;
            if (progress < 0.8)
                _lblTimerDisplay.ForeColor = Color.FromArgb(100, 220, 255);
            else if (progress < 1.0)
                _lblTimerDisplay.ForeColor = Color.FromArgb(255, 200, 100);
            else
                _lblTimerDisplay.ForeColor = Color.FromArgb(255, 100, 80);

            if (_timerEngine.Tick())
            {
                // Target reached!
                CompleteSession();
            }
        }

        private void CompleteSession()
        {
            _tickTimer.Stop();
            double hours = _timerEngine.Stop();

            _soundService.Play();
            _lblStatus.Text = $"✅ Session complete! +{hours:F2}h recorded";
            _lblRecentAmount.Text = $"+{hours:F2} h";
            _btnStart.Text = "▶  Start";
            _btnStart.BackColor = Color.FromArgb(50, 180, 120);

            // Show the window and flash
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            FlashTaskbar.StartFlashing(this);

            RefreshDateLabel();
            _trackBarHours.Enabled = true;
        }

        // ═════════════════════════════════════════════════════════════
        //  TRAY ICON
        // ═════════════════════════════════════════════════════════════

        private void SetupTrayIcon()
        {
            _trayMenu = new ContextMenuStrip();
            _trayMenu.Items.Add("Show", null, (s, e) =>
            {
                ShowInTaskbar = true;
                WindowState = FormWindowState.Normal;
            });
            _trayMenu.Items.Add("Start / Reset", null, BtnStart_Click);
            _trayMenu.Items.Add("📊 Statistics", null, BtnStats_Click);
            _trayMenu.Items.Add("⚙ Settings", null, BtnSettings_Click);
            _trayMenu.Items.Add("-");
            _trayMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

            _trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                ContextMenuStrip = _trayMenu,
                Visible = true,
                Text = "ChroniTimer"
            };
            _trayIcon.DoubleClick += (s, e) =>
            {
                ShowInTaskbar = true;
                WindowState = FormWindowState.Normal;
            };
        }

        // ═════════════════════════════════════════════════════════════
        //  EVENT HANDLERS
        // ═════════════════════════════════════════════════════════════

        private void TrackBarHours_Scroll(object? sender, EventArgs e)
        {
            double minutes = _trackBarHours.Value * 5.0;
            _lblSliderValue.Text = $"{minutes:F1} minutes";
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            if (_timerEngine.IsRunning)
            {
                // Reset / Stop
                _tickTimer.Stop();

                if (MessageBox.Show("Stop and record current session?", "ChroniTimer",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    _tickTimer.Start();
                    return;
                }

                double hours = _timerEngine.Stop();
                _lblStatus.Text = hours > 0
                    ? $"⏹ Stopped. +{hours:F2}h recorded"
                    : "⏹ Stopped.";
                _lblRecentAmount.Text = hours > 0 ? $"+{hours:F2} h" : "";
                _btnStart.Text = "▶  Start";
                _btnStart.BackColor = Color.FromArgb(50, 180, 120);
                _lblTimerDisplay.Text = "00:00:00";
                _lblTimerDisplay.ForeColor = Color.FromArgb(100, 220, 255);
                _trackBarHours.Enabled = true;
                RefreshDateLabel();
            }
            else
            {
                // Start
                double targetMinutes = _trackBarHours.Value * 5.0;
                _timerEngine.Start(targetMinutes / 60);
                _tickTimer.Start();
                _pauseFlashTimer.Stop();
                _pauseFlashTimer.Start(); // reset the pause flash timer
                FlashTaskbar.StopFlashing(this);

                _lblStatus.Text = $"⏱ Counting...";
                _btnStart.Text = "⏹  Reset";
                _btnStart.BackColor = Color.FromArgb(220, 80, 60);
                _trackBarHours.Enabled = false;

                // Minimize to tray
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        private void ChkHibernate_MouseDown(object? sender, MouseEventArgs e)
        {
            if (_timerEngine.IsRunning)
            {
                MessageBox.Show("Please stop the timer before hibernating.",
                    "ChroniTimer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_chkHibernate.Checked && e.Button == MouseButtons.Right)
            {
                _chkHibernate.Checked = false;
                Application.SetSuspendState(PowerState.Hibernate, true, true);
            }
        }

        private void LblDate_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _timerEngine.RefreshTodayData();
                RefreshDateLabel();
                ApplyRandomGradient();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Random gradient with arbitrary colors
                Color c1 = Color.FromArgb(255, _rng.Next(256), _rng.Next(256), _rng.Next(256));
                Color c2 = Color.FromArgb(255, _rng.Next(256), _rng.Next(256), _rng.Next(256));
                ApplyGradient(c1, c2);
            }
        }

        private void LblRecentAmount_Click(object? sender, EventArgs e)
        {
            if (e is MouseEventArgs me && me.Button == MouseButtons.Right)
            {
                if (!string.IsNullOrEmpty(_lblRecentAmount.Text))
                    Clipboard.SetText(_lblRecentAmount.Text);
            }
        }

        private void BtnAddExternal_Click(object? sender, EventArgs e)
        {
            if (double.TryParse(_txtExternalHours.Text, out double hours) && hours > 0)
            {
                _timerEngine.AddExternalTime(hours);
                _lblRecentAmount.Text = $"+{hours:F2} h (ext)";
                _txtExternalHours.Text = "";
                RefreshDateLabel();
            }
            else
            {
                MessageBox.Show("Enter a valid positive number.", "ChroniTimer",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ToggleExternalPanel()
        {
            _isPanelExternalCollapsed = !_isPanelExternalCollapsed;
            _panelExternal.Visible = !_isPanelExternalCollapsed;
            this.Height = _isPanelExternalCollapsed ? 450 : 540;
        }

        private void BtnSaveColor_Click(object? sender, EventArgs e)
        {
            var (start, end) = _currentGradient;

            // Check if this exact color pair already exists
            bool alreadySaved = _savedColors.Any(sc =>
                sc.StartR == start.R && sc.StartG == start.G && sc.StartB == start.B &&
                sc.EndR == end.R && sc.EndG == end.G && sc.EndB == end.B);

            if (alreadySaved)
            {
                _lblStatus.Text = "🎨 This color is already saved!";
                return;
            }

            string name = $"Custom #{_savedColors.Count + 1}";
            _savedColors.Add(new GradientColor
            {
                Name = name,
                StartR = start.R, StartG = start.G, StartB = start.B,
                EndR = end.R, EndG = end.G, EndB = end.B
            });
            _dataManager.SaveColors(_savedColors);
            _lblStatus.Text = $"🎨 Color saved as \"{name}\" ({_savedColors.Count} total)";
        }

        private void BtnAiColors_Click(object? sender, EventArgs e)
        {
            using var colorPicker = new ColorPickerWindow(this, _dataManager, _savedColors);
            colorPicker.ShowDialog(this);
        }

        private void BtnStats_Click(object? sender, EventArgs e)
        {
            using var statsWindow = new StatisticsWindow(_dataManager, _timerEngine.TodayRecord);
            statsWindow.ShowDialog(this);
        }

        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            using var settingsWindow = new SettingsWindow(this, _dataManager, _savedColors);
            settingsWindow.ShowDialog(this);
            // Refresh in case changes were made
            _savedColors = _dataManager.LoadSavedColors();
            _timerEngine.RefreshTodayData();
            RefreshDateLabel();
        }

        public void OnDataFolderChanged()
        {
            _savedColors = _dataManager.LoadSavedColors();
            _timerEngine.RefreshTodayData();
            RefreshDateLabel();
        }

        private void MainWindow_Resize(object? sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
        }

        private void MainWindow_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_timerEngine.IsRunning)
            {
                _tickTimer.Stop();
                if (MessageBox.Show("Timer is running. Stop and exit?", "ChroniTimer",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    _tickTimer.Start();
                    return;
                }
                _timerEngine.Stop();
            }
            _timerEngine.ArchiveToday();

            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            _soundService.Dispose();
        }

        // ═════════════════════════════════════════════════════════════
        //  HELPERS
        // ═════════════════════════════════════════════════════════════

        private void RefreshDateLabel()
        {
            var rec = _timerEngine.TodayRecord;
            _lblDate.Text = $"📅 {DateTime.Today:dd/MM/yyyy}  •  Total: {rec.TotalHours:F2} hours today";
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Suppress default painting – our gradient handler paints the background
        }
    }
}
