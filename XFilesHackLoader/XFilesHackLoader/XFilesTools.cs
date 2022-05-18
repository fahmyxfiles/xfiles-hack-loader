using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using RestSharp;
using RestSharp.Deserializers;
using System.Windows.Forms;
using XFilesHackLoader.Properties;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Management;
using XFilesHackLoader;


namespace XFilesFramework
{
    public class SimpleWebProxy : IWebProxy
    {
        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return destination;
        }

        public bool IsBypassed(Uri host)
        {
            // if return true, service will be very slow.
            return false;
        }

        private static SimpleWebProxy defaultProxy = new SimpleWebProxy();
        public static SimpleWebProxy Default
        {
            get
            {
                return defaultProxy;
            }
        }
    }
    public enum EServerResult
    {
        E_FAILED = 0x00000000,
        E_SUCCESS = 0x00000001,
    };
    public enum EInjectionMethod
    {
        E_EXTERNAL = 1,
        E_INTERNAL_GAMEPATH = 2,
        E_EXTERNAL_GAMEPATH = 3,
        E_INTERNAL_ABSOLUTE = 4,
        E_EXTERNAL_LINK = 5,
        E_EXTERNAL_STEALTH = 6,
    };
    public enum EGameStatus
    {
        E_ACTIVE = 1,
        E_DISABLED = 2,
        E_MAINTENANCE = 3,
        E_ACTIVE_SHOWMSG = 4,
    };
    public enum EPGType
    {
        E_BANKING = 1,
        E_MOBILECREDIT = 2,
    };
    public class RequestResult
    {
        public EServerResult Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    };

    public class UserData
    {
        public int id { get; set; }
        public string Product { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
        public int UplinkId { get; set; }
        public string DownloadHash { get; set; }
        public string LicenseKey { get; set; }
        public string GameId { get; set; }
        public int ClientCount { get; set; }
        public string ClientList { get; set; }
        public int Status { get; set; }
        public int Duration { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime LastAccessTime { get; set; }
    };
    public class GameData
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Publisher { get; set; }
        public string Executeable { get; set; }
        public int HackVersion { get; set; }
        public EInjectionMethod InjectionMethod { get; set; }
        public string DownloadUrl { get; set; }
        public string InjectionOption { get; set; }
        public EGameStatus Status { get; set; }
        public string Message { get; set; }
        public DateTime LastUpdated { get; set; }
    };
    public class ProductData
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Identity { get; set; }
        public string GameList { get; set; }
        public string LicenseCallback { get; set; }
        public string AdminLink { get; set; }
        public string Flags { get; set; }
        public int AllowHDSNChange { get; set; }
        public int AllowInternalSubs { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    };
    public class SessionData
    {
        public UserData User { get; set; }
        public ProductData Product { get; set; }
        public List<GameData> GameList { get; set; }
        public string WelcomeMessage { get; set; }
    };

    public class PackageData
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string AllowedGames { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
        public int ClientLimit { get; set; }
    };
    public class PaymentGatewayData
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public EPGType CType { get; set; }
        public string Alias { get; set; }
        public int NeedSender { get; set; }
    };
    public class PathList
    {
        public string GarenaMessenger { get; set; }
        public string PBID { get; set; }
    };
    public class XFramework
    {
        
        RestClient Client;
        JsonDeserializer JsDeserializer;
        public SessionData Session;
        public List<PackageData> PackageList;
        public List<PaymentGatewayData> PaymentGatewayList;
        public bool IsLoggedIn;
        public string InternalPassword;
        public PathList PathData;
        public XFramework()
        {
            Client = new RestClient("http://www.xfiles.co/v5/");
            Client.CookieContainer = new CookieContainer();
            Client.Proxy = SimpleWebProxy.Default;
            JsDeserializer = new JsonDeserializer();
            PathData = new PathList();
            IsLoggedIn = false;
            string GarenaConfigIni = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\GarenaMessenger\\app.ini";
            if (File.Exists(GarenaConfigIni))
            {
                IniParser Parser = new IniParser(GarenaConfigIni);
                PathData.GarenaMessenger = GetExactPathName(Parser.Read("im", "game"));
                PathData.PBID = GetExactPathName(Parser.Read("pbid", "game") + "\\apps\\PBID");
                if (!Directory.Exists(PathData.PBID) || !Directory.Exists(PathData.GarenaMessenger))
                {
                    MessageBox.Show("Perhatian: Gagal memeriksa Direktory Game, pastikan Game anda telah terinstall dan bisa di jalankan secara normal!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Perhatian: Tidak dapat membaca konfigurasi GarenaMessenger, pastikan GarenaMessenger telah terinstall di PC anda!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static string ClientLicenseUnlockKey = "f3abb28c84ed0efaa42be9851d41bf46";
        public static string ClientUnlockPathKey = "8fa52db287744e6cd87a1a6034913763";
        public static int ptr_Identity = 0x6E;
        public static int ptr_LicenseKey = 0x4E;
        public static byte[] exclusiveOR(byte[] arr1, string Key)
        {
            byte[] arr2 = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            byte[] result = new byte[arr1.Length];

            for (int i = 0; i < arr1.Length; ++i)
                result[i] = (byte)(arr1[i] ^ arr2[i % arr2.Length]);

            return result;
        }
        public string GetMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    byte[] result = md5.ComputeHash(stream);
                    return BitConverter.ToString(result).Replace("-", string.Empty).ToLower();
                }
            }
        }
        public bool StartProcess(string Path, string Argument)
        {
            using (var managementClass = new ManagementClass("Win32_Process"))
            {
                var processInfo = new ManagementClass("Win32_ProcessStartup");
                processInfo.Properties["CreateFlags"].Value = 0x00000008;

                var inParameters = managementClass.GetMethodParameters("Create");
                inParameters["CommandLine"] = Argument;
                inParameters["CurrentDirectory"] = Path;
                inParameters["ProcessStartupInformation"] = processInfo;

                var result = managementClass.InvokeMethod("Create", inParameters, null);
                if ((result == null) && ((uint)result.Properties["ReturnValue"].Value == 0))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
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
        [DllImport("shell32.dll")]
        public static extern bool SHGetSpecialFolderPath(
            IntPtr hwndOwner, [Out]StringBuilder lpszPath, int nFolder, bool fCreate
        );
        public static void AddFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {

            try
            {
                // Get a FileSecurity object that represents the
                // current security settings.
                FileSecurity fSecurity = File.GetAccessControl(fileName);

                // Add the FileSystemAccessRule to the security settings.
                fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                    rights, controlType));

                // Set the new access settings.
                File.SetAccessControl(fileName, fSecurity);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }
        public static void UnlockFile(string filepath)
        {
            ExecuteCommand("takeown /f \"" + filepath + "\"");
            ExecuteCommand("echo y| cacls \"" + filepath + "\" /G \"" + Environment.UserName + "\":f");
        }
        public void killLockingProcess(string Filepath)
        {
            Process[] pr = Process.GetProcesses();
            foreach (Process prt in pr)
            {
                try
                {
                    ProcessModuleCollection myProcessModuleCollection = prt.Modules;
                    foreach (ProcessModule prm in myProcessModuleCollection)
                    {
                        if (Path.GetFileName(prm.FileName.ToLower()) == Path.GetFileName(Filepath.ToLower()))
                        {
                            prt.Kill();
                            continue;
                        }
                    }
                }
                catch(Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    if (e.ToString().Contains("Access is Denied") && prt.ProcessName != "System" && prt.ProcessName != "Idle")
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Gagal membunuh Proses !\nNama Proses : " + prt.ProcessName + ".exe\nHarap Uninstall jika process tersebut adalah milik antivirus anda lalu install cheat lagi", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public static string GetExactPathName(string pathName)
        {
            if (!(File.Exists(pathName) || Directory.Exists(pathName)))
                return pathName;

            var di = new DirectoryInfo(pathName);

            if (di.Parent != null)
            {
                return Path.Combine(
                    GetExactPathName(di.Parent.FullName),
                    di.Parent.GetFileSystemInfos(di.Name)[0].Name);
            }
            else
            {
                return di.Name.ToUpper();
            }
        }
        public static string GetSystemDirectory()
        {
            StringBuilder path = new StringBuilder(260);
            SHGetSpecialFolderPath(IntPtr.Zero, path, 0x0029, false);
            return path.ToString();
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public string DecryptInstallPath(string EncryptedInstallPath)
        {
            byte[] InstallPathB = System.Text.Encoding.UTF8.GetBytes(Base64Decode(EncryptedInstallPath));
            byte[] InstallPathExB = exclusiveOR(InstallPathB, ClientUnlockPathKey);
            string InstallPath = System.Text.Encoding.UTF8.GetString(InstallPathExB);
            return InstallPath;
        }
        public string EncryptInstallPath(string RawInstallPath)
        {
            byte[] InstallPathB = System.Text.Encoding.UTF8.GetBytes(RawInstallPath);
            byte[] InstallPathExB = exclusiveOR(InstallPathB, ClientUnlockPathKey);
            string InstallPath = System.Text.Encoding.UTF8.GetString(InstallPathExB);
            return Base64Encode(InstallPath);
        }
        public string GetLicenseKey(byte[] SelfData)
        {
            //return "iTGv1z6mszHVRuToRQcShMjYR1hw3q1ljIRlhXIJ";
            byte[] fileStream = new byte[50];
            Array.Copy(SelfData, (SelfData.Length - ptr_LicenseKey), fileStream, 0, 50);
            string str = System.Text.Encoding.ASCII.GetString(exclusiveOR(fileStream, ClientLicenseUnlockKey)).Substring(10);
            return str;
            
        }
        public string GetProductId(byte[] SelfData)
        {
            //return "4f4d24340881081aca7f62220b74f3dd";
            byte[] fileStream = new byte[32];
            Array.Copy(SelfData, (SelfData.Length - ptr_Identity), fileStream, 0, 32);
            string str = System.Text.Encoding.ASCII.GetString(fileStream).Substring(0, 32);
            return str;
            
        }

        public bool CleanGarenaPatch()
        {
            try
            {
                string PatchTarget = PathData.GarenaMessenger + "\\dbghelp.dll";
                if (File.Exists(PatchTarget))
                {

                    Process[] procList = Process.GetProcessesByName("ggdllhost");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    procList = Process.GetProcessesByName("GarenaMessenger");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    procList = Process.GetProcessesByName("PointBlank");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    procList = Process.GetProcessesByName("pbid");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    try
                    {
                        File.Delete(PatchTarget);
                    }
                    catch
                    {
                        return true;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(new Form() { TopMost = true }, "Warning! Gagal melakukan patch pada sistem Garena+\nKemungkinan Cheat tidak akan berjalan normal\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }
        }

        public bool PreInstallGarenaGames()
        {
            try
            {
                string PatchTarget = PathData.GarenaMessenger + "\\dbghelp.dll";
                string PatchData = Directory.GetCurrentDirectory() + "\\patchData.txd";

                WebClient patchDownloader = new WebClient();
                Uri FileUrl = new Uri("https://www.xfiles.co/v5/files/ggpatch/6e2378d9d041e8870121d9953693475b");
                patchDownloader.Proxy = null;
                patchDownloader.DownloadFile(FileUrl, PatchData);
                File.WriteAllText(PathData.GarenaMessenger + "\\prl.ini", Process.GetCurrentProcess().ProcessName + ".exe");
                bool doPatchTarget = true;
                if (File.Exists(PatchTarget))
                {
                    string md5Target = GetMD5(PatchTarget);
                    string md5Current = GetMD5(PatchData);
                    if (md5Target == md5Current)
                    {
                        doPatchTarget = false;
                        File.Delete(PatchData);
                    }
                }


                if (doPatchTarget)
                {
                    // Kill Garena+
                    Process [] procList = Process.GetProcessesByName("ggdllhost");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    procList = Process.GetProcessesByName("GarenaMessenger");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    procList = Process.GetProcessesByName("PointBlank");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    procList = Process.GetProcessesByName("pbid");
                    foreach (Process prc in procList)
                    {
                        try
                        {
                            prc.Kill();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    Thread.Sleep(500);
                    // Start Patch
                    if (File.Exists(PatchTarget))
                    {
                        File.Delete(PatchTarget);
                    }
                    File.Move(PatchData, PatchTarget);
                    Process.Start(PathData.GarenaMessenger + "\\GarenaMessenger.exe");
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(new Form() { TopMost = true }, "Warning! Gagal melakukan patch pada sistem Garena+\nKemungkinan Cheat tidak akan berjalan normal\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }
        }

        public bool PreInstallPBID()
        {
            Process[] procList = Process.GetProcessesByName("ggdllhost");
            foreach (Process prc in procList)
            {
                try
                {
                    prc.Kill();
                }
                catch
                {
                    continue;
                }
            }
            procList = Process.GetProcessesByName("GarenaMessenger");
            foreach (Process prc in procList)
            {
                try
                {
                    prc.Kill();
                }
                catch
                {
                    continue;
                }
            }
            procList = Process.GetProcessesByName("PointBlank");
            foreach (Process prc in procList)
            {
                try
                {
                    prc.Kill();
                }
                catch
                {
                    continue;
                }
            }
            procList = Process.GetProcessesByName("pbid");
            foreach (Process prc in procList)
            {
                try
                {
                    prc.Kill();
                }
                catch
                {
                    continue;
                }
            }
            return true;   
        }
        
        public EServerResult Login(string Password)
        {
            byte[] SelfData = File.ReadAllBytes(Application.ExecutablePath);
            string LicenseKey = this.GetLicenseKey(SelfData);
            string ProductId = this.GetProductId(SelfData);
            var Request = new RestRequest("XFilesTools/User", Method.POST);
            Request.AddParameter("ProductId", (object)ProductId);
            Request.AddParameter("LicenseKey", (object)LicenseKey);
            Request.AddParameter("Password", (object)Password);

            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_SUCCESS)
            {
                Response.Content = Result.Data;
                Session = JsDeserializer.Deserialize<SessionData>(Response);
                InternalPassword = Password;
                IsLoggedIn = true;
                if (Session.WelcomeMessage.Length > 0)
                {
                    MessageBox.Show(new Form() { TopMost = true }, Session.WelcomeMessage, "Pesan Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                for (int i = 0; i < Session.GameList.Count; i++)
                {
                    if (Session.GameList[i].InjectionOption.Contains("%SYSDIR%"))
                    {
                        Session.GameList[i].InjectionOption = Session.GameList[i].InjectionOption.Replace("%SYSDIR%", GetSystemDirectory());
                        if (File.Exists(Session.GameList[i].InjectionOption))
                        {
                            UnlockFile(Session.GameList[i].InjectionOption);
                            killLockingProcess(Session.GameList[i].InjectionOption);
                        }
                    }
                    if (Session.GameList[i].InjectionOption.Contains("%PATH_PBID%"))
                    {
                        Session.GameList[i].InjectionOption = Session.GameList[i].InjectionOption.Replace("%PATH_PBID%", PathData.PBID);
                    }
                }

            }
            else if (Result.Status == EServerResult.E_FAILED)
            {
                MessageBox.Show(new Form() { TopMost = true }, Result.Message, "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Status;
        }

        public EServerResult UpdateHDSN(string NewHdsn)
        {
            var Request = new RestRequest("XFilesTools/UpdateHdsn", Method.POST);
            Request.AddParameter("NewHdsn", NewHdsn);
            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_FAILED)
            {
                MessageBox.Show(new Form() { TopMost = true }, Result.Message, "Ganti HDSN Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Status;
        }
        public EServerResult UpdatePassword(string OldPassword, string NewPassword)
        {
            var Request = new RestRequest("XFilesTools/UpdatePassword", Method.POST);
            Request.AddParameter("OldPassword", OldPassword);
            Request.AddParameter("NewPassword", NewPassword);
            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_SUCCESS)
            {
                InternalPassword = NewPassword;
            }
            else if (Result.Status == EServerResult.E_FAILED)
            {
                MessageBox.Show(new Form() { TopMost = true }, Result.Message, "Ganti Kata Sandi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Status;
        }
        public EServerResult GetPackagesList()
        {
            var Request = new RestRequest("XFilesTools/ipg/GetSubsPackage", Method.POST);
            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_SUCCESS)
            {
                Response.Content = Result.Data;
                PackageList = JsDeserializer.Deserialize<List<PackageData>>(Response);
            }
            else if (Result.Status == EServerResult.E_FAILED)
            {
                MessageBox.Show(new Form() { TopMost = true }, Result.Message, "Permintaan Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Status;
        }
        public EServerResult GetPaymentGatewayList()
        {
            var Request = new RestRequest("XFilesTools/ipg/GetPGList", Method.POST);
            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_SUCCESS)
            {
                Response.Content = Result.Data;
                PaymentGatewayList = JsDeserializer.Deserialize<List<PaymentGatewayData>>(Response);
            }
            else if (Result.Status == EServerResult.E_FAILED)
            {
                MessageBox.Show(new Form() { TopMost = true }, Result.Message, "Permintaan Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Status;
        }
        public EServerResult RequestPayment(string Package, int Duration, int ClientCount, string Gateway, string Sender)
        {
            var Request = new RestRequest("XFilesTools/ipg/RequestPayment", Method.POST);
            Request.AddParameter("Package", Package);
            Request.AddParameter("Duration", Duration);
            Request.AddParameter("ClientCount", ClientCount);
            Request.AddParameter("Gateway", Gateway);
            Request.AddParameter("Sender", Sender);
            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_SUCCESS)
            {
                string Filename = "P-" + Result.Message + ".txt";
                File.WriteAllText(Filename, Result.Data);
                MessageBox.Show(new Form() { TopMost = true }, "Permintaan berhasil! Silahkan melihat petunjuk pembayaran di file \"" + Filename + "\" pada folder tools. Kami akan membukanya untuk anda", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(Filename);
            }
            else if (Result.Status == EServerResult.E_FAILED)
            {
                MessageBox.Show(new Form() { TopMost = true }, Result.Message, "Permintaan Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Status;
        }

    };
};
