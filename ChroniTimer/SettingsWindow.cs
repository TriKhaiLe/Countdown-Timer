using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ChroniTimer
{
    public enum SettingsTab
    {
        DataStorage,
        SavedColors
    }

    /// <summary>
    /// Settings window allowing the user to:
    /// 1. Choose where to save data files (today.json, history.json, saved_colors.json).
    /// 2. Manage (view, apply, create, rename, delete) saved gradient colors.
    /// </summary>
    public class SettingsWindow : Form
    {
        private readonly MainWindow _mainWindow;
        private readonly DataManager _dataManager;
        private readonly List<GradientColor> _savedColors;

        // UI containers
        private Panel _headerPanel = null!;
        private Panel _tabBarPanel = null!;
        private Panel _contentPanel = null!;
        private Panel _panelDataStorage = null!;
        private Panel _panelSavedColors = null!;
        private Label _lblStatus = null!;

        // Tab buttons
        private Button _btnTabData = null!;
        private Button _btnTabColors = null!;
        private SettingsTab _currentTab = SettingsTab.DataStorage;

        // Data Storage controls
        private TextBox _txtDataFolder = null!;
        private Label _lblTodayStatus = null!;
        private Label _lblHistoryStatus = null!;
        private Label _lblColorsStatus = null!;
        private Label _lblStorageLocationType = null!;

        // Saved Colors controls
        private FlowLayoutPanel _colorsGrid = null!;
        private TextBox _txtColorFilter = null!;
        private Label _lblColorCount = null!;
        private Panel _emptyStatePanel = null!;

        // Color theme palette matching ChroniTimer
        private static readonly Color BgDark = Color.FromArgb(18, 18, 32);
        private static readonly Color BgPanel = Color.FromArgb(28, 30, 50);
        private static readonly Color BgCardHover = Color.FromArgb(38, 42, 70);
        private static readonly Color AccentCyan = Color.FromArgb(80, 200, 240);
        private static readonly Color AccentGreen = Color.FromArgb(80, 220, 150);
        private static readonly Color AccentViolet = Color.FromArgb(130, 100, 230);
        private static readonly Color AccentDanger = Color.FromArgb(230, 90, 100);
        private static readonly Color TextBright = Color.FromArgb(220, 230, 255);
        private static readonly Color TextMuted = Color.FromArgb(140, 150, 180);
        private static readonly Color BorderDefault = Color.FromArgb(50, 55, 80);

        public SettingsWindow(MainWindow mainWindow, DataManager dataManager, List<GradientColor> savedColors, SettingsTab initialTab = SettingsTab.DataStorage)
        {
            _mainWindow = mainWindow;
            _dataManager = dataManager;
            _savedColors = savedColors;
            _currentTab = initialTab;

            SetupUI();
            SwitchTab(_currentTab);
            UpdateStorageUI();
            PopulateSavedColors();
        }

        private void SetupUI()
        {
            this.Text = "⚙ ChroniTimer — Settings";
            this.ClientSize = new Size(620, 590);
            this.BackColor = BgDark;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.DoubleBuffered = true;

            // ── Top Header ───────────────────────────────────────────
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.Transparent,
                Padding = new Padding(22, 12, 22, 0)
            };

            var lblTitle = new Label
            {
                Text = "⚙ Settings & Preferences",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = TextBright,
                AutoSize = true,
                Location = new Point(20, 10),
                BackColor = Color.Transparent
            };

            var lblSubtitle = new Label
            {
                Text = "Configure data file location and manage saved gradient themes",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextMuted,
                AutoSize = true,
                Location = new Point(22, 38),
                BackColor = Color.Transparent
            };

            _headerPanel.Controls.Add(lblTitle);
            _headerPanel.Controls.Add(lblSubtitle);

            // ── Tab Bar ──────────────────────────────────────────────
            _tabBarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 4, 20, 4)
            };

            _btnTabData = CreateTabButton("📁 Data Storage", 0);
            _btnTabData.Click += (s, e) => SwitchTab(SettingsTab.DataStorage);

            _btnTabColors = CreateTabButton("🎨 Saved Colors", 160);
            _btnTabColors.Click += (s, e) => SwitchTab(SettingsTab.SavedColors);

            _tabBarPanel.Controls.Add(_btnTabData);
            _tabBarPanel.Controls.Add(_btnTabColors);

            // ── Bottom status & close bar ────────────────────────────
            var bottomBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(22, 22, 38),
                Padding = new Padding(20, 8, 20, 8)
            };

            _lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F),
                ForeColor = AccentGreen,
                Text = ""
            };

            var btnClose = new Button
            {
                Text = "✔ Done",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 60, 90),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 90,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 1;
            btnClose.FlatAppearance.BorderColor = BorderDefault;
            btnClose.Click += (s, e) => this.Close();

            bottomBar.Controls.Add(_lblStatus);
            bottomBar.Controls.Add(btnClose);

            // ── Content container ────────────────────────────────────
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 10)
            };

            BuildDataStoragePanel();
            BuildSavedColorsPanel();

            _contentPanel.Controls.Add(_panelDataStorage);
            _contentPanel.Controls.Add(_panelSavedColors);

            // Add all to form
            this.Controls.Add(_contentPanel);
            this.Controls.Add(bottomBar);
            this.Controls.Add(_tabBarPanel);
            this.Controls.Add(_headerPanel);
        }

        private Button CreateTabButton(string text, int left)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 36),
                Location = new Point(left + 20, 4),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void SwitchTab(SettingsTab tab)
        {
            _currentTab = tab;
            bool isData = tab == SettingsTab.DataStorage;

            _panelDataStorage.Visible = isData;
            _panelSavedColors.Visible = !isData;

            // Highlight tab button
            _btnTabData.ForeColor = isData ? AccentCyan : TextMuted;
            _btnTabData.BackColor = isData ? Color.FromArgb(35, 40, 70) : Color.Transparent;

            _btnTabColors.ForeColor = !isData ? AccentCyan : TextMuted;
            _btnTabColors.BackColor = !isData ? Color.FromArgb(35, 40, 70) : Color.Transparent;

            if (isData)
            {
                UpdateStorageUI();
            }
            else
            {
                PopulateSavedColors();
            }
        }

        // ═════════════════════════════════════════════════════════════
        //  TAB 1: DATA STORAGE
        // ═════════════════════════════════════════════════════════════

        private void BuildDataStoragePanel()
        {
            _panelDataStorage = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // ── Card 1: Folder Location ──────────────────────────────
            var cardLocation = CreateSectionCard("📂 Data Storage Folder", 175);

            var lblDesc = new Label
            {
                Text = "Choose where ChroniTimer stores your tracking logs, accumulated time history, and custom color presets.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextMuted,
                Location = new Point(16, 36),
                Size = new Size(540, 32),
                BackColor = Color.Transparent
            };

            _txtDataFolder = new TextBox
            {
                Location = new Point(16, 72),
                Size = new Size(540, 26),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.FromArgb(20, 22, 38),
                ForeColor = TextBright,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };

            var pnlButtons = new FlowLayoutPanel
            {
                Location = new Point(14, 110),
                Size = new Size(550, 40),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight
            };

            var btnBrowse = CreateActionButton("📂 Change Folder...", AccentCyan);
            btnBrowse.Size = new Size(150, 34);
            btnBrowse.Click += BtnBrowseFolder_Click;

            var btnOpenExplorer = CreateActionButton("📁 Open in Explorer", Color.FromArgb(70, 80, 120));
            btnOpenExplorer.Size = new Size(150, 34);
            btnOpenExplorer.Click += (s, e) => OpenInExplorer();

            var btnReset = CreateActionButton("↺ Reset to Default", Color.FromArgb(80, 60, 90));
            btnReset.Size = new Size(150, 34);
            btnReset.Click += BtnResetDefault_Click;

            pnlButtons.Controls.Add(btnBrowse);
            pnlButtons.Controls.Add(btnOpenExplorer);
            pnlButtons.Controls.Add(btnReset);

            _lblStorageLocationType = new Label
            {
                Location = new Point(16, 150),
                Size = new Size(540, 18),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = AccentGreen,
                BackColor = Color.Transparent
            };

            cardLocation.Controls.Add(lblDesc);
            cardLocation.Controls.Add(_txtDataFolder);
            cardLocation.Controls.Add(pnlButtons);
            cardLocation.Controls.Add(_lblStorageLocationType);

            // ── Card 2: Files in Storage ─────────────────────────────
            var cardFiles = CreateSectionCard("📄 Data Files Overview", 180);
            cardFiles.Top = 195;

            var pnlFileList = new TableLayoutPanel
            {
                Location = new Point(16, 40),
                Size = new Size(540, 120),
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            pnlFileList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            pnlFileList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            var lblTodayName = new Label { Text = "today.json", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = TextBright, Dock = DockStyle.Fill };
            _lblTodayStatus = new Label { Text = "Checking...", Font = new Font("Segoe UI", 8.5F), ForeColor = TextMuted, Dock = DockStyle.Fill };

            var lblHistoryName = new Label { Text = "history.json", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = TextBright, Dock = DockStyle.Fill };
            _lblHistoryStatus = new Label { Text = "Checking...", Font = new Font("Segoe UI", 8.5F), ForeColor = TextMuted, Dock = DockStyle.Fill };

            var lblColorsName = new Label { Text = "saved_colors.json", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = TextBright, Dock = DockStyle.Fill };
            _lblColorsStatus = new Label { Text = "Checking...", Font = new Font("Segoe UI", 8.5F), ForeColor = TextMuted, Dock = DockStyle.Fill };

            pnlFileList.Controls.Add(lblTodayName, 0, 0);
            pnlFileList.Controls.Add(_lblTodayStatus, 1, 0);
            pnlFileList.Controls.Add(lblHistoryName, 0, 1);
            pnlFileList.Controls.Add(_lblHistoryStatus, 1, 1);
            pnlFileList.Controls.Add(lblColorsName, 0, 2);
            pnlFileList.Controls.Add(_lblColorsStatus, 1, 2);

            cardFiles.Controls.Add(pnlFileList);

            _panelDataStorage.Controls.Add(cardLocation);
            _panelDataStorage.Controls.Add(cardFiles);
        }

        private void UpdateStorageUI()
        {
            string folder = _dataManager.DataFolder;
            _txtDataFolder.Text = folder;

            bool isDefault = string.Equals(
                Path.GetFullPath(folder),
                Path.GetFullPath(DataManager.DefaultDataFolder),
                StringComparison.OrdinalIgnoreCase);

            _lblStorageLocationType.Text = isDefault
                ? "● Using default AppData folder"
                : "● Using custom user-specified directory";
            _lblStorageLocationType.ForeColor = isDefault ? TextMuted : AccentCyan;

            // Today file status
            if (File.Exists(_dataManager.TodayFilePath))
            {
                var fi = new FileInfo(_dataManager.TodayFilePath);
                var today = _dataManager.LoadToday();
                _lblTodayStatus.Text = $"✅ {fi.Length} bytes • Date: {today.Date} • Total: {today.TotalHours:F2}h";
                _lblTodayStatus.ForeColor = AccentGreen;
            }
            else
            {
                _lblTodayStatus.Text = "⚪ Not created yet (will be created on first session)";
                _lblTodayStatus.ForeColor = TextMuted;
            }

            // History file status
            if (File.Exists(_dataManager.HistoryFilePath))
            {
                var fi = new FileInfo(_dataManager.HistoryFilePath);
                var hist = _dataManager.LoadHistory();
                _lblHistoryStatus.Text = $"✅ {fi.Length} bytes • {hist.Count} archived day(s)";
                _lblHistoryStatus.ForeColor = AccentGreen;
            }
            else
            {
                _lblHistoryStatus.Text = "⚪ Empty / No history file yet";
                _lblHistoryStatus.ForeColor = TextMuted;
            }

            // Colors file status
            if (File.Exists(_dataManager.ColorsFilePath))
            {
                var fi = new FileInfo(_dataManager.ColorsFilePath);
                _lblColorsStatus.Text = $"✅ {fi.Length} bytes • {_savedColors.Count} saved color(s)";
                _lblColorsStatus.ForeColor = AccentGreen;
            }
            else
            {
                _lblColorsStatus.Text = $"⚪ No saved_colors.json ({_savedColors.Count} in memory)";
                _lblColorsStatus.ForeColor = TextMuted;
            }
        }

        private void BtnBrowseFolder_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select a folder to store ChroniTimer data files",
                UseDescriptionForTitle = true,
                InitialDirectory = _dataManager.DataFolder,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                string selectedPath = dialog.SelectedPath;
                if (string.Equals(Path.GetFullPath(selectedPath), Path.GetFullPath(_dataManager.DataFolder), StringComparison.OrdinalIgnoreCase))
                {
                    ShowNotification("ℹ Selected folder is already the current data folder.", AccentCyan);
                    return;
                }

                var choice = MessageBox.Show(
                    "Do you want to copy existing data files (today.json, history.json, saved_colors.json) to the new location?\n\n" +
                    "• Yes: Copy your existing data to the new folder.\n" +
                    "• No: Use the new folder as-is (existing files there will be read, or new ones created).\n" +
                    "• Cancel: Abort folder change.",
                    "Copy Data Files?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (choice == DialogResult.Cancel)
                    return;

                bool copyFiles = (choice == DialogResult.Yes);

                try
                {
                    _dataManager.SetDataFolder(selectedPath, copyFiles);
                    _savedColors.Clear();
                    _savedColors.AddRange(_dataManager.LoadSavedColors());
                    _mainWindow.OnDataFolderChanged();
                    UpdateStorageUI();
                    PopulateSavedColors();
                    ShowNotification("✅ Data location changed successfully!", AccentGreen);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to change data folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnResetDefault_Click(object? sender, EventArgs e)
        {
            if (string.Equals(Path.GetFullPath(_dataManager.DataFolder), Path.GetFullPath(DataManager.DefaultDataFolder), StringComparison.OrdinalIgnoreCase))
            {
                ShowNotification("ℹ Already using the default AppData folder.", AccentCyan);
                return;
            }

            var choice = MessageBox.Show(
                $"Reset data storage location back to default?\n\nDefault: {DataManager.DefaultDataFolder}\n\nDo you want to copy files from the current folder back to default?",
                "Reset to Default Storage",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (choice == DialogResult.Cancel)
                return;

            bool copyFiles = (choice == DialogResult.Yes);

            try
            {
                _dataManager.SetDataFolder(DataManager.DefaultDataFolder, copyFiles);
                _savedColors.Clear();
                _savedColors.AddRange(_dataManager.LoadSavedColors());
                _mainWindow.OnDataFolderChanged();
                UpdateStorageUI();
                PopulateSavedColors();
                ShowNotification("✅ Reset storage location to default!", AccentGreen);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to reset data folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenInExplorer()
        {
            try
            {
                if (Directory.Exists(_dataManager.DataFolder))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _dataManager.DataFolder,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Folder does not exist yet.", "ChroniTimer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════
        //  TAB 2: SAVED COLORS MANAGEMENT
        // ═════════════════════════════════════════════════════════════

        private void BuildSavedColorsPanel()
        {
            _panelSavedColors = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = false
            };

            // ── Toolbar ──────────────────────────────────────────────
            var toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 8)
            };

            _txtColorFilter = new TextBox
            {
                Location = new Point(2, 6),
                Size = new Size(200, 26),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.FromArgb(28, 30, 50),
                ForeColor = TextBright,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "🔍 Search colors..."
            };
            _txtColorFilter.TextChanged += (s, e) => PopulateSavedColors();

            _lblColorCount = new Label
            {
                Location = new Point(215, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = TextMuted,
                Text = "0 saved"
            };

            var btnAddColor = CreateActionButton("➕ Add Custom Color", AccentGreen);
            btnAddColor.Size = new Size(160, 32);
            btnAddColor.Location = new Point(415, 4);
            btnAddColor.Click += BtnAddCustomColor_Click;

            toolbar.Controls.Add(_txtColorFilter);
            toolbar.Controls.Add(_lblColorCount);
            toolbar.Controls.Add(btnAddColor);

            // ── Grid ─────────────────────────────────────────────────
            _colorsGrid = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(0, 6, 0, 6)
            };

            // ── Empty state panel ────────────────────────────────────
            _emptyStatePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = false
            };

            var lblEmpty = new Label
            {
                Text = "🎨\n\nNo saved colors found.\nSave colors from the timer or click '+ Add Custom Color' above!",
                Font = new Font("Segoe UI", 11F),
                ForeColor = TextMuted,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            _emptyStatePanel.Controls.Add(lblEmpty);

            _panelSavedColors.Controls.Add(_colorsGrid);
            _panelSavedColors.Controls.Add(_emptyStatePanel);
            _panelSavedColors.Controls.Add(toolbar);
        }

        private void PopulateSavedColors()
        {
            _colorsGrid.SuspendLayout();
            _colorsGrid.Controls.Clear();

            string query = _txtColorFilter.Text.Trim().ToLowerInvariant();
            var filtered = string.IsNullOrEmpty(query)
                ? _savedColors
                : _savedColors.Where(c => (c.Name ?? "").ToLowerInvariant().Contains(query)).ToList();

            _lblColorCount.Text = $"{_savedColors.Count} saved color{(_savedColors.Count == 1 ? "" : "s")}";

            if (filtered.Count == 0)
            {
                _emptyStatePanel.Visible = true;
                _colorsGrid.Visible = false;
                _colorsGrid.ResumeLayout();
                return;
            }

            _emptyStatePanel.Visible = false;
            _colorsGrid.Visible = true;

            int cardWidth = 276;
            int cardHeight = 120;

            for (int i = 0; i < filtered.Count; i++)
            {
                var colorItem = filtered[i];
                var card = CreateColorManagementCard(colorItem, cardWidth, cardHeight);
                _colorsGrid.Controls.Add(card);
            }

            _colorsGrid.ResumeLayout();
        }

        private Panel CreateColorManagementCard(GradientColor colorItem, int width, int height)
        {
            var card = new Panel
            {
                Size = new Size(width, height),
                Margin = new Padding(5, 5, 5, 5),
                BackColor = BgPanel,
                Cursor = Cursors.Default
            };

            // Gradient swatch
            var swatch = new Panel
            {
                Location = new Point(8, 8),
                Size = new Size(width - 16, 44),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            swatch.Paint += (s, e) =>
            {
                var rect = swatch.ClientRectangle;
                if (rect.Width > 0 && rect.Height > 0)
                {
                    using var brush = new LinearGradientBrush(
                        rect, colorItem.StartColor, colorItem.EndColor, 45F);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using var path = RoundedRect(rect, 6);
                    e.Graphics.FillPath(brush, path);
                }
            };
            swatch.Click += (s, e) => ApplyColorToTimer(colorItem);

            // Name
            var lblName = new Label
            {
                Text = colorItem.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextBright,
                Location = new Point(8, 56),
                Size = new Size(width - 16, 18),
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            // Hex info
            var lblHex = new Label
            {
                Text = $"{colorItem.StartHex} → {colorItem.EndHex}",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = TextMuted,
                Location = new Point(8, 74),
                Size = new Size(130, 16),
                BackColor = Color.Transparent
            };

            // Action buttons on card
            var btnApply = new Button
            {
                Text = "Apply",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 90, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(54, 24),
                Location = new Point(width - 150, 88),
                Cursor = Cursors.Hand
            };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += (s, e) => ApplyColorToTimer(colorItem);

            var btnRename = new Button
            {
                Text = "✏",
                Font = new Font("Segoe UI", 8F),
                ForeColor = TextBright,
                BackColor = Color.FromArgb(50, 55, 80),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, 24),
                Location = new Point(width - 90, 88),
                Cursor = Cursors.Hand
            };
            btnRename.FlatAppearance.BorderSize = 0;
            btnRename.Click += (s, e) => RenameColor(colorItem);

            var btnDelete = new Button
            {
                Text = "🗑",
                Font = new Font("Segoe UI", 8F),
                ForeColor = AccentDanger,
                BackColor = Color.FromArgb(60, 30, 40),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, 24),
                Location = new Point(width - 48, 88),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteColor(colorItem);

            card.Controls.Add(swatch);
            card.Controls.Add(lblName);
            card.Controls.Add(lblHex);
            card.Controls.Add(btnApply);
            card.Controls.Add(btnRename);
            card.Controls.Add(btnDelete);

            // Card border
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderDefault, 1);
                var rect = card.ClientRectangle;
                rect.Inflate(-1, -1);
                using var path = RoundedRect(rect, 8);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            };

            return card;
        }

        private void ApplyColorToTimer(GradientColor colorItem)
        {
            _mainWindow.ApplyGradientFromExternal(colorItem.StartColor, colorItem.EndColor);
            ShowNotification($"✅ Applied \"{colorItem.Name}\" to the timer!", AccentGreen);
        }

        private void RenameColor(GradientColor colorItem)
        {
            string? newName = PromptDialog.Show(this, "Rename Color", "Enter a new name for this gradient:", colorItem.Name);
            if (!string.IsNullOrWhiteSpace(newName) && newName != colorItem.Name)
            {
                colorItem.Name = newName.Trim();
                _dataManager.SaveColors(_savedColors);
                PopulateSavedColors();
                UpdateStorageUI();
                ShowNotification($"✏ Renamed to \"{colorItem.Name}\"", AccentCyan);
            }
        }

        private void DeleteColor(GradientColor colorItem)
        {
            if (MessageBox.Show($"Are you sure you want to delete \"{colorItem.Name}\"?",
                "Delete Color", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _savedColors.Remove(colorItem);
                _dataManager.SaveColors(_savedColors);
                PopulateSavedColors();
                UpdateStorageUI();
                ShowNotification($"🗑 Removed \"{colorItem.Name}\"", AccentDanger);
            }
        }

        private void BtnAddCustomColor_Click(object? sender, EventArgs e)
        {
            using var editor = new ColorEditorDialog();
            if (editor.ShowDialog(this) == DialogResult.OK && editor.ResultColor != null)
            {
                _savedColors.Add(editor.ResultColor);
                _dataManager.SaveColors(_savedColors);
                PopulateSavedColors();
                UpdateStorageUI();
                ShowNotification($"🎨 Saved new color \"{editor.ResultColor.Name}\"!", AccentGreen);
            }
        }

        // ═════════════════════════════════════════════════════════════
        //  HELPERS
        // ═════════════════════════════════════════════════════════════

        private void ShowNotification(string message, Color color)
        {
            _lblStatus.Text = message;
            _lblStatus.ForeColor = color;
        }

        private Panel CreateSectionCard(string title, int height)
        {
            var card = new Panel
            {
                Size = new Size(575, height),
                BackColor = BgPanel,
                Margin = new Padding(0, 0, 0, 15)
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = TextBright,
                Location = new Point(16, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblTitle);

            card.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderDefault, 1);
                var rect = card.ClientRectangle;
                rect.Inflate(-1, -1);
                using var path = RoundedRect(rect, 8);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            };

            return card;
        }

        private Button CreateActionButton(string text, Color accentColor)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(
                    Math.Max(accentColor.R - 70, 25),
                    Math.Max(accentColor.G - 70, 25),
                    Math.Max(accentColor.B - 70, 25)),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 0)
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = accentColor;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(
                Math.Min((int)accentColor.R, 130),
                Math.Min((int)accentColor.G, 130),
                Math.Min((int)accentColor.B, 130));
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(
                Math.Max(accentColor.R - 70, 25),
                Math.Max(accentColor.G - 70, 25),
                Math.Max(accentColor.B - 70, 25));
            return btn;
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                using var brush = new LinearGradientBrush(
                    ClientRectangle,
                    BgDark,
                    Color.FromArgb(24, 26, 44),
                    90F);
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }

    /// <summary>
    /// Dark-themed dialog for creating or editing a custom gradient color.
    /// </summary>
    public class ColorEditorDialog : Form
    {
        public GradientColor? ResultColor { get; private set; }

        private Color _startColor = Color.FromArgb(30, 40, 80);
        private Color _endColor = Color.FromArgb(70, 140, 220);
        private TextBox _txtName = null!;
        private Panel _startSwatch = null!;
        private Panel _endSwatch = null!;
        private Panel _previewPanel = null!;
        private Label _lblStartHex = null!;
        private Label _lblEndHex = null!;

        public ColorEditorDialog(GradientColor? initialColor = null)
        {
            if (initialColor != null)
            {
                _startColor = initialColor.StartColor;
                _endColor = initialColor.EndColor;
            }

            SetupUI(initialColor?.Name ?? "Custom Gradient");
        }

        private void SetupUI(string initialName)
        {
            this.Text = "🎨 Create Custom Gradient";
            this.ClientSize = new Size(420, 360);
            this.BackColor = Color.FromArgb(20, 22, 36);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Name
            var lblName = new Label
            {
                Text = "Gradient Name:",
                Location = new Point(25, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 210, 240)
            };

            _txtName = new TextBox
            {
                Text = initialName,
                Location = new Point(25, 45),
                Size = new Size(370, 26),
                Font = new Font("Segoe UI", 9.5F),
                BackColor = Color.FromArgb(32, 34, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Color Pickers row
            var lblStart = new Label
            {
                Text = "Start Color:",
                Location = new Point(25, 88),
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 210, 240)
            };

            _startSwatch = new Panel
            {
                Location = new Point(25, 112),
                Size = new Size(50, 32),
                BackColor = _startColor,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };
            _startSwatch.Click += (s, e) => PickColor(true);

            var btnPickStart = new Button
            {
                Text = "Pick...",
                Location = new Point(82, 112),
                Size = new Size(70, 32),
                BackColor = Color.FromArgb(45, 50, 80),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnPickStart.Click += (s, e) => PickColor(true);

            _lblStartHex = new Label
            {
                Text = $"#{_startColor.R:X2}{_startColor.G:X2}{_startColor.B:X2}",
                Location = new Point(25, 148),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(140, 150, 180)
            };

            var lblEnd = new Label
            {
                Text = "End Color:",
                Location = new Point(220, 88),
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 210, 240)
            };

            _endSwatch = new Panel
            {
                Location = new Point(220, 112),
                Size = new Size(50, 32),
                BackColor = _endColor,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };
            _endSwatch.Click += (s, e) => PickColor(false);

            var btnPickEnd = new Button
            {
                Text = "Pick...",
                Location = new Point(277, 112),
                Size = new Size(70, 32),
                BackColor = Color.FromArgb(45, 50, 80),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnPickEnd.Click += (s, e) => PickColor(false);

            _lblEndHex = new Label
            {
                Text = $"#{_endColor.R:X2}{_endColor.G:X2}{_endColor.B:X2}",
                Location = new Point(220, 148),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(140, 150, 180)
            };

            // Preview
            var lblPreview = new Label
            {
                Text = "Preview:",
                Location = new Point(25, 175),
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 210, 240)
            };

            _previewPanel = new Panel
            {
                Location = new Point(25, 200),
                Size = new Size(370, 50),
                BackColor = Color.Transparent
            };
            _previewPanel.Paint += (s, e) =>
            {
                var rect = _previewPanel.ClientRectangle;
                if (rect.Width > 0 && rect.Height > 0)
                {
                    using var brush = new LinearGradientBrush(rect, _startColor, _endColor, 45F);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using var path = new GraphicsPath();
                    path.AddArc(rect.X, rect.Y, 12, 12, 180, 90);
                    path.AddArc(rect.Right - 12, rect.Y, 12, 12, 270, 90);
                    path.AddArc(rect.Right - 12, rect.Bottom - 12, 12, 12, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - 12, 12, 12, 90, 90);
                    path.CloseFigure();
                    e.Graphics.FillPath(brush, path);
                }
            };

            // Buttons
            var btnSave = new Button
            {
                Text = "💾 Save Color",
                Location = new Point(190, 285),
                Size = new Size(110, 36),
                BackColor = Color.FromArgb(40, 120, 80),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.Click += (s, e) =>
            {
                string name = _txtName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                    name = "Custom Gradient";

                ResultColor = new GradientColor
                {
                    Name = name,
                    StartR = _startColor.R,
                    StartG = _startColor.G,
                    StartB = _startColor.B,
                    EndR = _endColor.R,
                    EndG = _endColor.G,
                    EndB = _endColor.B
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(310, 285),
                Size = new Size(85, 36),
                BackColor = Color.FromArgb(50, 55, 75),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(lblName);
            this.Controls.Add(_txtName);
            this.Controls.Add(lblStart);
            this.Controls.Add(_startSwatch);
            this.Controls.Add(btnPickStart);
            this.Controls.Add(_lblStartHex);
            this.Controls.Add(lblEnd);
            this.Controls.Add(_endSwatch);
            this.Controls.Add(btnPickEnd);
            this.Controls.Add(_lblEndHex);
            this.Controls.Add(lblPreview);
            this.Controls.Add(_previewPanel);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void PickColor(bool isStart)
        {
            using var dlg = new ColorDialog
            {
                Color = isStart ? _startColor : _endColor,
                FullOpen = true
            };

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (isStart)
                {
                    _startColor = dlg.Color;
                    _startSwatch.BackColor = _startColor;
                    _lblStartHex.Text = $"#{_startColor.R:X2}{_startColor.G:X2}{_startColor.B:X2}";
                }
                else
                {
                    _endColor = dlg.Color;
                    _endSwatch.BackColor = _endColor;
                    _lblEndHex.Text = $"#{_endColor.R:X2}{_endColor.G:X2}{_endColor.B:X2}";
                }
                _previewPanel.Invalidate();
            }
        }
    }

    /// <summary>
    /// Dark-themed prompt dialog for string inputs like renaming.
    /// </summary>
    public static class PromptDialog
    {
        public static string? Show(IWin32Window owner, string title, string prompt, string defaultValue = "")
        {
            using var form = new Form
            {
                Text = title,
                ClientSize = new Size(380, 150),
                BackColor = Color.FromArgb(20, 22, 36),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label
            {
                Text = prompt,
                Location = new Point(20, 15),
                Size = new Size(340, 25),
                ForeColor = Color.FromArgb(210, 220, 245)
            };

            var txt = new TextBox
            {
                Text = defaultValue,
                Location = new Point(20, 45),
                Size = new Size(340, 26),
                BackColor = Color.FromArgb(32, 34, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(190, 95),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(40, 110, 160),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(280, 95),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(50, 55, 75),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            form.Controls.Add(lbl);
            form.Controls.Add(txt);
            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            return form.ShowDialog(owner) == DialogResult.OK ? txt.Text : null;
        }
    }
}
