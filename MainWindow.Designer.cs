
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timeUnit_box = new System.Windows.Forms.ComboBox();
            this.postpone_btn = new System.Windows.Forms.Button();
            this.plus_btn = new System.Windows.Forms.Button();
            this.subtract_btn = new System.Windows.Forms.Button();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.date_lb = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 17);
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
            this.lb_periodBox.Size = new System.Drawing.Size(122, 17);
            this.lb_periodBox.TabIndex = 11;
            this.lb_periodBox.Text = "Thời gian hẹn giờ:";
            // 
            // start_btn
            // 
            this.start_btn.BackColor = System.Drawing.SystemColors.Window;
            this.start_btn.Location = new System.Drawing.Point(287, 177);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(84, 36);
            this.start_btn.TabIndex = 12;
            this.start_btn.Text = "Bắt đầu";
            this.start_btn.UseVisualStyleBackColor = false;
            this.start_btn.Click += new System.EventHandler(this.start_Click);
            this.start_btn.MouseLeave += new System.EventHandler(this.ChangeColor_MouseLeave);
            this.start_btn.MouseHover += new System.EventHandler(this.ChangeColor_MouseHover);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(16, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(81, 21);
            this.radioButton1.TabIndex = 13;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Standby";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Location = new System.Drawing.Point(160, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(135, 28);
            this.panel1.TabIndex = 15;
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
            this.postpone_btn.Location = new System.Drawing.Point(12, 183);
            this.postpone_btn.Name = "postpone_btn";
            this.postpone_btn.Size = new System.Drawing.Size(94, 30);
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
            this.date_lb.Size = new System.Drawing.Size(64, 17);
            this.date_lb.TabIndex = 21;
            this.date_lb.Text = "Hôm nay";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(383, 227);
            this.Controls.Add(this.date_lb);
            this.Controls.Add(this.subtract_btn);
            this.Controls.Add(this.plus_btn);
            this.Controls.Add(this.postpone_btn);
            this.Controls.Add(this.timeUnit_box);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.lb_periodBox);
            this.Controls.Add(this.period_box);
            this.Controls.Add(this.label4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reminder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox period_box;
        private System.Windows.Forms.Label lb_periodBox;
        private System.Windows.Forms.Button start_btn;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox timeUnit_box;
        private System.Windows.Forms.Button postpone_btn;
        private System.Windows.Forms.Button plus_btn;
        private System.Windows.Forms.Button subtract_btn;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.Label date_lb;
    }
}

