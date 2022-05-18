namespace XFilesHackLoader
{
    partial class PasswordChangeForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OldPasswordText = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.NewPasswordText = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ConfirmPasswordText = new System.Windows.Forms.TextBox();
            this.ButtonAccept = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OldPasswordText);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kata Sandi Lama";
            // 
            // OldPasswordText
            // 
            this.OldPasswordText.Location = new System.Drawing.Point(6, 18);
            this.OldPasswordText.Name = "OldPasswordText";
            this.OldPasswordText.PasswordChar = '*';
            this.OldPasswordText.Size = new System.Drawing.Size(165, 20);
            this.OldPasswordText.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.NewPasswordText);
            this.groupBox2.Location = new System.Drawing.Point(12, 56);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(177, 44);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Kata Sandi Baru";
            // 
            // NewPasswordText
            // 
            this.NewPasswordText.Location = new System.Drawing.Point(6, 18);
            this.NewPasswordText.Name = "NewPasswordText";
            this.NewPasswordText.PasswordChar = '*';
            this.NewPasswordText.Size = new System.Drawing.Size(165, 20);
            this.NewPasswordText.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ConfirmPasswordText);
            this.groupBox3.Location = new System.Drawing.Point(12, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(177, 44);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Konfirmasi Kata Sandi Baru";
            // 
            // ConfirmPasswordText
            // 
            this.ConfirmPasswordText.Location = new System.Drawing.Point(6, 18);
            this.ConfirmPasswordText.Name = "ConfirmPasswordText";
            this.ConfirmPasswordText.PasswordChar = '*';
            this.ConfirmPasswordText.Size = new System.Drawing.Size(165, 20);
            this.ConfirmPasswordText.TabIndex = 0;
            // 
            // ButtonAccept
            // 
            this.ButtonAccept.Location = new System.Drawing.Point(12, 150);
            this.ButtonAccept.Name = "ButtonAccept";
            this.ButtonAccept.Size = new System.Drawing.Size(177, 26);
            this.ButtonAccept.TabIndex = 2;
            this.ButtonAccept.Text = "Ganti Kata Sandi";
            this.ButtonAccept.UseVisualStyleBackColor = true;
            this.ButtonAccept.Click += new System.EventHandler(this.button1_Click);
            // 
            // PasswordChangeForm
            // 
            this.AcceptButton = this.ButtonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 185);
            this.Controls.Add(this.ButtonAccept);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordChangeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Ganti Kata Sandi";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox OldPasswordText;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox NewPasswordText;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox ConfirmPasswordText;
        private System.Windows.Forms.Button ButtonAccept;
    }
}