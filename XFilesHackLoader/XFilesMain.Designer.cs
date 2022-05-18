namespace XFilesHackLoader
{
    partial class XFilesMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XFilesMain));
            this.MainWorker = new System.ComponentModel.BackgroundWorker();
            this.UserInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.MemberInfoTextBox = new System.Windows.Forms.TextBox();
            this.HDSNChangeGroupBox = new System.Windows.Forms.GroupBox();
            this.HDSNChangeButton = new System.Windows.Forms.Button();
            this.HDSNChangeTextBox = new System.Windows.Forms.TextBox();
            this.HDSNComboBox = new System.Windows.Forms.ComboBox();
            this.GameInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.GameInfoTextBox = new System.Windows.Forms.TextBox();
            this.GameSelectGroupBox = new System.Windows.Forms.GroupBox();
            this.GameSelectButton2 = new System.Windows.Forms.Button();
            this.GameSelectButton1 = new System.Windows.Forms.Button();
            this.GameSelectComboBox = new System.Windows.Forms.ComboBox();
            this.StatusGroupBox = new System.Windows.Forms.GroupBox();
            this.CurrentStatusLabel = new System.Windows.Forms.Label();
            this.StatusProgressBar = new System.Windows.Forms.ProgressBar();
            this.HackWorker = new System.ComponentModel.BackgroundWorker();
            this.SupportGroupBox = new System.Windows.Forms.GroupBox();
            this.ButtonChatSupport = new System.Windows.Forms.Button();
            this.ButtonDlDepedencies = new System.Windows.Forms.Button();
            this.AccountGroupBox = new System.Windows.Forms.GroupBox();
            this.ButtonChangePwd = new System.Windows.Forms.Button();
            this.ButtonPurchase = new System.Windows.Forms.Button();
            this.UserInfoGroupBox.SuspendLayout();
            this.HDSNChangeGroupBox.SuspendLayout();
            this.GameInfoGroupBox.SuspendLayout();
            this.GameSelectGroupBox.SuspendLayout();
            this.StatusGroupBox.SuspendLayout();
            this.SupportGroupBox.SuspendLayout();
            this.AccountGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainWorker
            // 
            this.MainWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MainWorker_DoWork_1);
            // 
            // UserInfoGroupBox
            // 
            this.UserInfoGroupBox.Controls.Add(this.MemberInfoTextBox);
            this.UserInfoGroupBox.Location = new System.Drawing.Point(12, 12);
            this.UserInfoGroupBox.Name = "UserInfoGroupBox";
            this.UserInfoGroupBox.Size = new System.Drawing.Size(262, 100);
            this.UserInfoGroupBox.TabIndex = 0;
            this.UserInfoGroupBox.TabStop = false;
            this.UserInfoGroupBox.Text = "Informasi Pengguna";
            // 
            // MemberInfoTextBox
            // 
            this.MemberInfoTextBox.Location = new System.Drawing.Point(6, 18);
            this.MemberInfoTextBox.Multiline = true;
            this.MemberInfoTextBox.Name = "MemberInfoTextBox";
            this.MemberInfoTextBox.ReadOnly = true;
            this.MemberInfoTextBox.Size = new System.Drawing.Size(250, 76);
            this.MemberInfoTextBox.TabIndex = 0;
            // 
            // HDSNChangeGroupBox
            // 
            this.HDSNChangeGroupBox.Controls.Add(this.HDSNChangeButton);
            this.HDSNChangeGroupBox.Controls.Add(this.HDSNChangeTextBox);
            this.HDSNChangeGroupBox.Controls.Add(this.HDSNComboBox);
            this.HDSNChangeGroupBox.Location = new System.Drawing.Point(12, 118);
            this.HDSNChangeGroupBox.Name = "HDSNChangeGroupBox";
            this.HDSNChangeGroupBox.Size = new System.Drawing.Size(262, 79);
            this.HDSNChangeGroupBox.TabIndex = 1;
            this.HDSNChangeGroupBox.TabStop = false;
            this.HDSNChangeGroupBox.Text = "Ganti HDSN";
            // 
            // HDSNChangeButton
            // 
            this.HDSNChangeButton.Location = new System.Drawing.Point(194, 46);
            this.HDSNChangeButton.Name = "HDSNChangeButton";
            this.HDSNChangeButton.Size = new System.Drawing.Size(62, 27);
            this.HDSNChangeButton.TabIndex = 2;
            this.HDSNChangeButton.Text = "Ganti";
            this.HDSNChangeButton.UseVisualStyleBackColor = true;
            this.HDSNChangeButton.Click += new System.EventHandler(this.HDSNChangeButton_Click);
            // 
            // HDSNChangeTextBox
            // 
            this.HDSNChangeTextBox.Location = new System.Drawing.Point(6, 48);
            this.HDSNChangeTextBox.Multiline = true;
            this.HDSNChangeTextBox.Name = "HDSNChangeTextBox";
            this.HDSNChangeTextBox.Size = new System.Drawing.Size(182, 23);
            this.HDSNChangeTextBox.TabIndex = 1;
            // 
            // HDSNComboBox
            // 
            this.HDSNComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HDSNComboBox.FormattingEnabled = true;
            this.HDSNComboBox.Location = new System.Drawing.Point(6, 19);
            this.HDSNComboBox.Name = "HDSNComboBox";
            this.HDSNComboBox.Size = new System.Drawing.Size(250, 21);
            this.HDSNComboBox.TabIndex = 0;
            this.HDSNComboBox.SelectedIndexChanged += new System.EventHandler(this.HDSNComboBox_SelectedIndexChanged);
            // 
            // GameInfoGroupBox
            // 
            this.GameInfoGroupBox.Controls.Add(this.GameInfoTextBox);
            this.GameInfoGroupBox.Location = new System.Drawing.Point(280, 12);
            this.GameInfoGroupBox.Name = "GameInfoGroupBox";
            this.GameInfoGroupBox.Size = new System.Drawing.Size(262, 100);
            this.GameInfoGroupBox.TabIndex = 1;
            this.GameInfoGroupBox.TabStop = false;
            this.GameInfoGroupBox.Text = "Informasi Game";
            // 
            // GameInfoTextBox
            // 
            this.GameInfoTextBox.Location = new System.Drawing.Point(6, 18);
            this.GameInfoTextBox.Multiline = true;
            this.GameInfoTextBox.Name = "GameInfoTextBox";
            this.GameInfoTextBox.ReadOnly = true;
            this.GameInfoTextBox.Size = new System.Drawing.Size(250, 76);
            this.GameInfoTextBox.TabIndex = 0;
            this.GameInfoTextBox.WordWrap = false;
            // 
            // GameSelectGroupBox
            // 
            this.GameSelectGroupBox.Controls.Add(this.GameSelectButton2);
            this.GameSelectGroupBox.Controls.Add(this.GameSelectButton1);
            this.GameSelectGroupBox.Controls.Add(this.GameSelectComboBox);
            this.GameSelectGroupBox.Location = new System.Drawing.Point(280, 118);
            this.GameSelectGroupBox.Name = "GameSelectGroupBox";
            this.GameSelectGroupBox.Size = new System.Drawing.Size(262, 79);
            this.GameSelectGroupBox.TabIndex = 2;
            this.GameSelectGroupBox.TabStop = false;
            this.GameSelectGroupBox.Text = "Pilih Game";
            // 
            // GameSelectButton2
            // 
            this.GameSelectButton2.Location = new System.Drawing.Point(136, 46);
            this.GameSelectButton2.Name = "GameSelectButton2";
            this.GameSelectButton2.Size = new System.Drawing.Size(120, 27);
            this.GameSelectButton2.TabIndex = 2;
            this.GameSelectButton2.UseVisualStyleBackColor = true;
            this.GameSelectButton2.Visible = false;
            this.GameSelectButton2.Click += new System.EventHandler(this.Button2);
            // 
            // GameSelectButton1
            // 
            this.GameSelectButton1.Location = new System.Drawing.Point(6, 46);
            this.GameSelectButton1.Name = "GameSelectButton1";
            this.GameSelectButton1.Size = new System.Drawing.Size(250, 27);
            this.GameSelectButton1.TabIndex = 1;
            this.GameSelectButton1.Text = "Start Cheat";
            this.GameSelectButton1.UseVisualStyleBackColor = true;
            this.GameSelectButton1.Click += new System.EventHandler(this.Button1);
            // 
            // GameSelectComboBox
            // 
            this.GameSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GameSelectComboBox.FormattingEnabled = true;
            this.GameSelectComboBox.Location = new System.Drawing.Point(6, 19);
            this.GameSelectComboBox.Name = "GameSelectComboBox";
            this.GameSelectComboBox.Size = new System.Drawing.Size(250, 21);
            this.GameSelectComboBox.TabIndex = 0;
            this.GameSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.GameSelectComboBox_SelectedIndexChanged);
            // 
            // StatusGroupBox
            // 
            this.StatusGroupBox.Controls.Add(this.CurrentStatusLabel);
            this.StatusGroupBox.Controls.Add(this.StatusProgressBar);
            this.StatusGroupBox.Location = new System.Drawing.Point(12, 255);
            this.StatusGroupBox.Name = "StatusGroupBox";
            this.StatusGroupBox.Size = new System.Drawing.Size(530, 59);
            this.StatusGroupBox.TabIndex = 3;
            this.StatusGroupBox.TabStop = false;
            this.StatusGroupBox.Text = "Status";
            // 
            // CurrentStatusLabel
            // 
            this.CurrentStatusLabel.AutoSize = true;
            this.CurrentStatusLabel.Location = new System.Drawing.Point(3, 15);
            this.CurrentStatusLabel.Name = "CurrentStatusLabel";
            this.CurrentStatusLabel.Size = new System.Drawing.Size(58, 13);
            this.CurrentStatusLabel.TabIndex = 1;
            this.CurrentStatusLabel.Text = "Menunggu";
            // 
            // StatusProgressBar
            // 
            this.StatusProgressBar.Location = new System.Drawing.Point(6, 31);
            this.StatusProgressBar.Name = "StatusProgressBar";
            this.StatusProgressBar.Size = new System.Drawing.Size(518, 22);
            this.StatusProgressBar.TabIndex = 0;
            // 
            // HackWorker
            // 
            this.HackWorker.WorkerSupportsCancellation = true;
            this.HackWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.HackWorker_DoWork);
            this.HackWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.HackWorker_RunWorkerCompleted);
            // 
            // SupportGroupBox
            // 
            this.SupportGroupBox.Controls.Add(this.ButtonChatSupport);
            this.SupportGroupBox.Controls.Add(this.ButtonDlDepedencies);
            this.SupportGroupBox.Location = new System.Drawing.Point(12, 203);
            this.SupportGroupBox.Name = "SupportGroupBox";
            this.SupportGroupBox.Size = new System.Drawing.Size(262, 46);
            this.SupportGroupBox.TabIndex = 4;
            this.SupportGroupBox.TabStop = false;
            this.SupportGroupBox.Text = "Support";
            // 
            // ButtonChatSupport
            // 
            this.ButtonChatSupport.Location = new System.Drawing.Point(133, 14);
            this.ButtonChatSupport.Name = "ButtonChatSupport";
            this.ButtonChatSupport.Size = new System.Drawing.Size(123, 27);
            this.ButtonChatSupport.TabIndex = 1;
            this.ButtonChatSupport.Text = "Hubungi Admin";
            this.ButtonChatSupport.UseVisualStyleBackColor = true;
            this.ButtonChatSupport.Click += new System.EventHandler(this.ButtonChatSupport_Click);
            // 
            // ButtonDlDepedencies
            // 
            this.ButtonDlDepedencies.Location = new System.Drawing.Point(6, 14);
            this.ButtonDlDepedencies.Name = "ButtonDlDepedencies";
            this.ButtonDlDepedencies.Size = new System.Drawing.Size(123, 27);
            this.ButtonDlDepedencies.TabIndex = 0;
            this.ButtonDlDepedencies.Text = "Install Jamu";
            this.ButtonDlDepedencies.UseVisualStyleBackColor = true;
            this.ButtonDlDepedencies.Click += new System.EventHandler(this.ButtonDlDepedencies_Click);
            // 
            // AccountGroupBox
            // 
            this.AccountGroupBox.Controls.Add(this.ButtonChangePwd);
            this.AccountGroupBox.Controls.Add(this.ButtonPurchase);
            this.AccountGroupBox.Location = new System.Drawing.Point(279, 203);
            this.AccountGroupBox.Name = "AccountGroupBox";
            this.AccountGroupBox.Size = new System.Drawing.Size(262, 46);
            this.AccountGroupBox.TabIndex = 5;
            this.AccountGroupBox.TabStop = false;
            this.AccountGroupBox.Text = "Akun";
            // 
            // ButtonChangePwd
            // 
            this.ButtonChangePwd.Location = new System.Drawing.Point(133, 14);
            this.ButtonChangePwd.Name = "ButtonChangePwd";
            this.ButtonChangePwd.Size = new System.Drawing.Size(123, 27);
            this.ButtonChangePwd.TabIndex = 1;
            this.ButtonChangePwd.Text = "Ganti Kata Sandi";
            this.ButtonChangePwd.UseVisualStyleBackColor = true;
            this.ButtonChangePwd.Click += new System.EventHandler(this.ButtonChangePwd_Click);
            // 
            // ButtonPurchase
            // 
            this.ButtonPurchase.Location = new System.Drawing.Point(6, 14);
            this.ButtonPurchase.Name = "ButtonPurchase";
            this.ButtonPurchase.Size = new System.Drawing.Size(123, 27);
            this.ButtonPurchase.TabIndex = 0;
            this.ButtonPurchase.Text = "Perpanjang Durasi";
            this.ButtonPurchase.UseVisualStyleBackColor = true;
            this.ButtonPurchase.Click += new System.EventHandler(this.ButtonPurchase_Click);
            // 
            // XFilesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 321);
            this.Controls.Add(this.AccountGroupBox);
            this.Controls.Add(this.SupportGroupBox);
            this.Controls.Add(this.StatusGroupBox);
            this.Controls.Add(this.GameSelectGroupBox);
            this.Controls.Add(this.GameInfoGroupBox);
            this.Controls.Add(this.HDSNChangeGroupBox);
            this.Controls.Add(this.UserInfoGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XFilesMain";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.XFilesMain_Load);
            this.UserInfoGroupBox.ResumeLayout(false);
            this.UserInfoGroupBox.PerformLayout();
            this.HDSNChangeGroupBox.ResumeLayout(false);
            this.HDSNChangeGroupBox.PerformLayout();
            this.GameInfoGroupBox.ResumeLayout(false);
            this.GameInfoGroupBox.PerformLayout();
            this.GameSelectGroupBox.ResumeLayout(false);
            this.StatusGroupBox.ResumeLayout(false);
            this.StatusGroupBox.PerformLayout();
            this.SupportGroupBox.ResumeLayout(false);
            this.AccountGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker MainWorker;
        private System.Windows.Forms.GroupBox UserInfoGroupBox;
        private System.Windows.Forms.TextBox MemberInfoTextBox;
        private System.Windows.Forms.GroupBox HDSNChangeGroupBox;
        private System.Windows.Forms.Button HDSNChangeButton;
        private System.Windows.Forms.TextBox HDSNChangeTextBox;
        private System.Windows.Forms.ComboBox HDSNComboBox;
        private System.Windows.Forms.GroupBox GameInfoGroupBox;
        private System.Windows.Forms.TextBox GameInfoTextBox;
        private System.Windows.Forms.GroupBox GameSelectGroupBox;
        private System.Windows.Forms.Button GameSelectButton1;
        private System.Windows.Forms.ComboBox GameSelectComboBox;
        private System.Windows.Forms.GroupBox StatusGroupBox;
        private System.Windows.Forms.Label CurrentStatusLabel;
        private System.Windows.Forms.ProgressBar StatusProgressBar;
        private System.Windows.Forms.Button GameSelectButton2;
        private System.ComponentModel.BackgroundWorker HackWorker;
        private System.Windows.Forms.GroupBox SupportGroupBox;
        private System.Windows.Forms.Button ButtonChatSupport;
        private System.Windows.Forms.Button ButtonDlDepedencies;
        private System.Windows.Forms.GroupBox AccountGroupBox;
        private System.Windows.Forms.Button ButtonChangePwd;
        private System.Windows.Forms.Button ButtonPurchase;
    }
}