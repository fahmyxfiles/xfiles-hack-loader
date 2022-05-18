using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using XFilesFramework;
using InjectionLibrary;
using JLibrary.PortableExecutable;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Collections;



namespace XFilesHackLoader
{
    public partial class XFilesMain : Form
    {

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        public XFilesMain()
        {
            InitializeComponent();
        }

        public XFramework Framework;
        public IniParser Config;
        public WebClient WC;
        public String[] Hari = { "", "Senin", "Selasa", "Rabu", "Kamis", "Jum'at", "Sabtu", "Minggu" };
        public String[] Bulan = { "", "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };

        

        public static string ExecuteCommand(string command)
        {

            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = new Process())
            {
                proc.StartInfo = procStartInfo;
                proc.Start();

                string output = proc.StandardOutput.ReadToEnd();

                if (string.IsNullOrEmpty(output))
                    output = proc.StandardError.ReadToEnd();

                return output;
            }

        }
        
        private void MainWorker_DoWork_1(object sender, DoWorkEventArgs e)
        {
            if (Config == null) Config = new IniParser("Config.ini");
            if (Framework == null) Framework = new XFramework();
            EServerResult Result = EServerResult.E_FAILED;
            this.Invoke(new Action(() =>
            {
                this.Enabled = false;
                GameSelectComboBox.Items.Clear();
                GameSelectButton1.Enabled = false;
                GameSelectButton2.Enabled = false;

                HDSNComboBox.Items.Clear();
                HDSNChangeGroupBox.Enabled = false;
                HDSNChangeTextBox.Text = "Fitur ini tidak tersedia";

                this.Text = "Sedang Login, Harap Tunggu...";
                CurrentStatusLabel.Text = "Menunggu";
            }));

            Result = Framework.Login();
            if (Result != EServerResult.E_SUCCESS)
            {
                this.Invoke(new Action(() =>
                {
                    this.Close();
                    Application.Exit();
                }));
                return;
            }

            this.Invoke(new Action(() =>
            {
                this.Text = Framework.Session.Product.Name + " Tools";
                this.Enabled = true;
            }));



            string UserInfo = "";
            UserInfo += "Nama Pengguna : " + Framework.Session.User.Username + System.Environment.NewLine;
            UserInfo += "Jumlah PC : " + Framework.Session.User.ClientCount + System.Environment.NewLine;
            UserInfo += "Durasi : " + Framework.Session.User.Duration + " Hari" + System.Environment.NewLine;
            UserInfo += "Kadaluarsa : " + Hari[(int)Framework.Session.User.Expiration.DayOfWeek] + ", " + Framework.Session.User.Expiration.ToString("dd") + " " + Bulan[(int)Framework.Session.User.Expiration.Month] + Framework.Session.User.Expiration.ToString(" yyyy HH:mm:ss") + System.Environment.NewLine;
            this.Invoke(new Action(() =>
            {
                this.MemberInfoTextBox.Text = UserInfo;
            }));

            if (Framework.Session.Product.AllowHDSNChange == 1)
            {
                this.Invoke(new Action(() =>
                {
                    this.HDSNChangeGroupBox.Enabled = true;
                }));
                string ClientListStr = Framework.Session.User.ClientList;
                List<string> ClientList = ClientListStr.Split(';').ToList();
                for (int i = 0; i < Framework.Session.User.ClientCount; i++)
                {
                    string MyHdsn;
                    if (i >= ClientList.Count)
                    {
                        MyHdsn = "";
                    }
                    else MyHdsn = ClientList[i];
                    ComboboxItem item = new ComboboxItem();
                    item.Text = "Slot #" + (i + 1) + " : " + MyHdsn;
                    item.Value = MyHdsn;
                    this.Invoke(new Action(() =>
                    {
                        HDSNComboBox.Items.Add(item);
                    }));
                }
                this.Invoke(new Action(() =>
                {
                    HDSNComboBox.SelectedIndex = 0;
                }));
            }
            if (Framework.Session.Product.AllowInternalSubs == 0)
            {
                ButtonPurchase.Enabled = false;
                ButtonPurchase.Text = "Fitur tidak tersedia";
            }
            for (int i = 0; i < Framework.Session.GameList.Count; i++)
            {
                GameData Game = Framework.Session.GameList[i];
                if (Framework.Session.User.GameId == "AIO" || Framework.Session.User.GameId == Game.Alias)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = Game.Name;
                    item.Value = Game.Alias;
                    this.Invoke(new Action(() =>
                    {
                        GameSelectComboBox.Items.Add(item);
                    }));
                }
            }
            this.Invoke(new Action(() =>
            {
                GameSelectComboBox.SelectedIndex = 0;
            }));
        }

