
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
            this.plus_btn = new System.Windows.Forms.Button();
            this.subtract_btn = new System.Windows.Forms.Button();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.date_lb = new System.Windows.Forms.Label();
            this.reset_btn = new System.Windows.Forms.Button();
            this.outer_time_box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.add_btn = new System.Windows.Forms.Button();
            this._pauseTimer = new System.Windows.Forms.Timer(this.components);
            this.checkBoxHibernate = new System.Windows.Forms.CheckBox();
            this.lb_periodBox = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // period_box
            // 
            this.period_box.Location = new System.Drawing.Point(155, 71);
            this.period_box.Name = "period_box";
            this.period_box.Size = new System.Drawing.Size(107, 26);
            this.period_box.TabIndex = 10;
            this.period_box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.period_box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.period_box_KeyPress);
            // 
            // start_btn
            // 
            this.start_btn.BackColor = System.Drawing.SystemColors.Window;
            this.start_btn.Location = new System.Drawing.Point(298, 111);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(94, 36);
            this.start_btn.TabIndex = 18;
            this.start_btn.Text = "Start";
            this.start_btn.UseVisualStyleBackColor = false;
            this.start_btn.Click += new System.EventHandler(this.start_Click);
            this.start_btn.MouseLeave += new System.EventHandler(this.ChangeColor_MouseLeave);
            this.start_btn.MouseHover += new System.EventHandler(this.ChangeColor_MouseHover);
            // 
            // plus_btn
            // 
            this.plus_btn.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.plus_btn.Location = new System.Drawing.Point(268, 71);
            this.plus_btn.Name = "plus_btn";
            this.plus_btn.Size = new System.Drawing.Size(36, 26);
            this.plus_btn.TabIndex = 19;
            this.plus_btn.Text = "+";
            this.plus_btn.UseVisualStyleBackColor = false;
            this.plus_btn.Click += new System.EventHandler(this.plus_btn_Click);
            // 
            // subtract_btn
            // 
            this.subtract_btn.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.subtract_btn.Location = new System.Drawing.Point(113, 71);
            this.subtract_btn.Name = "subtract_btn";
            this.subtract_btn.Size = new System.Drawing.Size(36, 26);
            this.subtract_btn.TabIndex = 20;
            this.subtract_btn.Text = "-";
            this.subtract_btn.UseVisualStyleBackColor = false;
            this.subtract_btn.Click += new System.EventHandler(this.subtract_btn_Click);
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
            this.date_lb.Size = new System.Drawing.Size(414, 25);
            this.date_lb.TabIndex = 21;
            this.date_lb.Text = "Today";
            this.date_lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.date_lb.DoubleClick += new System.EventHandler(this.date_lb_DoubleClick);
            this.date_lb.MouseHover += new System.EventHandler(this.date_lb_MouseHover);
            // 
            // reset_btn
            // 
            this.reset_btn.Location = new System.Drawing.Point(25, 111);
            this.reset_btn.Name = "reset_btn";
            this.reset_btn.Size = new System.Drawing.Size(94, 36);
            this.reset_btn.TabIndex = 23;
            this.reset_btn.Text = "Reset";
            this.reset_btn.UseVisualStyleBackColor = true;
            this.reset_btn.Click += new System.EventHandler(this.reset_btn_Click);
            // 
            // outer_time_box
            // 
            this.outer_time_box.Location = new System.Drawing.Point(176, 218);
            this.outer_time_box.Name = "outer_time_box";
            this.outer_time_box.Size = new System.Drawing.Size(67, 26);
            this.outer_time_box.TabIndex = 26;
            this.outer_time_box.TabStop = false;
            this.outer_time_box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(4, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 20);
            this.label1.TabIndex = 27;
            this.label1.Text = "External Time (minute):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // add_btn
            // 
            this.add_btn.Location = new System.Drawing.Point(164, 256);
            this.add_btn.Name = "add_btn";
            this.add_btn.Size = new System.Drawing.Size(94, 29);
            this.add_btn.TabIndex = 28;
            this.add_btn.Text = "Thêm";
            this.add_btn.UseVisualStyleBackColor = true;
            this.add_btn.Click += new System.EventHandler(this.add_btn_Click);
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
            this.checkBoxHibernate.Location = new System.Drawing.Point(159, 108);
            this.checkBoxHibernate.Name = "checkBoxHibernate";
            this.checkBoxHibernate.Size = new System.Drawing.Size(133, 44);
            this.checkBoxHibernate.TabIndex = 30;
            this.checkBoxHibernate.Text = "Hibernate";
            this.checkBoxHibernate.UseVisualStyleBackColor = false;
            this.checkBoxHibernate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBoxHibernate_MouseDown);
            // 
            // lb_periodBox
            // 
            this.lb_periodBox.BackColor = System.Drawing.Color.Transparent;
            this.lb_periodBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.lb_periodBox.Location = new System.Drawing.Point(0, 25);
            this.lb_periodBox.Name = "lb_periodBox";
            this.lb_periodBox.Size = new System.Drawing.Size(414, 38);
            this.lb_periodBox.TabIndex = 32;
            this.lb_periodBox.Text = "Set Timer (minute):";
            this.lb_periodBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button1.Location = new System.Drawing.Point(249, 218);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 26);
            this.button1.TabIndex = 33;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button2.Location = new System.Drawing.Point(132, 218);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 26);
            this.button2.TabIndex = 34;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(414, 303);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lb_periodBox);
            this.Controls.Add(this.checkBoxHibernate);
            this.Controls.Add(this.add_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outer_time_box);
            this.Controls.Add(this.reset_btn);
            this.Controls.Add(this.date_lb);
            this.Controls.Add(this.subtract_btn);
            this.Controls.Add(this.plus_btn);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.period_box);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reminder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox period_box;
        public System.Windows.Forms.Button start_btn;
        public System.Windows.Forms.Button plus_btn;
        public System.Windows.Forms.Button subtract_btn;
        public System.Windows.Forms.Timer _timer;
        public System.Windows.Forms.Label date_lb;
        public System.Windows.Forms.Button reset_btn;
        public System.Windows.Forms.TextBox outer_time_box;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button add_btn;
        public System.Windows.Forms.Timer _pauseTimer;
        public System.Windows.Forms.CheckBox checkBoxHibernate;
        public System.Windows.Forms.Label lb_periodBox;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button button2;
    }
}

