using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XFilesHackLoader
{
    public partial class PasswordChangeForm : Form
    {
        public string OldPassword;
        public string NewPassword;
        public string ConfirmPassword;
        public PasswordChangeForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.OldPassword = OldPasswordText.Text;
            this.NewPassword = NewPasswordText.Text;
            this.ConfirmPassword = ConfirmPasswordText.Text;

            if (NewPassword != ConfirmPassword)
            {
                MessageBox.Show("Konfirmasi kata sandi baru tidak cocok!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NewPassword.Length < 5)
            {
                MessageBox.Show("Kata sandi minimal 5 karakter!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Close();
        }
    }
}
