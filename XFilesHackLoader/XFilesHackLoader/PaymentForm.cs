using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using XFilesFramework;

namespace XFilesHackLoader
{
    public partial class PaymentForm : Form
    {
        public class ComboboxPackageItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public int Duration { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
        public class ComboboxPGItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
        public XFramework Framework;
        public PaymentForm(XFramework Fm)
        {
            InitializeComponent();
            this.Framework = Fm;
        }
        
        private void PaymentForm_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (Framework.GetPackagesList() == EServerResult.E_SUCCESS)
            {
                for (int i = 0; i < Framework.PackageList.Count; i++)
                {
                    ComboboxPackageItem item = new ComboboxPackageItem();
                    item.Text = Framework.PackageList[i].Name + " " + Framework.PackageList[i].Duration + " Hari";
                    item.Value = Framework.PackageList[i].Alias;
                    item.Duration = Framework.PackageList[i].Duration;
                    PackageComboBox.Items.Add(item);
                }
                PackageComboBox.SelectedIndex = 0;
            }
            else this.Close();
            if (Framework.GetPaymentGatewayList() == EServerResult.E_SUCCESS)
            {
                for (int i = 0; i < Framework.PaymentGatewayList.Count; i++)
                {
                    ComboboxPGItem item = new ComboboxPGItem();
                    item.Text = Framework.PaymentGatewayList[i].Type + " - " + Framework.PaymentGatewayList[i].Name;
                    item.Value = Framework.PaymentGatewayList[i].Alias;
                    PaymentGatewayComboBox.Items.Add(item);
                }
                PaymentGatewayComboBox.SelectedIndex = 0;
            }
            else this.Close();
            this.Enabled = true;
        }

        private void ClientCountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            
        }

        private void PackageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ClientCountTextBox.Text.Length > 0)
            {
                PackageData Package = Framework.PackageList[PackageComboBox.SelectedIndex];

                int ClientCount = int.Parse(ClientCountTextBox.Text);
                int Price = Package.Price;
                int Discount = 0;
                if (ClientCount > 5)
                {
                    Discount = (ClientCount-5) * 5;
                    if (Discount > 70) Discount = 70;
                    Price = Package.Price - ((Package.Price * Discount) / 100);
                }
                int PriceTotal = Price * ClientCount;
                CultureInfo culture = new CultureInfo("id-ID");
                string HargaTotal = Decimal.Parse(PriceTotal.ToString()).ToString("C", culture);
                string Harga = Decimal.Parse(Price.ToString()).ToString("C", culture);
                string PackageInfo = "";
                PackageInfo += "Nama Paket : " + Package.Name + System.Environment.NewLine;
                PackageInfo += "Batas Maksimal PC : " + Package.ClientLimit + System.Environment.NewLine;
                PackageInfo += "Durasi : " + Package.Duration + " Hari" + System.Environment.NewLine;
                PackageInfo += "Diskon : " + Discount + " %" + System.Environment.NewLine;
                PackageInfo += "Harga Per PC : " + Harga + System.Environment.NewLine;
                PackageInfo += "Harga Total : " + HargaTotal + System.Environment.NewLine;
                PackageInfo += "Daftar Game : " + System.Environment.NewLine;
                List<string> GameList = Package.AllowedGames.Split(';').ToList();
                foreach (string Game in GameList)
                {
                    if (Game.Length > 0)
                    {
                        PackageInfo += "- " + Game + System.Environment.NewLine;
                    }
                }
                PackageDetailTextBox.Text = PackageInfo;
            }
        }

        private void PaymentGatewayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaymentGatewayData PaymentGateway = Framework.PaymentGatewayList[PaymentGatewayComboBox.SelectedIndex];

            string PaymentInfo = "";
            PaymentInfo += "Jenis Metode : " + PaymentGateway.Type + System.Environment.NewLine;
            PaymentInfo += "Provider : " + PaymentGateway.Name + System.Environment.NewLine;
            PaymentInfo += "Catatan : " + System.Environment.NewLine;

            if (PaymentGateway.CType == EPGType.E_BANKING)
            {
                PaymentInfo += "- Anda harus mentransfer sesuai nominal yang diberikan" + System.Environment.NewLine;
                PaymentInfo += "- Kesalahan nominal transfer harus menunggu proses admin" + System.Environment.NewLine;
                PaymentInfo += "- Proses pembayaran maksimal 30 menit";
            }
            else if (PaymentGateway.CType == EPGType.E_MOBILECREDIT)
            {
                PaymentInfo += "- Anda akan terkena biaya rate konversi" + System.Environment.NewLine;
                PaymentInfo += "- Anda harus mentransfer sesuai nominal yang diberikan" + System.Environment.NewLine;
                PaymentInfo += "- Anda harus mentransfer dari nomor yang dimasukkan" + System.Environment.NewLine;
                PaymentInfo += "- Anda harus mentransfer pulsa, bukan mengisi pulsa ke nomor tujuan" + System.Environment.NewLine;
                PaymentInfo += "- Mengisi langsung pulsa transaksi dianggap hangus" + System.Environment.NewLine;
                PaymentInfo += "- Kesalahan nominal transfer harus menunggu proses admin" + System.Environment.NewLine;
                PaymentInfo += "- Proses pembayaran maksimal 30 menit";
            }
            PaymentInfoTextbox.Text = PaymentInfo;

            if (PaymentGateway.NeedSender == 1)
            {
                SenderGroupBox.Enabled = true;
            }
            else SenderGroupBox.Enabled = false;
        }

        private void SenderTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ClientCountTextBox_TextChanged(object sender, EventArgs e)
        {
            PackageData Package = Framework.PackageList[PackageComboBox.SelectedIndex];
            if (ClientCountTextBox.Text.Length > 0)
            {
                if (int.Parse(ClientCountTextBox.Text) < 1)
                {
                    ClientCountTextBox.Text = "1";
                }
                if (int.Parse(ClientCountTextBox.Text) > Package.ClientLimit)
                {
                    ClientCountTextBox.Text = Package.ClientLimit.ToString();
                }
            }
            PackageComboBox_SelectedIndexChanged(PackageComboBox, new EventArgs());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PaymentGatewayData PaymentGateway = Framework.PaymentGatewayList[PaymentGatewayComboBox.SelectedIndex];
            PackageData Package = Framework.PackageList[PackageComboBox.SelectedIndex];
            int ClientCount = int.Parse(ClientCountTextBox.Text);
            string Sender = SenderTextBox.Text;
            if (ClientCount > Package.ClientLimit)
            {
                MessageBox.Show("Gagal request pembayaran karena jumlah PC lebih dari batas maksimal paket", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PaymentGateway.CType == EPGType.E_MOBILECREDIT && Sender.Length < 5)
            {
                MessageBox.Show("Gagal request pembayaran karena nomor pengirim tidak valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Framework.RequestPayment(Package.Alias, Package.Duration, ClientCount, PaymentGateway.Alias, Sender) == EServerResult.E_SUCCESS)
            {
                this.Close();
            }
        }
    }
}
