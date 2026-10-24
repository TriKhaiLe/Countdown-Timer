using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniTimer
{
    /// <summary>
    /// Color Picker window that displays AI-suggested gradient colors
    /// from the Gemini API and lets the user pick one to apply or save.
    /// </summary>
    public class ColorPickerWindow : Form
    {
        private readonly MainWindow _mainWindow;
        private readonly DataManager _dataManager;
        private readonly List<GradientColor> _savedColors;

        // UI controls
        private Panel _headerPanel = null!;
        private FlowLayoutPanel _colorGrid = null!;
        private Label _lblStatus = null!;
        private Button _btnRefresh = null!;
        private Panel _loadingPanel = null!;
        private Label _lblLoading = null!;
        private System.Windows.Forms.Timer _loadingAnimTimer = null!;
        private int _loadingDotCount = 0;

        // Color data
        private List<(Color Start, Color End, string Name)> _suggestedColors = new();
        private (Color Start, Color End, string Name)? _selectedColor;

        // Design constants
        private static readonly Color BgDark = Color.FromArgb(18, 18, 32);
        private static readonly Color BgCard = Color.FromArgb(28, 30, 50);
        private static readonly Color BgCardHover = Color.FromArgb(38, 42, 70);
        private static readonly Color BgCardSelected = Color.FromArgb(50, 55, 90);
        private static readonly Color AccentCyan = Color.FromArgb(80, 200, 240);
        private static readonly Color AccentGreen = Color.FromArgb(80, 220, 150);
        private static readonly Color TextMuted = Color.FromArgb(140, 150, 180);
        private static readonly Color TextBright = Color.FromArgb(220, 230, 255);
        private static readonly Color BorderDefault = Color.FromArgb(50, 55, 80);
        private static readonly Color BorderSelected = Color.FromArgb(100, 200, 240);

        public ColorPickerWindow(MainWindow mainWindow, DataManager dataManager, List<GradientColor> savedColors)
        {
            _mainWindow = mainWindow;
            _dataManager = dataManager;
            _savedColors = savedColors;

            SetupUI();
            _ = LoadColorsFromGeminiAsync();
        }

        private void SetupUI()
        {
            // ── Form settings ──────────────────────────────────────
            this.Text = "🎨 AI Color Picker — ChroniTimer";
            this.ClientSize = new Size(580, 620);
            this.BackColor = BgDark;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.DoubleBuffered = true;

            // ── Header ─────────────────────────────────────────────
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 12, 20, 5)
            };

            var lblTitle = new Label
            {
                Text = "🎨 AI Color Suggestions",
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                ForeColor = TextBright,
                AutoSize = true,
                Location = new Point(20, 10),
                BackColor = Color.Transparent
            };

            var lblSubtitle = new Label
            {
                Text = "Powered by Gemini AI  ·  Pick a gradient to apply",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = TextMuted,
                AutoSize = true,
                Location = new Point(22, 42),
                BackColor = Color.Transparent
            };

            var btnManageSaved = new Button
            {
                Text = "🎨 Manage",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 60, 95),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(95, 32),
                Location = new Point(340, 18),
                Cursor = Cursors.Hand
            };
            btnManageSaved.FlatAppearance.BorderSize = 1;
            btnManageSaved.FlatAppearance.BorderColor = BorderDefault;
            btnManageSaved.Click += (s, e) =>
            {
                using var settings = new SettingsWindow(_mainWindow, _dataManager, _savedColors, SettingsTab.SavedColors);
                settings.ShowDialog(this);
            };
            btnManageSaved.MouseEnter += (s, e) => btnManageSaved.BackColor = Color.FromArgb(60, 80, 125);
            btnManageSaved.MouseLeave += (s, e) => btnManageSaved.BackColor = Color.FromArgb(45, 60, 95);

            _btnRefresh = new Button
            {
                Text = "🔄 New AI",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(55, 60, 95),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 32),
                Location = new Point(445, 18),
                Cursor = Cursors.Hand
            };
            _btnRefresh.FlatAppearance.BorderSize = 1;
            _btnRefresh.FlatAppearance.BorderColor = BorderDefault;
            _btnRefresh.Click += async (s, e) => await LoadColorsFromGeminiAsync();
            _btnRefresh.MouseEnter += (s, e) => _btnRefresh.BackColor = Color.FromArgb(70, 75, 120);
            _btnRefresh.MouseLeave += (s, e) => _btnRefresh.BackColor = Color.FromArgb(55, 60, 95);

            _headerPanel.Controls.Add(lblTitle);
            _headerPanel.Controls.Add(lblSubtitle);
            _headerPanel.Controls.Add(btnManageSaved);
            _headerPanel.Controls.Add(_btnRefresh);

            // ── Status bar ─────────────────────────────────────────
            _lblStatus = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                Text = ""
            };

            // ── Loading panel ──────────────────────────────────────
            _loadingPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = true
            };

            _lblLoading = new Label
            {
                Text = "✨ Asking Gemini for beautiful colors...",
                Font = new Font("Segoe UI", 13F, FontStyle.Italic),
                ForeColor = AccentCyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            _loadingPanel.Controls.Add(_lblLoading);

            _loadingAnimTimer = new System.Windows.Forms.Timer { Interval = 400 };
            _loadingAnimTimer.Tick += (s, e) =>
            {
                _loadingDotCount = (_loadingDotCount + 1) % 4;
                string dots = new string('.', _loadingDotCount);
                _lblLoading.Text = $"✨ Asking Gemini for beautiful colors{dots}";
            };

            // ── Color grid ─────────────────────────────────────────
            _colorGrid = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                Padding = new Padding(18, 10, 18, 10),
                WrapContents = true,
                Visible = false
            };

            // ── Bottom button panel ────────────────────────────────
            var bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                ColumnCount = 3,
                BackColor = Color.FromArgb(22, 22, 38),
                Padding = new Padding(20, 8, 20, 8)
            };
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

            var btnApply = CreateActionButton("✅ Apply", AccentGreen);
            btnApply.Click += BtnApply_Click;

            var btnSave = CreateActionButton("💾 Save", AccentCyan);
            btnSave.Click += BtnSave_Click;

            var btnClose = CreateActionButton("✖ Close", Color.FromArgb(180, 80, 80));
            btnClose.Click += (s, e) => this.Close();

            bottomPanel.Controls.Add(btnApply, 0, 0);
            bottomPanel.Controls.Add(btnSave, 1, 0);
            bottomPanel.Controls.Add(btnClose, 2, 0);

            // ── Assemble ───────────────────────────────────────────
            this.Controls.Add(_colorGrid);
            this.Controls.Add(_loadingPanel);
            this.Controls.Add(_lblStatus);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(_headerPanel);
        }

        private Button CreateActionButton(string text, Color accentColor)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(
                    Math.Max(accentColor.R - 60, 20),
                    Math.Max(accentColor.G - 60, 20),
                    Math.Max(accentColor.B - 60, 20)),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 5, 0)
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = accentColor;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(
                Math.Min((int)accentColor.R, 120),
                Math.Min((int)accentColor.G, 120),
                Math.Min((int)accentColor.B, 120));
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(
                Math.Max(accentColor.R - 60, 20),
                Math.Max(accentColor.G - 60, 20),
                Math.Max(accentColor.B - 60, 20));
            return btn;
        }

        // ═════════════════════════════════════════════════════════════
        //  GEMINI API CALL
        // ═════════════════════════════════════════════════════════════

        private async Task LoadColorsFromGeminiAsync()
        {
            // Show loading state
            _loadingPanel.Visible = true;
            _colorGrid.Visible = false;
            _btnRefresh.Enabled = false;
            _lblStatus.Text = "";
            _selectedColor = null;
            _loadingAnimTimer.Start();

            try
            {
                _suggestedColors = await GeminiColorService.GetSuggestedColorsAsync();
                _lblStatus.Text = $"✨ {_suggestedColors.Count} colors generated by Gemini AI";
                _lblStatus.ForeColor = AccentGreen;
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"⚠ API error – showing fallback colors. ({ex.Message})";
                _lblStatus.ForeColor = Color.FromArgb(255, 180, 80);
                // Fallback colors are already returned by the service
                _suggestedColors = await Task.FromResult(GetLocalFallbackColors());
            }
            finally
            {
                _loadingAnimTimer.Stop();
                _loadingPanel.Visible = false;
                _colorGrid.Visible = true;
                _btnRefresh.Enabled = true;
            }

            PopulateColorGrid();
        }

        private List<(Color Start, Color End, string Name)> GetLocalFallbackColors()
        {
            return new List<(Color Start, Color End, string Name)>
            {
                (Color.FromArgb(15, 25, 55), Color.FromArgb(60, 100, 170), "Sapphire Night"),
                (Color.FromArgb(30, 15, 50), Color.FromArgb(120, 50, 130), "Amethyst Glow"),
                (Color.FromArgb(10, 35, 35), Color.FromArgb(30, 120, 100), "Emerald Depths"),
                (Color.FromArgb(40, 25, 15), Color.FromArgb(150, 80, 30),  "Copper Sunset"),
                (Color.FromArgb(20, 20, 40), Color.FromArgb(80, 60, 140),  "Lavender Haze"),
                (Color.FromArgb(10, 30, 50), Color.FromArgb(40, 90, 160),  "Atlantic Blue"),
                (Color.FromArgb(35, 10, 30), Color.FromArgb(140, 40, 80),  "Rose Noir"),
                (Color.FromArgb(15, 30, 20), Color.FromArgb(40, 130, 70),  "Forest Mist"),
                (Color.FromArgb(30, 30, 30), Color.FromArgb(90, 90, 90),   "Silver Shadow"),
                (Color.FromArgb(25, 15, 45), Color.FromArgb(100, 50, 160), "Plum Twilight"),
            };
        }

        // ═════════════════════════════════════════════════════════════
        //  COLOR GRID
        // ═════════════════════════════════════════════════════════════

        private void PopulateColorGrid()
        {
            _colorGrid.Controls.Clear();

            for (int i = 0; i < _suggestedColors.Count; i++)
            {
                var color = _suggestedColors[i];
                var card = CreateColorCard(color, i);
                _colorGrid.Controls.Add(card);
            }
        }

        private Panel CreateColorCard((Color Start, Color End, string Name) color, int index)
        {
            int cardWidth = 258;
            int cardHeight = 110;

            var card = new Panel
            {
                Size = new Size(cardWidth, cardHeight),
                Margin = new Padding(6),
                BackColor = BgCard,
                Cursor = Cursors.Hand,
                Tag = index
            };

            // ── Gradient preview area ──────────────────────────────
            var preview = new Panel
            {
                Location = new Point(8, 8),
                Size = new Size(cardWidth - 16, 55),
                BackColor = Color.Transparent
            };

            preview.Paint += (s, e) =>
            {
                var rect = preview.ClientRectangle;
                if (rect.Width > 0 && rect.Height > 0)
                {
                    using var brush = new LinearGradientBrush(
                        rect, color.Start, color.End, 45F);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Rounded rectangle
                    using var path = RoundedRect(rect, 6);
                    e.Graphics.FillPath(brush, path);
                }
            };

            // ── Name label ─────────────────────────────────────────
            var lblName = new Label
            {
                Text = color.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextBright,
                Location = new Point(10, 68),
                Size = new Size(cardWidth - 20, 18),
                BackColor = Color.Transparent
            };

            // ── RGB info ───────────────────────────────────────────
            var lblRgb = new Label
            {
                Text = $"({color.Start.R},{color.Start.G},{color.Start.B}) → ({color.End.R},{color.End.G},{color.End.B})",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = TextMuted,
                Location = new Point(10, 88),
                Size = new Size(cardWidth - 20, 16),
                BackColor = Color.Transparent
            };

            card.Controls.Add(preview);
            card.Controls.Add(lblName);
            card.Controls.Add(lblRgb);

            // ── Selection / hover behavior ─────────────────────────
            void OnCardClick(object? sender, EventArgs e)
            {
                _selectedColor = color;

                // Reset all cards
                foreach (Control ctrl in _colorGrid.Controls)
                {
                    if (ctrl is Panel p)
                    {
                        p.BackColor = BgCard;
                        p.Invalidate();
                    }
                }
                // Highlight selected
                card.BackColor = BgCardSelected;
                card.Invalidate();
                _lblStatus.Text = $"🎯 Selected: {color.Name}";
                _lblStatus.ForeColor = AccentCyan;
            }

            // Wire click to the card and all children
            card.Click += OnCardClick;
            preview.Click += OnCardClick;
            lblName.Click += OnCardClick;
            lblRgb.Click += OnCardClick;

            // Hover
            void OnEnter(object? s, EventArgs e)
            {
                if (_selectedColor == null || _selectedColor.Value.Name != color.Name)
                    card.BackColor = BgCardHover;
            }
            void OnLeave(object? s, EventArgs e)
            {
                if (_selectedColor == null || _selectedColor.Value.Name != color.Name)
                    card.BackColor = BgCard;
            }

            card.MouseEnter += OnEnter;
            card.MouseLeave += OnLeave;
            preview.MouseEnter += OnEnter;
            preview.MouseLeave += OnLeave;
            lblName.MouseEnter += OnEnter;
            lblName.MouseLeave += OnLeave;
            lblRgb.MouseEnter += OnEnter;
            lblRgb.MouseLeave += OnLeave;

            // ── Selection border paint ─────────────────────────────
            card.Paint += (s, e) =>
            {
                bool isSelected = _selectedColor != null && _selectedColor.Value.Name == color.Name;
                var borderColor = isSelected ? BorderSelected : BorderDefault;
                int thickness = isSelected ? 2 : 1;

                using var pen = new Pen(borderColor, thickness);
                var rect = card.ClientRectangle;
                rect.Inflate(-1, -1);
                using var path = RoundedRect(rect, 8);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            };

            return card;
        }

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ═════════════════════════════════════════════════════════════
        //  ACTIONS
        // ═════════════════════════════════════════════════════════════

        private void BtnApply_Click(object? sender, EventArgs e)
        {
            if (_selectedColor == null)
            {
                _lblStatus.Text = "⚠ Please select a color first!";
                _lblStatus.ForeColor = Color.FromArgb(255, 180, 80);
                return;
            }

            var (start, end, name) = _selectedColor.Value;
            _mainWindow.ApplyGradientFromExternal(start, end);
            _lblStatus.Text = $"✅ Applied \"{name}\" to the main window!";
            _lblStatus.ForeColor = AccentGreen;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (_selectedColor == null)
            {
                _lblStatus.Text = "⚠ Please select a color first!";
                _lblStatus.ForeColor = Color.FromArgb(255, 180, 80);
                return;
            }

            var (start, end, name) = _selectedColor.Value;

            // Check if already saved
            bool alreadySaved = _savedColors.Any(sc =>
                sc.StartR == start.R && sc.StartG == start.G && sc.StartB == start.B &&
                sc.EndR == end.R && sc.EndG == end.G && sc.EndB == end.B);

            if (alreadySaved)
            {
                _lblStatus.Text = $"🎨 \"{name}\" is already in your saved colors!";
                _lblStatus.ForeColor = Color.FromArgb(255, 180, 80);
                return;
            }

            _savedColors.Add(new GradientColor
            {
                Name = name,
                StartR = start.R, StartG = start.G, StartB = start.B,
                EndR = end.R, EndG = end.G, EndB = end.B
            });
            _dataManager.SaveColors(_savedColors);

            _lblStatus.Text = $"💾 \"{name}\" saved! ({_savedColors.Count} total)";
            _lblStatus.ForeColor = AccentGreen;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Draw a subtle gradient background for the window itself
            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                using var brush = new LinearGradientBrush(
                    ClientRectangle,
                    BgDark,
                    Color.FromArgb(24, 26, 48),
                    90F);
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }
}
