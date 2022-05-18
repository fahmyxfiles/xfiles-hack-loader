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
    public partial class PasswordForm : Form
    {
        public string Password;
        public PasswordForm(string Title, string ButtonText)
        {
            InitializeComponent();
            this.Text = Title;
            this.button1.Text = ButtonText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Password = textBox1.Text;
            this.Close();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {

        }

    }
}