        private void XFilesMain_Load(object sender, EventArgs e)
        {
            MainWorker.RunWorkerAsync();
        }

        private void HDSNComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem item = (ComboboxItem)HDSNComboBox.Items[HDSNComboBox.SelectedIndex];
            this.HDSNChangeTextBox.Text = (string)item.Value;
        }

        private void GameSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameSelectButton1.Enabled = true;

            GameData Game = Framework.Session.GameList[GameSelectComboBox.SelectedIndex];

            String[] GameStatus = { "", "Aktif", "Nonaktif", "Perbaikan", "Aktif" };
            String[] InjectMet = { "", "External", "Internal Folder Game", "Internal Start Game", "Internal Sistem", "Download External", "External Stealth" };

            string GameInfoStats = GameStatus[(int)Game.Status];
            if (Game.Method == EInjectionMethod.E_INTERNAL_GAMEPATH)
            {
                if (Config.KeyExists("InstallPath", Game.Alias) && Config.KeyExists("md5", Game.Alias))
                {
                    string InstallPath = Framework.DecryptInstallPath(Config.Read("InstallPath", Game.Alias));

                    string MD5Hash = Config.Read("md5", Game.Alias);
                    if (File.Exists(InstallPath) && Framework.GetMD5(InstallPath) == MD5Hash)
                    {
                        string VersionInfo = "";
                        if (Config.KeyExists("HackVer", Game.Alias))
                        {
                            VersionInfo = " ( V." + Config.Read("HackVer", Game.Alias) + " )";
                        }
                        GameInfoStats += " | Terpasang" + VersionInfo + System.Environment.NewLine;
                        GameInfoStats += "Lokasi Pemasangan : " + Config.Read(Game.Alias, "InstallPath");
                    }
                    else GameInfoStats += " | Tidak terpasang";
                }
                else GameInfoStats += " | Tidak terpasang";
            }
            else if (Game.Method == EInjectionMethod.E_INTERNAL_ABSOLUTE)
            {
                if (Config.KeyExists("md5", Game.Alias))
                {
                    string MD5Hash = Config.Read("md5", Game.Alias);
                    if (File.Exists(Game.Path) && Framework.GetMD5(Game.Path) == MD5Hash)
                    {
                        string VersionInfo = "";
                        if (Config.KeyExists("HackVer", Game.Alias))
                        {
                            VersionInfo = " ( r" + Config.Read("HackVer", Game.Alias) + " )";
                        }
                        GameInfoStats += " | Terpasang" + VersionInfo;
                    }
                    else if (File.Exists(Game.Path))
                    {
                        GameInfoStats += " | Terpasang - Tidak Valid";
                    }
                    else GameInfoStats += " | Tidak terpasang";
                }
                else GameInfoStats += " | Tidak terpasang";
            }
            string GameInfo = "";
            GameInfo += "Status : " + GameInfoStats + System.Environment.NewLine;
            GameInfo += "Metode : " + InjectMet[(int)Game.Method] + System.Environment.NewLine;
            GameInfo += "Versi  : r" + Game.HackVersion + System.Environment.NewLine;
            GameInfoTextBox.Text = GameInfo;

            if (Game.Method == EInjectionMethod.E_EXTERNAL)
            {
                GameSelectButton1.Text = "Start Cheat";
                GameSelectButton1.Size = new System.Drawing.Size(250, 27); // 120, 27
                GameSelectButton2.Enabled = false;
                GameSelectButton2.Visible = false;
            }
            if (Game.Method == EInjectionMethod.E_INTERNAL_AUTOSTART)
            {
                GameSelectButton1.Text = "Start Cheat";
                GameSelectButton1.Size = new System.Drawing.Size(250, 27); // 120, 27
                GameSelectButton2.Enabled = false;
                GameSelectButton2.Visible = false;
            }
            else if (Game.Method == EInjectionMethod.E_INTERNAL_GAMEPATH || Game.Method == EInjectionMethod.E_INTERNAL_ABSOLUTE)
            {
                GameSelectButton1.Text = "Install Cheat";
                GameSelectButton1.Size = new System.Drawing.Size(250, 27); // 120, 27
                GameSelectButton2.Enabled = false;
                GameSelectButton2.Visible = false;
                if (Game.Method == EInjectionMethod.E_INTERNAL_GAMEPATH)
                {
                    if (Config.KeyExists("InstallPath", Game.Alias) && Config.KeyExists("md5", Game.Alias))
                    {
                        string InstallPath = Framework.DecryptInstallPath(Config.Read("InstallPath", Game.Alias));
                        string MD5Hash = Config.Read("md5", Game.Alias);
                        if (File.Exists(InstallPath) && Framework.GetMD5(InstallPath) == MD5Hash)
                        {
                            GameSelectButton1.Text = "Update Cheat";
                            GameSelectButton2.Text = "Hapus Cheat";
                            GameSelectButton1.Size = new System.Drawing.Size(120, 27); // 120, 27
                            GameSelectButton2.Enabled = true;
                            GameSelectButton2.Visible = true;
                        }
                    }
                }
                else if (Game.Method == EInjectionMethod.E_INTERNAL_ABSOLUTE)
                {
                    if (Config.KeyExists("md5", Game.Alias))
                    {
                        string MD5Hash = Config.Read("md5", Game.Alias);
                        if (File.Exists(Game.Path) && Framework.GetMD5(Game.Path) == MD5Hash)
                        {
                            GameSelectButton1.Text = "Update Cheat";
                            GameSelectButton2.Text = "Hapus Cheat";
                            GameSelectButton1.Size = new System.Drawing.Size(120, 27); // 120, 27
                            GameSelectButton2.Enabled = true;
                            GameSelectButton2.Visible = true;
                        }
                        else if (File.Exists(Game.Path))
                        {
                            GameSelectButton1.Text = "Reinstall Cheat";
                            GameSelectButton2.Text = "Hapus Cheat";
                            GameSelectButton1.Size = new System.Drawing.Size(120, 27); // 120, 27
                            GameSelectButton2.Enabled = true;
                            GameSelectButton2.Visible = true;
                        }
                    }
                }
            }
            else if (Game.Method == EInjectionMethod.E_EXTERNAL_LINK)
            {
                GameSelectButton1.Text = "Download Cheat";
                GameSelectButton1.Size = new System.Drawing.Size(250, 27); // 120, 27
                GameSelectButton2.Enabled = false;
                GameSelectButton2.Visible = false;
            }
            else if (Game.Method == EInjectionMethod.E_EXTERNAL_STEALTH)
            {
                GameSelectButton1.Text = "Start Cheat";
                GameSelectButton1.Size = new System.Drawing.Size(250, 27); // 120, 27
                GameSelectButton2.Enabled = false;
                GameSelectButton2.Visible = false;
            }
        }
        private void SetStatusLabel(string Status)
        {
            this.Invoke(new Action(() =>
            {
                CurrentStatusLabel.Text = Status;
            }));
        }
        private void SetProgressBarValue(int Value)
        {
            this.Invoke(new Action(() =>
            {
                StatusProgressBar.Value = Value;
            }));
        }
        private static string GetCommandLine(Process process)
        {
            var commandLine = new StringBuilder(process.MainModule.FileName);

            commandLine.Append(" ");
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    commandLine.Append(@object["CommandLine"]);
                    commandLine.Append(" ");
                }
            }

            return commandLine.ToString();
        }
        private string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
        //================================ Hack Downloader =======================================
        private void ProgressDownloadChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            SetStatusLabel("Mendownload File ... [" + (e.BytesReceived / 1000) + "KB / " + (e.TotalBytesToReceive / 1000) + "KB]");
            int val = int.Parse(Math.Truncate(percentage).ToString());
            if (val > 0 && val <= 100)
            {
                SetProgressBarValue(val);
            }
        }
        //================================ EXTERNAL START HACK ===================================
        private void Button1(object sender, EventArgs e)
        {
            UserInfoGroupBox.Enabled = false;
            GameInfoGroupBox.Enabled = false;

            if (Framework.Session.Product.AllowHDSNChange == 1) HDSNChangeGroupBox.Enabled = false;
            GameInfoGroupBox.Enabled = false;
            GameSelectGroupBox.Enabled = false;

            SupportGroupBox.Enabled = false;
            AccountGroupBox.Enabled = false;

            SetStatusLabel("Sedang mempersiapkan ...");
            SetProgressBarValue(0);
            GameData Game = Framework.Session.GameList[GameSelectComboBox.SelectedIndex];

            if (Game.Method != EInjectionMethod.E_EXTERNAL_LINK)
            {
                if (WC != null) WC = null;
                WC = new WebClient();
                string DownloadUrl = Game.DownloadUrl.Replace("%GID%", Game.Alias).Replace("%DLHASH%", Framework.Session.User.DownloadHash);
                Uri FileUrl = new Uri(DownloadUrl);
                WC.Proxy = SimpleWebProxy.Default;
                WC.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressDownloadChanged);
                WC.DownloadFileAsync(FileUrl, "dwnData.txd");
                WC.DownloadFileCompleted += new AsyncCompletedEventHandler(StartHack_OnDownloadCompleted);
            }
            else
            {
                Process.Start(Game.Path);
                SetStatusLabel("Menunggu...");
                SetProgressBarValue(0);
            }
        }
        private void Button2(object sender, EventArgs e)
        {
            GameData Game = Framework.Session.GameList[GameSelectComboBox.SelectedIndex];
            DialogResult res = MessageBox.Show(new Form() { TopMost = true }, "Yakin ingin menghapus cheat dari sistem?\nCheat tidak akan muncul di game anda setelah melakukan aksi ini.", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                if (Game.Method == EInjectionMethod.E_INTERNAL_ABSOLUTE)
                {
                    string InstallPath = Game.Path;
                    if (File.Exists(InstallPath))
                    {
                        Framework.killLockingProcess(InstallPath);
                        Framework.UnlockFilePermission(InstallPath);
                        try
                        {
                            File.Delete(InstallPath);
                        }
                        catch
                        {

                        }
                        while (File.Exists(InstallPath))
                        {
                            string Random = Framework.RandomString(10);
                            while (File.Exists(InstallPath + "." + Random))
                            {
                                Random = Framework.RandomString(10);
                            }
                            File.Move(InstallPath, InstallPath + "." + Random);
                        }
                        if (File.Exists(InstallPath + ".bak"))
                        {
                            if (Game.RestoreFile == 1)
                            {
                                File.Move(InstallPath + ".bak", InstallPath);
                            }
                        }
                        if (Game.Property.ProtectOwnerToSystem)
                        {
                            Framework.RestoreFilePermission(InstallPath);
                        }
                    }
                    Config.DeleteSection(Game.Alias);
                }
                else if (Game.Method == EInjectionMethod.E_INTERNAL_GAMEPATH)
                {
                    if (Config.KeyExists("InstallPath", Game.Alias))
                    {
                        string InstallPath = Framework.DecryptInstallPath(Config.Read("InstallPath", Game.Alias));
                        if (File.Exists(InstallPath))
                        {
                            File.Delete(InstallPath);
                            if (File.Exists(InstallPath + ".bak"))
                            {
                                if (Game.RestoreFile == 1)
                                {
                                    File.Copy(InstallPath + ".bak", InstallPath);
                                }
                                File.Delete(InstallPath + ".bak");
                            }
                        }
                        Config.DeleteSection(Game.Alias);
                    }
                }
            }
            GameSelectComboBox_SelectedIndexChanged(GameSelectComboBox, new EventArgs());
        }
        private void StartHack_OnDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (!HackWorker.IsBusy) HackWorker.RunWorkerAsync();
            }
        }

        private void HackWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            Thread.Sleep(1000);
            int SelectedIndex = 0;
            this.Invoke(new Action(() =>
            {
                SelectedIndex = GameSelectComboBox.SelectedIndex;
            }));
            GameData Game = Framework.Session.GameList[SelectedIndex];

            foreach (string InstallPath in Game.CleanFileList)
            {
                if (File.Exists(InstallPath))
                {
                    File.Delete(InstallPath);
                    if (File.Exists(InstallPath + ".bak"))
                    {
                        File.Copy(InstallPath + ".bak", InstallPath);
                        File.Delete(InstallPath + ".bak");
                    }
                }
            }

            SetStatusLabel("Download Sukses!");
            SetProgressBarValue(0);

            byte[] fileArray = File.ReadAllBytes("dwnData.txd");
            byte[] sigPat = new byte[] { 0x68, 0xAD, 0x7F, 0xAC, 0xFC, 0x90, 0xB7, 0x91, 0x9A, 0xFF, 0x84, 0x88, 0xA6, 0x65, 0x8C, 0x58 };
            int sigPos = Framework.FindPattern(fileArray, sigPat, 0);
            if (sigPos != -1)
            {
                string rndSigStr = Framework.RandomString(256);
                byte[] rndSigByte = Encoding.ASCII.GetBytes(rndSigStr);
                for (int i = 0; i < 256; i++)
                {
                    fileArray[sigPos + i] = rndSigByte[i];
                }
                File.WriteAllBytes("dwnData.txd", fileArray);
            }

            SetStatusLabel("Sedang mempersiapkan dll...");
            SetProgressBarValue(10);

            Thread.Sleep(2000);
            if (Game.Method == EInjectionMethod.E_EXTERNAL)
            {
                SetStatusLabel("Sedang mempersiapkan dll...");
                SetProgressBarValue(25);

                var injector = InjectionMethod.Create(InjectionMethodType.Standard);
                var img = new PortableExecutable("dwnData.txd");

                Thread.Sleep(1000);

                SetStatusLabel("Menunggu Game di Jalankan ( " + Game.Executeable + " )");
                SetProgressBarValue(50);

                string ProcessName = Game.Executeable.Substring(0, Game.Executeable.Length - 4);
                Process[] List = Process.GetProcessesByName(ProcessName);
                while (List.Count() == 0)
                {
                    List = Process.GetProcessesByName(ProcessName);
                    Thread.Sleep(10);
                }


                SetStatusLabel("Sedang menginject Dll...");
                SetProgressBarValue(75);

                var hModule = injector.Inject(img, List[0].Id);

                if (hModule != IntPtr.Zero)
                {
                    SetStatusLabel("Operasi Berhasil!");
                    SetProgressBarValue(100);
                    Thread.Sleep(500);
                    e.Result = 255;
                }
                else
                {
                    List[0].Kill();
                    if (injector.GetLastError() != null)
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(new Form() { TopMost = true }, injector.GetLastError().Message, "Inject Gagal! Harap pastikan anda Run As Admin tools ini!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                    e.Result = 0;
                }
            }
            else if (Game.Method == EInjectionMethod.E_INTERNAL_GAMEPATH)
            {
                SetStatusLabel("Sedang mempersiapkan dll...");
                SetProgressBarValue(25);

                bool isAbsolute = false;

                if (Path.IsPathRooted(Game.Path))
                {
                    isAbsolute = true;
                }

                Thread.Sleep(1000);
                if (Config.KeyExists("InstallPath", Game.Alias))
                {
                    string InstallPath = Framework.DecryptInstallPath(Config.Read("InstallPath", Game.Alias));
                    Framework.killLockingProcess(InstallPath);
                    Framework.UnlockFilePermission(InstallPath);
                    if (File.Exists(InstallPath))
                    {
                        SetStatusLabel("Sedang mengupdate dll yang telah terinstal");
                        SetProgressBarValue(50);
                        File.Copy("dwnData.txd", InstallPath, true);
                        File.SetLastAccessTime(InstallPath, new DateTime(2009, 7, 14, 8, 14, 20));
                        File.SetLastWriteTime(InstallPath, new DateTime(2009, 7, 14, 8, 14, 20));
                        File.SetCreationTime(InstallPath, new DateTime(2009, 7, 14, 8, 14, 20));                    
                        Config.Write("HackVer", Game.HackVersion.ToString(), Game.Alias);
                        Config.DeleteKey("md5", Game.Alias);
                        Config.Write("md5", Framework.GetMD5(InstallPath), Game.Alias);
                        Thread.Sleep(1000);
                        SetStatusLabel("Update Cheat Berhasil!");
                        SetProgressBarValue(100);
                        Thread.Sleep(500);
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "Install Cheat Berhasil!\nSilahkan Close tools dan Start game anda!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                        e.Result = 0;
                        return;
                    }
                }

                SetStatusLabel("Menunggu Game di Jalankan ( " + Game.Executeable + " )");
                SetProgressBarValue(50);

                string ProcessName = Game.Executeable.Substring(0, Game.Executeable.Length - 4);
                Process[] List = Process.GetProcessesByName(ProcessName);
                while (List.Count() == 0)
                {
                    List = Process.GetProcessesByName(ProcessName);
                    Thread.Sleep(10);
                }

                string FilePath = List[0].MainModule.FileName.Substring(0, List[0].MainModule.FileName.IndexOf(Game.Executeable));
                string Argument = GetCommandLine(List[0]);

                SetStatusLabel("Terminasi Proses...");
                SetProgressBarValue(60);
                List[0].Kill();

                Thread.Sleep(100);

                SetStatusLabel("Menyalin File...");
                SetProgressBarValue(60);

                string InstallFile = FilePath + Game.Path;
                string BackupFile = InstallFile + ".bak";

                if (isAbsolute)
                {
                    InstallFile = Game.Path;
                }

                Framework.killLockingProcess(InstallFile);
                Framework.UnlockFilePermission(InstallFile);

                if (!File.Exists(BackupFile) && File.Exists(InstallFile) && Game.RestoreFile == 1)
                {
                    File.Copy(InstallFile, BackupFile);
                }

                File.Copy("dwnData.txd", InstallFile, true);
                File.SetLastAccessTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                File.SetLastWriteTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                File.SetCreationTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                Config.Write("InstallPath", Framework.EncryptInstallPath(InstallFile), Game.Alias);
                Config.DeleteKey("HackVer", Game.Alias);
                Config.Write("HackVer", Game.HackVersion.ToString(), Game.Alias);
                Config.DeleteKey("md5", Game.Alias);
                Config.Write("md5", Framework.GetMD5(InstallFile), Game.Alias);

                SetStatusLabel("Install Cheat Berhasil!");
                SetProgressBarValue(100);

                Thread.Sleep(500);
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Install Cheat Berhasil! Anda sekarang dapat menjalankan Game tanpa membuka Tools ini!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
                e.Result = 0;
            }
            else if (Game.Method == EInjectionMethod.E_INTERNAL_AUTOSTART)
            {
                SetStatusLabel("Sedang mempersiapkan dll...");
                SetProgressBarValue(25);

                Thread.Sleep(1000);
                

                SetStatusLabel("Menunggu Game di Jalankan ( " + Game.Executeable + " )");
                SetProgressBarValue(50);

                string ProcessName = Game.Executeable.Substring(0, Game.Executeable.Length - 4);
                Process[] List = Process.GetProcessesByName(ProcessName);
                while (List.Count() == 0)
                {
                    List = Process.GetProcessesByName(ProcessName);
                    Thread.Sleep(10);
                }

                string FilePath = List[0].MainModule.FileName.Substring(0, List[0].MainModule.FileName.IndexOf(Game.Executeable));
                string Argument = GetCommandLine(List[0]);

                SetStatusLabel("Terminasi Proses...");
                SetProgressBarValue(60);
                List[0].Kill();

                Thread.Sleep(100);

                SetStatusLabel("Menyalin File...");
                SetProgressBarValue(70);

                string InstallFile = FilePath + Game.Path;
                string BackupFile = InstallFile + ".bak";

                if (!File.Exists(BackupFile) && File.Exists(InstallFile) && Game.RestoreFile == 1)
                {
                    File.Copy(InstallFile, BackupFile);
                }

                Framework.killLockingProcess(InstallFile);
                Framework.UnlockFilePermission(InstallFile);

                File.Copy("dwnData.txd", InstallFile, true);
                File.SetLastAccessTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                File.SetLastWriteTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                File.SetCreationTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));


                SetStatusLabel("Menjalankan ulang Proses...");
                SetProgressBarValue(80);

                using (var managementClass = new ManagementClass("Win32_Process"))
                {
                    var processInfo = new ManagementClass("Win32_ProcessStartup");
                    processInfo.Properties["CreateFlags"].Value = 0x00000008;

                    var inParameters = managementClass.GetMethodParameters("Create");
                    inParameters["CommandLine"] = Argument;
                    inParameters["CurrentDirectory"] = FilePath;
                    inParameters["ProcessStartupInformation"] = processInfo;

                    var result = managementClass.InvokeMethod("Create", inParameters, null);
                    if ((result == null) && ((uint)result.Properties["ReturnValue"].Value == 0))
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "Gagal menjalankan ulang Proses Game!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                        e.Result = 0;
                    }
                    else
                    {
                        SetStatusLabel("Start Cheat Berhasil!");
                        SetProgressBarValue(100);
                        Thread.Sleep(500);
                        e.Result = 255;
                    }
                }
                e.Result = 0;
            }
            else if (Game.Method == EInjectionMethod.E_INTERNAL_ABSOLUTE)
            {
                SetStatusLabel("Sedang mempersiapkan dll...");
                SetProgressBarValue(30);
                Thread.Sleep(1000);
                
                try
                {
                    string InstallFile = Game.Path;
                    string BackupFile = InstallFile + ".bak";

                    if (!File.Exists(BackupFile) && File.Exists(InstallFile) && Game.RestoreFile == 1)
                    {
                        File.Copy(InstallFile, BackupFile);
                    }
                    if (File.Exists(InstallFile))
                    {
                        Framework.killLockingProcess(InstallFile);
                        Framework.UnlockFilePermission(InstallFile);
                        try
                        {
                            File.Delete(InstallFile);
                        }
                        catch
                        {

                        }
                        SetStatusLabel("Sedang mengupdate dll yang telah terinstal");
                        SetProgressBarValue(50);
                        while (File.Exists(InstallFile))
                        {
                            string Random = Framework.RandomString(10);
                            while (File.Exists(InstallFile + "." + Random))
                            {
                                Random = Framework.RandomString(10);
                            }
                            File.Move(InstallFile, InstallFile + "." + Random);
                        }
                        File.Move("dwnData.txd", InstallFile);
                        File.SetLastAccessTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                        File.SetLastWriteTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                        File.SetCreationTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                        Config.DeleteKey("HackVer", Game.Alias);
                        Config.Write("HackVer", Game.HackVersion.ToString(), Game.Alias);
                        Config.DeleteKey("md5", Game.Alias);
                        Config.Write("md5", Framework.GetMD5(InstallFile), Game.Alias);
                        Thread.Sleep(1000);
                        if (Game.Property.ProtectOwnerToSystem)
                        {
                            Framework.RestoreFilePermission(InstallFile);
                        }
                        
                        SetStatusLabel("Update Cheat Berhasil!");
                        SetProgressBarValue(100);
                        Thread.Sleep(500);

                        
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "Update Cheat Berhasil!\nSilahkan Close tools dan Start game anda!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                        e.Result = 0;
                    }
                    else
                    {

                        Thread.Sleep(100);

                        SetStatusLabel("Menyalin File...");
                        SetProgressBarValue(50);
                        File.Move("dwnData.txd", InstallFile);
                        File.SetLastAccessTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                        File.SetLastWriteTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                        File.SetCreationTime(InstallFile, new DateTime(2009, 7, 14, 8, 14, 20));
                        Config.DeleteKey("HackVer", Game.Alias);
                        Config.Write("HackVer", Game.HackVersion.ToString(), Game.Alias);
                        Config.DeleteKey("md5", Game.Alias);
                        Config.Write("md5", Framework.GetMD5(Game.Path), Game.Alias);

                        Thread.Sleep(500);

                        SetStatusLabel("Install Cheat Berhasil!");
                        SetProgressBarValue(100);
                        Thread.Sleep(500);
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "Install Cheat Berhasil! Silahkan Close tools dan Start game anda!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                        e.Result = 0;
                    }
                }
                catch
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Gagal menginstall cheat! Harap tutup program lain sebelum menginstall cheat!", "Gagal!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                    e.Result = 0;
                }
            }
            else if (Game.Method == EInjectionMethod.E_EXTERNAL_STEALTH)
            {
                string ss = Path.GetRandomFileName();
                ss = Path.GetTempPath() + "\\" + ss.Replace(".", "") + ".exe";
                SetStatusLabel("Sedang menyalin file...");
                SetProgressBarValue(50);

                File.Copy("dwnData.txd", ss);
                File.Delete("dwnData.txd");

                var process = Process.Start(ss, Framework.Session.User.DownloadHash);
                if (!Game.Property.NoWaitExit)
                {
                    process.WaitForExit();
                    File.Delete(ss);
                    if (process.ExitCode == 0)
                    {
                        SetStatusLabel("Operasi Berhasil!");
                        SetProgressBarValue(100);
                        Thread.Sleep(500);
                        e.Result = 255;
                    }
                    else
                    {
                        SetStatusLabel("Gagal menjalankan cheat!");
                        SetProgressBarValue(100);
                        Thread.Sleep(500);
                        e.Result = 0;
                    }
                }
                else
                {
                    SetStatusLabel("Operasi Berhasil!");
                    SetProgressBarValue(100);
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Silahkan klik OK lalu jalankan game anda!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                    Thread.Sleep(500);
                    e.Result = 255;
                }

                
            }
        }

        private void HackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UserInfoGroupBox.Enabled = true;
            GameInfoGroupBox.Enabled = true;

            if (Framework.Session.Product.AllowHDSNChange == 1) HDSNChangeGroupBox.Enabled = true;
            GameInfoGroupBox.Enabled = true;
            GameSelectGroupBox.Enabled = true;

            SupportGroupBox.Enabled = true;
            AccountGroupBox.Enabled = true;

            File.Delete("dwnData.txd");

            SetStatusLabel("Menunggu");
            SetProgressBarValue(0);
            GameSelectComboBox_SelectedIndexChanged(GameSelectComboBox, new EventArgs());
            if ((int)e.Result == 255)
            {
                this.Close();
            }
        }

        private void HDSNChangeButton_Click(object sender, EventArgs e)
        {
            UserInfoGroupBox.Enabled = false;
            GameInfoGroupBox.Enabled = false;
            HDSNChangeGroupBox.Enabled = false;
            GameSelectGroupBox.Enabled = false;
            SupportGroupBox.Enabled = false;
            AccountGroupBox.Enabled = false;
            StatusGroupBox.Enabled = false;

            string ClientListStr = Framework.Session.User.ClientList;
            List<string> ClientList = ClientListStr.Split(';').ToList();
            List<string> NewClientList = new List<string>();
            for (int i = 0; i < Framework.Session.User.ClientCount; i++)
            {
                string MyHdsn;
                if (i >= ClientList.Count)
                {
                    MyHdsn = "";
                }
                else MyHdsn = ClientList[i];
                if (i == HDSNComboBox.SelectedIndex) MyHdsn = HDSNChangeTextBox.Text;
                if (MyHdsn.Length > 0)
                {
                    NewClientList.Add(MyHdsn);
                }
            }
            var Frm = new PasswordForm("Verifikasi Sandi", "OK");
            Frm.ShowDialog();
            string NewHdsn = String.Join(";", NewClientList) + ";";
            if (Framework.UpdateHDSN(NewHdsn, Frm.Password) == EServerResult.E_SUCCESS)
            {
                MainWorker_DoWork_1(MainWorker, new DoWorkEventArgs(null));
            }

            UserInfoGroupBox.Enabled = true;
            GameInfoGroupBox.Enabled = true;
            HDSNChangeGroupBox.Enabled = true;
            GameSelectGroupBox.Enabled = true;
            SupportGroupBox.Enabled = true;
            AccountGroupBox.Enabled = true;
            StatusGroupBox.Enabled = true;
        }

        private void ButtonDlDepedencies_Click(object sender, EventArgs e)
        {

            UserInfoGroupBox.Enabled = false;
            GameInfoGroupBox.Enabled = false;

            if (Framework.Session.Product.AllowHDSNChange == 1) HDSNChangeGroupBox.Enabled = false;
            GameInfoGroupBox.Enabled = false;
            GameSelectGroupBox.Enabled = false;

            SupportGroupBox.Enabled = false;
            AccountGroupBox.Enabled = false;

            SetStatusLabel("Sedang mempersiapkan ...");
            SetProgressBarValue(0);

            string szFileName = "Redist_" + Framework.Session.Product.Alias + ".exe";

            if (!File.Exists(szFileName))
            {
                string Depedencies = "https://www.xfiles.co/v5/files/XFilesHackFullDepedencies.exe";
                if (Framework.Session.Product.CustomRedist.Length > 0)
                {
                    Depedencies = Framework.Session.Product.CustomRedist;
                }
                WebClient RedistDownloader = new WebClient();
                Uri FileUrl = new Uri(Depedencies);
                RedistDownloader.Proxy = null;
                RedistDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressDownloadChanged);
                RedistDownloader.DownloadFileAsync(FileUrl, szFileName);
                RedistDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(InstallRedist_OnDownloadCompleted);
            }
            else InstallRedist_OnDownloadCompleted(null, new AsyncCompletedEventArgs(null, false, null));
        }

        private void InstallRedist_OnDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            SetStatusLabel("Jamu sukses di download. Silahkan Install Jamu pada jendela penginstalan");
            SetProgressBarValue(100);
            string szFileName = "Redist_" + Framework.Session.Product.Alias + ".exe";
            if (!e.Cancelled)
            {
                this.TopMost = false;
                // Use ProcessStartInfo class
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = szFileName;
                startInfo.UseShellExecute = true;

                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                }
                catch
                {
                    // Log error.
                }
                this.TopMost = true;
            }
            UserInfoGroupBox.Enabled = true;
            GameInfoGroupBox.Enabled = true;

            if (Framework.Session.Product.AllowHDSNChange == 1) HDSNChangeGroupBox.Enabled = true;
            GameInfoGroupBox.Enabled = true;
            GameSelectGroupBox.Enabled = true;

            SupportGroupBox.Enabled = true;
            AccountGroupBox.Enabled = true;
            SetStatusLabel("Menunggu");
            SetProgressBarValue(0);
        }

        private void ButtonChatSupport_Click(object sender, EventArgs e)
        {
            Process.Start(Framework.Session.Product.AdminLink);
        }

        private void ButtonPurchase_Click(object sender, EventArgs e)
        {
            var pForm = new PaymentForm(this.Framework);
            pForm.ShowDialog();
        }

        private void ButtonChangePwd_Click(object sender, EventArgs e)
        {
            var pwc = new PasswordChangeForm();
            pwc.ShowDialog();
            if (!string.IsNullOrEmpty(pwc.OldPassword))
            {
                if (Framework.UpdatePassword(pwc.OldPassword, pwc.NewPassword) == EServerResult.E_SUCCESS)
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Sukses mengganti kata sandi", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


    }
}
