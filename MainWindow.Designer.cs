namespace ChroniTask
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.period_box = new System.Windows.Forms.TextBox();
            this.start_btn = new System.Windows.Forms.Button();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.date_lb = new System.Windows.Forms.Label();
            this._pauseTimer = new System.Windows.Forms.Timer(this.components);
            this.checkBoxHibernate = new System.Windows.Forms.CheckBox();
            this.lb_periodBox = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbRecentAmount = new System.Windows.Forms.Label();
            this.panelExternalTime = new System.Windows.Forms.Panel();
            this.add_btn = new System.Windows.Forms.Button();
            this.outer_time_box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarTime = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelExternalTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // period_box
            // 
            this.period_box.Dock = System.Windows.Forms.DockStyle.Top;
            this.period_box.Location = new System.Drawing.Point(118, 3);
            this.period_box.Name = "period_box";
            this.period_box.Size = new System.Drawing.Size(133, 26);
            this.period_box.TabIndex = 18;
            this.period_box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.period_box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.period_box_KeyPress);
            // 
            // start_btn
            // 
            this.start_btn.BackColor = System.Drawing.SystemColors.Window;
            this.start_btn.Dock = System.Windows.Forms.DockStyle.Top;
            this.start_btn.Location = new System.Drawing.Point(135, 13);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(100, 36);
            this.start_btn.TabIndex = 10;
            this.start_btn.Text = "Start";
            this.start_btn.UseVisualStyleBackColor = false;
            this.start_btn.Click += new System.EventHandler(this.start_Click);
            this.start_btn.MouseLeave += new System.EventHandler(this.ChangeColor_MouseLeave);
            this.start_btn.MouseHover += new System.EventHandler(this.ChangeColor_MouseHover);
            // 
            // _timer
            // 
            this._timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // date_lb
            // 
            this.date_lb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.date_lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.date_lb.Dock = System.Windows.Forms.DockStyle.Top;
            this.date_lb.Location = new System.Drawing.Point(0, 0);
            this.date_lb.Name = "date_lb";
            this.date_lb.Size = new System.Drawing.Size(371, 25);
            this.date_lb.TabIndex = 21;
            this.date_lb.Text = "Today";
            this.date_lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.date_lb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.date_lb_MouseDoubleClick);
            this.date_lb.MouseHover += new System.EventHandler(this.date_lb_MouseHover);
            // 
            // _pauseTimer
            // 
            this._pauseTimer.Enabled = true;
            this._pauseTimer.Interval = 120000;
            this._pauseTimer.Tick += new System.EventHandler(this._pauseTimer_Tick);
            // 
            // checkBoxHibernate
            // 
            this.checkBoxHibernate.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxHibernate.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxHibernate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxHibernate.Location = new System.Drawing.Point(299, 13);
            this.checkBoxHibernate.Name = "checkBoxHibernate";
            this.checkBoxHibernate.Size = new System.Drawing.Size(69, 43);
            this.checkBoxHibernate.TabIndex = 30;
            this.checkBoxHibernate.Text = "Hibernate";
            this.checkBoxHibernate.UseVisualStyleBackColor = false;
            this.checkBoxHibernate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBoxHibernate_MouseDown);
            // 
            // lb_periodBox
            // 
            this.lb_periodBox.BackColor = System.Drawing.Color.Transparent;
            this.lb_periodBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.lb_periodBox.Location = new System.Drawing.Point(0, 0);
            this.lb_periodBox.Name = "lb_periodBox";
            this.lb_periodBox.Size = new System.Drawing.Size(371, 38);
            this.lb_periodBox.TabIndex = 32;
            this.lb_periodBox.Text = "(minute):";
            this.lb_periodBox.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.lb_periodBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(371, 197);
            this.panel1.TabIndex = 35;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.lbRecentAmount);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 222);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(371, 47);
            this.panel3.TabIndex = 37;
            this.panel3.DoubleClick += new System.EventHandler(this.panelExternalTime_DoubleClick);
            // 
            // lbRecentAmount
            // 
            this.lbRecentAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRecentAmount.Location = new System.Drawing.Point(0, 0);
            this.lbRecentAmount.Name = "lbRecentAmount";
            this.lbRecentAmount.Size = new System.Drawing.Size(371, 47);
            this.lbRecentAmount.TabIndex = 0;
            this.lbRecentAmount.Text = "??";
            this.lbRecentAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbRecentAmount.Click += new System.EventHandler(this.lbRecentAmount_Click);
            this.lbRecentAmount.DoubleClick += new System.EventHandler(this.panelExternalTime_DoubleClick);
            this.lbRecentAmount.MouseHover += new System.EventHandler(this.lbRecentAmount_MouseHover);
            this.lbRecentAmount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbRecentAmount_MouseDown);
            this.lbRecentAmount.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbRecentAmount_MouseMove);
            // 
            // panelExternalTime
            // 
            this.panelExternalTime.BackColor = System.Drawing.Color.Transparent;
            this.panelExternalTime.Controls.Add(this.tableLayoutPanel4);
            this.panelExternalTime.Controls.Add(this.tableLayoutPanel5);
            this.panelExternalTime.Controls.Add(this.label1);
            this.panelExternalTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExternalTime.Location = new System.Drawing.Point(0, 269);
            this.panelExternalTime.Name = "panelExternalTime";
            this.panelExternalTime.Size = new System.Drawing.Size(371, 131);
            this.panelExternalTime.TabIndex = 38;
            this.panelExternalTime.DoubleClick += new System.EventHandler(this.panelExternalTime_DoubleClick);
            // 
            // add_btn
            // 
            this.add_btn.Location = new System.Drawing.Point(147, 13);
            this.add_btn.Name = "add_btn";
            this.add_btn.Size = new System.Drawing.Size(75, 29);
            this.add_btn.TabIndex = 28;
            this.add_btn.Text = "Thêm";
            this.add_btn.UseVisualStyleBackColor = true;
            this.add_btn.Click += new System.EventHandler(this.add_btn_Click);
            // 
            // outer_time_box
            // 
            this.outer_time_box.Location = new System.Drawing.Point(154, 13);
            this.outer_time_box.Name = "outer_time_box";
            this.outer_time_box.Size = new System.Drawing.Size(62, 26);
            this.outer_time_box.TabIndex = 26;
            this.outer_time_box.TabStop = false;
            this.outer_time_box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 20);
            this.label1.TabIndex = 27;
            this.label1.Text = "External Time (minute):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.DoubleClick += new System.EventHandler(this.panelExternalTime_DoubleClick);
            // 
            // trackBarTime
            // 
            this.trackBarTime.Location = new System.Drawing.Point(64, 3);
            this.trackBarTime.Maximum = 85;
            this.trackBarTime.MaximumSize = new System.Drawing.Size(300, 30);
            this.trackBarTime.Minimum = 5;
            this.trackBarTime.Name = "trackBarTime";
            this.trackBarTime.Size = new System.Drawing.Size(241, 30);
            this.trackBarTime.TabIndex = 33;
            this.trackBarTime.TickFrequency = 5;
            this.trackBarTime.Value = 45;
            this.trackBarTime.Scroll += new System.EventHandler(this.trackBarTime_Scroll);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.trackBarTime, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 38);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(371, 59);
            this.tableLayoutPanel1.TabIndex = 34;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.96879F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.242198F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.57803F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.242198F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.96879F));
            this.tableLayoutPanel2.Controls.Add(this.period_box, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 97);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(371, 35);
            this.tableLayoutPanel2.TabIndex = 35;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.71429F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.71429F));
            this.tableLayoutPanel3.Controls.Add(this.start_btn, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxHibernate, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 132);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(371, 59);
            this.tableLayoutPanel3.TabIndex = 36;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.76641F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.46718F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.7664F));
            this.tableLayoutPanel5.Controls.Add(this.outer_time_box, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(371, 59);
            this.tableLayoutPanel5.TabIndex = 37;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.0625F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.875F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.0625F));
            this.tableLayoutPanel4.Controls.Add(this.add_btn, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 79);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(371, 48);
            this.tableLayoutPanel4.TabIndex = 38;
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(371, 392);
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Controls.Add(this.panelExternalTime);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.date_lb);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reminder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panelExternalTime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TextBox period_box;
        public System.Windows.Forms.Button start_btn;
        public System.Windows.Forms.Timer _timer;
        public System.Windows.Forms.Label date_lb;
        public System.Windows.Forms.Timer _pauseTimer;
        public System.Windows.Forms.CheckBox checkBoxHibernate;
        public System.Windows.Forms.Label lb_periodBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Panel panelExternalTime;
        public System.Windows.Forms.Button add_btn;
        public System.Windows.Forms.TextBox outer_time_box;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lbRecentAmount;
        public System.Windows.Forms.TrackBar trackBarTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    }
}

