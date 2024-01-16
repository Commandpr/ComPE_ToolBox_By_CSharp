namespace WindowsFormsApp1
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            label1 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label17 = new System.Windows.Forms.Label();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            label2 = new System.Windows.Forms.Label();
            listView1 = new System.Windows.Forms.ListBox();
            linkLabel2 = new System.Windows.Forms.LinkLabel();
            linkLabel3 = new System.Windows.Forms.LinkLabel();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.Color.RoyalBlue;
            label1.Location = new System.Drawing.Point(2, 19);
            label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(141, 64);
            label1.TabIndex = 0;
            label1.Text = "关于";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(144, 12);
            pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(282, 88);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
            label17.Location = new System.Drawing.Point(47, 423);
            label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(330, 17);
            label17.TabIndex = 7;
            label17.Text = "Copyright © 2024 ComPE-纯净且简洁的Windows PE系统";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
            linkLabel1.LinkColor = System.Drawing.Color.FromArgb(0, 0, 64);
            linkLabel1.Location = new System.Drawing.Point(185, 402);
            linkLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(56, 17);
            linkLabel1.TabIndex = 8;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "官方网站";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.SystemColors.Highlight;
            label2.Location = new System.Drawing.Point(110, 103);
            label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(195, 23);
            label2.TabIndex = 9;
            label2.Text = "Command Open More";
            // 
            // listView1
            // 
            listView1.BackColor = System.Drawing.Color.White;
            listView1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            listView1.ForeColor = System.Drawing.Color.Black;
            listView1.FormattingEnabled = true;
            listView1.HorizontalScrollbar = true;
            listView1.ItemHeight = 14;
            listView1.Location = new System.Drawing.Point(11, 185);
            listView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(415, 214);
            listView1.TabIndex = 10;
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
            linkLabel2.LinkColor = System.Drawing.Color.FromArgb(0, 0, 64);
            linkLabel2.Location = new System.Drawing.Point(103, 402);
            linkLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new System.Drawing.Size(68, 17);
            linkLabel2.TabIndex = 12;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "命令提示符";
            linkLabel2.LinkClicked += linkLabel2_LinkClicked;
            // 
            // linkLabel3
            // 
            linkLabel3.AutoSize = true;
            linkLabel3.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
            linkLabel3.LinkColor = System.Drawing.Color.FromArgb(0, 0, 64);
            linkLabel3.Location = new System.Drawing.Point(254, 402);
            linkLabel3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new System.Drawing.Size(68, 17);
            linkLabel3.TabIndex = 13;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "任务管理器";
            linkLabel3.LinkClicked += linkLabel3_LinkClicked;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
            label3.Location = new System.Drawing.Point(-1, 126);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(441, 51);
            label3.TabIndex = 14;
            label3.Text = "感谢您使用ComPE！\r\n本程序使用C# .NET编写，允许您将ComPE安装到您的设备中，或者保存ISO文件\r\n以下是您的当前系统相关信息";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form3
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(437, 451);
            Controls.Add(label3);
            Controls.Add(linkLabel3);
            Controls.Add(linkLabel2);
            Controls.Add(listView1);
            Controls.Add(label2);
            Controls.Add(linkLabel1);
            Controls.Add(label17);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form3";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "关于ComPE";
            Load += Form3_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listView1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Label label3;
    }
}