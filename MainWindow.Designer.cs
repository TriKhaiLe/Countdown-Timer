
namespace Scheduler
{
    partial class Form1
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
            this.label4 = new System.Windows.Forms.Label();
            this.period_box = new System.Windows.Forms.TextBox();
            this.lb_periodBox = new System.Windows.Forms.Label();
            this.start_btn = new System.Windows.Forms.Button();
            this.timeUnit_box = new System.Windows.Forms.ComboBox();
            this.postpone_btn = new System.Windows.Forms.Button();
            this.plus_btn = new System.Windows.Forms.Button();
            this.subtract_btn = new System.Windows.Forms.Button();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.date_lb = new System.Windows.Forms.Label();
            this.reset_btn = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.outer_time_box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.add_btn = new System.Windows.Forms.Button();
            this._pauseTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Chọn thao tác:\r\n";
            // 
            // period_box
            // 
            this.period_box.Location = new System.Drawing.Point(140, 123);
            this.period_box.Name = "period_box";
            this.period_box.Size = new System.Drawing.Size(107, 22);
            this.period_box.TabIndex = 10;
            // 
            // lb_periodBox
            // 
            this.lb_periodBox.AutoSize = true;
            this.lb_periodBox.Location = new System.Drawing.Point(12, 126);
            this.lb_periodBox.Name = "lb_periodBox";
            this.lb_periodBox.Size = new System.Drawing.Size(113, 16);
            this.lb_periodBox.TabIndex = 11;
            this.lb_periodBox.Text = "Thời gian hẹn giờ:";
            // 
            // start_btn
            // 
            this.start_btn.BackColor = System.Drawing.SystemColors.Window;
            this.start_btn.Location = new System.Drawing.Point(287, 179);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(84, 36);
            this.start_btn.TabIndex = 12;
            this.start_btn.Text = "Bắt đầu";
            this.start_btn.UseVisualStyleBackColor = false;
            this.start_btn.Click += new System.EventHandler(this.start_Click);
            this.start_btn.MouseLeave += new System.EventHandler(this.ChangeColor_MouseLeave);
            this.start_btn.MouseHover += new System.EventHandler(this.ChangeColor_MouseHover);
            // 
            // timeUnit_box
            // 
            this.timeUnit_box.FormattingEnabled = true;
            this.timeUnit_box.Items.AddRange(new object[] {
            "second",
            "minute",
            "hour"});
            this.timeUnit_box.Location = new System.Drawing.Point(296, 121);
            this.timeUnit_box.Name = "timeUnit_box";
            this.timeUnit_box.Size = new System.Drawing.Size(75, 24);
            this.timeUnit_box.TabIndex = 17;
            // 
            // postpone_btn
            // 
            this.postpone_btn.BackColor = System.Drawing.SystemColors.Window;
            this.postpone_btn.Location = new System.Drawing.Point(12, 179);
            this.postpone_btn.Name = "postpone_btn";
            this.postpone_btn.Size = new System.Drawing.Size(94, 36);
            this.postpone_btn.TabIndex = 18;
            this.postpone_btn.Text = "Nhắc lại";
            this.postpone_btn.UseVisualStyleBackColor = false;
            this.postpone_btn.Click += new System.EventHandler(this.start_Click);
            this.postpone_btn.MouseLeave += new System.EventHandler(this.ChangeColor_MouseLeave);
            this.postpone_btn.MouseHover += new System.EventHandler(this.ChangeColor_MouseHover);
            // 
            // plus_btn
            // 
            this.plus_btn.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.plus_btn.Location = new System.Drawing.Point(253, 108);
            this.plus_btn.Name = "plus_btn";
            this.plus_btn.Size = new System.Drawing.Size(36, 24);
            this.plus_btn.TabIndex = 19;
            this.plus_btn.Text = "+";
            this.plus_btn.UseVisualStyleBackColor = false;
            this.plus_btn.Click += new System.EventHandler(this.plus_btn_Click);
            // 
            // subtract_btn
            // 
            this.subtract_btn.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.subtract_btn.Location = new System.Drawing.Point(253, 138);
            this.subtract_btn.Name = "subtract_btn";
            this.subtract_btn.Size = new System.Drawing.Size(36, 24);
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
            this.date_lb.AutoSize = true;
            this.date_lb.BackColor = System.Drawing.SystemColors.HighlightText;
            this.date_lb.Location = new System.Drawing.Point(9, 9);
            this.date_lb.Name = "date_lb";
            this.date_lb.Size = new System.Drawing.Size(61, 16);
            this.date_lb.TabIndex = 21;
            this.date_lb.Text = "Hôm nay";
            // 
            // reset_btn
            // 
            this.reset_btn.Location = new System.Drawing.Point(112, 186);
            this.reset_btn.Name = "reset_btn";
            this.reset_btn.Size = new System.Drawing.Size(75, 23);
            this.reset_btn.TabIndex = 23;
            this.reset_btn.Text = "Reset";
            this.reset_btn.UseVisualStyleBackColor = true;
            this.reset_btn.Click += new System.EventHandler(this.reset_btn_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(169, 41);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(157, 20);
            this.radioButton1.TabIndex = 24;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Countdown n Standby";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(169, 68);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(87, 20);
            this.radioButton2.TabIndex = 25;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Hibernate";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // outer_time_box
            // 
            this.outer_time_box.Location = new System.Drawing.Point(180, 238);
            this.outer_time_box.Name = "outer_time_box";
            this.outer_time_box.Size = new System.Drawing.Size(67, 22);
            this.outer_time_box.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 27;
            this.label1.Text = "Thời gian ngoài (phút):";
            // 
            // add_btn
            // 
            this.add_btn.Location = new System.Drawing.Point(253, 237);
            this.add_btn.Name = "add_btn";
            this.add_btn.Size = new System.Drawing.Size(75, 23);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(383, 272);
            this.Controls.Add(this.add_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outer_time_box);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.reset_btn);
            this.Controls.Add(this.date_lb);
            this.Controls.Add(this.subtract_btn);
            this.Controls.Add(this.plus_btn);
            this.Controls.Add(this.postpone_btn);
            this.Controls.Add(this.timeUnit_box);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.lb_periodBox);
            this.Controls.Add(this.period_box);
            this.Controls.Add(this.label4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reminder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox period_box;
        private System.Windows.Forms.Label lb_periodBox;
        private System.Windows.Forms.Button start_btn;
        private System.Windows.Forms.ComboBox timeUnit_box;
        private System.Windows.Forms.Button postpone_btn;
        private System.Windows.Forms.Button plus_btn;
        private System.Windows.Forms.Button subtract_btn;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.Label date_lb;
        private System.Windows.Forms.Button reset_btn;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TextBox outer_time_box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button add_btn;
        private System.Windows.Forms.Timer _pauseTimer;
    }
}

