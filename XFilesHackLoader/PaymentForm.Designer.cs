namespace XFilesHackLoader
{
    partial class PaymentForm
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
            this.PackageComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PackageDetailTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.PaymentGatewayComboBox = new System.Windows.Forms.ComboBox();
            this.SenderGroupBox = new System.Windows.Forms.GroupBox();
            this.SenderTextBox = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.PaymentInfoTextbox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.ClientCountTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SenderGroupBox.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PackageComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pilih Paket";
            // 
            // PackageComboBox
            // 
            this.PackageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PackageComboBox.FormattingEnabled = true;
            this.PackageComboBox.Location = new System.Drawing.Point(6, 19);
            this.PackageComboBox.Name = "PackageComboBox";
            this.PackageComboBox.Size = new System.Drawing.Size(244, 21);
            this.PackageComboBox.TabIndex = 0;
            this.PackageComboBox.SelectedIndexChanged += new System.EventHandler(this.PackageComboBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PackageDetailTextBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 180);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detail Paket";
            // 
            // PackageDetailTextBox
            // 
            this.PackageDetailTextBox.Location = new System.Drawing.Point(6, 16);
            this.PackageDetailTextBox.Multiline = true;
            this.PackageDetailTextBox.Name = "PackageDetailTextBox";
            this.PackageDetailTextBox.ReadOnly = true;
            this.PackageDetailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.PackageDetailTextBox.Size = new System.Drawing.Size(244, 158);
            this.PackageDetailTextBox.TabIndex = 0;
            this.PackageDetailTextBox.WordWrap = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.PaymentGatewayComboBox);
            this.groupBox3.Location = new System.Drawing.Point(274, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(256, 49);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Metode Pembayaran";
            // 
            // PaymentGatewayComboBox
            // 
            this.PaymentGatewayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PaymentGatewayComboBox.FormattingEnabled = true;
            this.PaymentGatewayComboBox.Location = new System.Drawing.Point(6, 19);
            this.PaymentGatewayComboBox.Name = "PaymentGatewayComboBox";
            this.PaymentGatewayComboBox.Size = new System.Drawing.Size(244, 21);
            this.PaymentGatewayComboBox.TabIndex = 1;
            this.PaymentGatewayComboBox.SelectedIndexChanged += new System.EventHandler(this.PaymentGatewayComboBox_SelectedIndexChanged);
            // 
            // SenderGroupBox
            // 
            this.SenderGroupBox.Controls.Add(this.SenderTextBox);
            this.SenderGroupBox.Location = new System.Drawing.Point(274, 61);
            this.SenderGroupBox.Name = "SenderGroupBox";
            this.SenderGroupBox.Size = new System.Drawing.Size(256, 43);
            this.SenderGroupBox.TabIndex = 2;
            this.SenderGroupBox.TabStop = false;
            this.SenderGroupBox.Text = "Nomor HP Pengirim";
            // 
            // SenderTextBox
            // 
            this.SenderTextBox.Location = new System.Drawing.Point(6, 16);
            this.SenderTextBox.MaxLength = 15;
            this.SenderTextBox.Name = "SenderTextBox";
            this.SenderTextBox.Size = new System.Drawing.Size(244, 20);
            this.SenderTextBox.TabIndex = 0;
            this.SenderTextBox.Text = "08";
            this.SenderTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SenderTextBox_KeyPress);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.PaymentInfoTextbox);
            this.groupBox5.Location = new System.Drawing.Point(274, 103);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(256, 180);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Info Pembayaran";
            // 
            // PaymentInfoTextbox
            // 
            this.PaymentInfoTextbox.Location = new System.Drawing.Point(6, 16);
            this.PaymentInfoTextbox.Multiline = true;
            this.PaymentInfoTextbox.Name = "PaymentInfoTextbox";
            this.PaymentInfoTextbox.ReadOnly = true;
            this.PaymentInfoTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.PaymentInfoTextbox.Size = new System.Drawing.Size(244, 158);
            this.PaymentInfoTextbox.TabIndex = 1;
            this.PaymentInfoTextbox.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "Request Pembayaran";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Location = new System.Drawing.Point(12, 289);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(517, 49);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Aksi";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.ClientCountTextBox);
            this.groupBox7.Location = new System.Drawing.Point(12, 61);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(256, 43);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Jumlah PC";
            // 
            // ClientCountTextBox
            // 
            this.ClientCountTextBox.Location = new System.Drawing.Point(6, 16);
            this.ClientCountTextBox.MaxLength = 6;
            this.ClientCountTextBox.Name = "ClientCountTextBox";
            this.ClientCountTextBox.Size = new System.Drawing.Size(244, 20);
            this.ClientCountTextBox.TabIndex = 0;
            this.ClientCountTextBox.Text = "1";
            this.ClientCountTextBox.TextChanged += new System.EventHandler(this.ClientCountTextBox_TextChanged);
            this.ClientCountTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ClientCountTextBox_KeyPress);
            // 
            // PaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 350);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.SenderGroupBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaymentForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Formulir Perpanjangan";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PaymentForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.SenderGroupBox.ResumeLayout(false);
            this.SenderGroupBox.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox PackageComboBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox PackageDetailTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox PaymentGatewayComboBox;
        private System.Windows.Forms.GroupBox SenderGroupBox;
        private System.Windows.Forms.TextBox SenderTextBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox PaymentInfoTextbox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox ClientCountTextBox;
    }
}