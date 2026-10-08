using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ChroniTimer
{
    /// <summary>
    /// Statistics window that displays historical time tracking data
    /// with a bar chart visualization and data table.
    /// </summary>
    public class StatisticsWindow : Form
    {
        private readonly DataManager _dataManager;
        private List<DayRecord> _history;
        private DayRecord _todayRecord;
        private Panel _chartPanel = null!;
        private DataGridView _dataGrid = null!;
        private Label _lblSummary = null!;
        private ComboBox _cboRange = null!;

        // Chart colors
        private static readonly Color BarColor = Color.FromArgb(80, 180, 240);
        private static readonly Color BarHighlightColor = Color.FromArgb(100, 230, 180);
        private static readonly Color GridLineColor = Color.FromArgb(50, 55, 80);
        private static readonly Color TextColorLight = Color.FromArgb(190, 200, 220);
        private static readonly Color BgDark = Color.FromArgb(22, 24, 40);
        private static readonly Color BgPanel = Color.FromArgb(28, 32, 55);

        public StatisticsWindow(DataManager dataManager, DayRecord todayRecord)
        {
            _dataManager = dataManager;
            _todayRecord = todayRecord;
            _history = _dataManager.LoadHistory();
            SetupUI();
            LoadData(30);
        }

        private void SetupUI()
        {
            this.Text = "📊 ChroniTimer — Statistics";
            this.ClientSize = new Size(700, 520);
            this.BackColor = BgDark;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.DoubleBuffered = true;

            // ── Header ──────────────────────────────────────────────
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.Transparent,
                Padding = new Padding(15, 10, 15, 0)
            };

            var lblTitle = new Label
            {
                Text = "📊 Time History",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 220, 255),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            _cboRange = new ComboBox
            {
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.FromArgb(40, 44, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 120,
                Location = new Point(560, 15)
            };
            _cboRange.Items.AddRange(new object[] { "Last 7 days", "Last 14 days", "Last 30 days", "All time" });
            _cboRange.SelectedIndex = 2;
            _cboRange.SelectedIndexChanged += (s, e) =>
            {
                int days = _cboRange.SelectedIndex switch
                {
                    0 => 7,
                    1 => 14,
                    2 => 30,
                    _ => 9999
                };
                LoadData(days);
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(_cboRange);

            // ── Summary ─────────────────────────────────────────────
            _lblSummary = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(160, 180, 210),
                BackColor = Color.Transparent,
                Padding = new Padding(15, 0, 15, 0)
            };

            // ── Chart ───────────────────────────────────────────────
            _chartPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 220,
                BackColor = BgPanel,
                Padding = new Padding(15, 10, 15, 15)
            };
            _chartPanel.Paint += ChartPanel_Paint;

            // ── Data grid ───────────────────────────────────────────
            _dataGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = BgDark,
                GridColor = GridLineColor,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5F)
            };
            _dataGrid.DefaultCellStyle.BackColor = BgDark;
            _dataGrid.DefaultCellStyle.ForeColor = TextColorLight;
            _dataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 50, 80);
            _dataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _dataGrid.ColumnHeadersDefaultCellStyle.BackColor = BgPanel;
            _dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(180, 200, 240);
            _dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _dataGrid.EnableHeadersVisualStyles = false;

            _dataGrid.Columns.Add("Date", "📅 Date");
            _dataGrid.Columns.Add("Hours", "⏱ Hours");
            _dataGrid.Columns.Add("Bar", "Progress");

            // ── Assemble ────────────────────────────────────────────
            this.Controls.Add(_dataGrid);
            this.Controls.Add(_chartPanel);
            this.Controls.Add(_lblSummary);
            this.Controls.Add(headerPanel);
        }

        private void LoadData(int days)
        {
            // Combine history + today
            var allRecords = new List<DayRecord>(_history);

            // Add/update today in the display set
            var todayIdx = allRecords.FindIndex(r => r.Date == _todayRecord.Date);
            if (todayIdx >= 0)
                allRecords[todayIdx] = _todayRecord;
            else
                allRecords.Add(_todayRecord);

            // Sort by date
            allRecords.Sort((a, b) => string.Compare(a.Date, b.Date, StringComparison.Ordinal));

            // Filter by range
            var cutoff = DateTime.Today.AddDays(-days);
            var filtered = allRecords.Where(r =>
            {
                if (DateTime.TryParse(r.Date, out var dt))
                    return dt >= cutoff;
                return false;
            }).ToList();

            // Update summary
            double totalHours = filtered.Sum(r => r.TotalHours);
            double avgHours = filtered.Count > 0 ? totalHours / filtered.Count : 0;
            double maxHours = filtered.Count > 0 ? filtered.Max(r => r.TotalHours) : 0;
            _lblSummary.Text = $"Total: {totalHours:F1}h  |  Daily avg: {avgHours:F1}h  |  Best day: {maxHours:F1}h  |  {filtered.Count} days";

            // Update grid
            _dataGrid.Rows.Clear();
            foreach (var rec in filtered.AsEnumerable().Reverse())
            {
                string barStr = new string('█', Math.Min((int)(rec.TotalHours * 5), 50));
                _dataGrid.Rows.Add(rec.Date, $"{rec.TotalHours:F2}", barStr);
            }

            // Store for chart
            _chartData = filtered;
            _chartPanel.Invalidate();
        }

        private List<DayRecord> _chartData = new();

        private void ChartPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (_chartData.Count == 0) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var area = _chartPanel.ClientRectangle;
            int leftMargin = 45, rightMargin = 15, topMargin = 10, bottomMargin = 30;
            int chartWidth = area.Width - leftMargin - rightMargin;
            int chartHeight = area.Height - topMargin - bottomMargin;

            double maxVal = Math.Max(_chartData.Max(r => r.TotalHours), 1);
            // Round up to nice number
            maxVal = Math.Ceiling(maxVal);

            int barCount = _chartData.Count;
            float barWidth = Math.Max((float)chartWidth / barCount - 2, 2);
            float gap = 2;

            // Draw horizontal grid lines
            using var gridPen = new Pen(GridLineColor, 1);
            using var textBrush = new SolidBrush(TextColorLight);
            using var smallFont = new Font("Segoe UI", 7.5F);

            for (int i = 0; i <= 4; i++)
            {
                float y = topMargin + chartHeight - (chartHeight * i / 4f);
                g.DrawLine(gridPen, leftMargin, y, leftMargin + chartWidth, y);
                string label = $"{maxVal * i / 4:F1}h";
                g.DrawString(label, smallFont, textBrush, 2, y - 7);
            }

            // Draw bars
            for (int i = 0; i < barCount; i++)
            {
                float x = leftMargin + i * (barWidth + gap);
                float barH = (float)(_chartData[i].TotalHours / maxVal * chartHeight);
                float y = topMargin + chartHeight - barH;

                bool isToday = _chartData[i].Date == _todayRecord.Date;
                var barColor = isToday ? BarHighlightColor : BarColor;

                using var brush = new SolidBrush(Color.FromArgb(180, barColor));
                g.FillRectangle(brush, x, y, barWidth, barH);

                // Date labels (show every Nth label to avoid overlap)
                if (barCount <= 14 || i % (barCount / 7 + 1) == 0 || isToday)
                {
                    string dateLabel = _chartData[i].Date.Length >= 5
                        ? _chartData[i].Date.Substring(5) // "MM-dd"
                        : _chartData[i].Date;
                    var labelSize = g.MeasureString(dateLabel, smallFont);
                    g.DrawString(dateLabel, smallFont, textBrush,
                        x + barWidth / 2 - labelSize.Width / 2,
                        topMargin + chartHeight + 5);
                }
            }
        }
    }
}
