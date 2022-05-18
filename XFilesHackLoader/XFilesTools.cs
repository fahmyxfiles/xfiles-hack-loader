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
using System.Security.Permissions;
using System.Threading;
using XFilesHackLoader;
using ProcessPrivileges;



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
        E_INTERNAL_AUTOSTART = 3,
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
        public class GamePropertyData
        {
            public GamePropertyData (string PropertiesString)
            {
                // Set All To False
                this.ProtectOwnerToSystem = false;
                this.NoWaitExit = false;

                List<string> sList = PropertiesString.Split(';').ToList();
                foreach (string Values in sList)
                {
                    if (Values == "PROTECT_OWNER")
                    {
                        this.ProtectOwnerToSystem = true;
                    }
                    if (Values == "NO_WAIT")
                    {
                        this.NoWaitExit = true;
                    }
                }
            }
            public bool ProtectOwnerToSystem { get; set; }
            public bool NoWaitExit { get; set; }
        };
        public int id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Publisher { get; set; }
        public string Executeable { get; set; }
        public int HackVersion { get; set; }
        public EInjectionMethod Method { get; set; }
        public int RestoreFile { get; set; }
        public string CleanFile { get; set; }
        public List<string> CleanFileList { get; set; }
        public string DownloadUrl { get; set; }
        public string Path { get; set; }
        public string Properties { get; set; }
        public GamePropertyData Property { get; set; }
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
        public string CustomRedist { get; set; }
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
        public void RestoreFilePermission(string filepath)
        {
            ExecuteCommand("takeown /f \"" + filepath + "\"");
            ExecuteCommand("icacls \"" + filepath + "\" /grant \"" + Environment.UserName + "\":(F) /t");
            ExecuteCommand("icacls \"" + filepath + "\" /grant \"Administrators\":(RX) /t");
            ExecuteCommand("icacls \"" + filepath + "\" /grant \"Users\":(RX) /t");
            ExecuteCommand("icacls \"" + filepath + "\" /grant \"SYSTEM\":(RX) /t");
            ExecuteCommand("icacls \"" + filepath + "\" /grant \"NT SERVICE\\TrustedInstaller\":(F) /t");
            ExecuteCommand("icacls \"" + filepath + "\" /inheritance:r");
            ExecuteCommand("icacls \"" + filepath + "\" /setowner \"NT SERVICE\\TrustedInstaller\" /t");
            ExecuteCommand("icacls \"" + filepath + "\" /remove \"" + Environment.UserName + "\"");
        }
        public void UnlockFilePermission(string filepath)
        {
            ExecuteCommand("takeown /f \"" + filepath + "\"");
            ExecuteCommand("icacls \"" + filepath + "\" /grant \""+Environment.UserName+"\":(F) /t");

        }
        public string generateRandomString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }
        public string RandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
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
                            if (prt.ProcessName != Process.GetCurrentProcess().ProcessName)
                            {
                                prt.Kill();
                                continue;
                            }
                            else continue;
                        }
                    }
                }
                catch(Exception e)
                {
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
            //return "KFEWz47mGTlKSdPyJKJU1OBwvQmIsMAcrf81kgo0";
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

        public int FindPattern(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            int found = -1;
            bool matched = false;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            matched = true;
                            for (int y = 1; y <= searchBytes.Length - 1; y++)
                            {
                                if (searchIn[i + y] != searchBytes[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }

                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }

            }
            return found;
        }
        
        public EServerResult Login()
        {
            byte[] SelfData = File.ReadAllBytes(Application.ExecutablePath);
            string LicenseKey = this.GetLicenseKey(SelfData);
            string ProductId = this.GetProductId(SelfData);
            var Request = new RestRequest("XFilesTools/User", Method.POST);
            Request.AddParameter("ProductId", (object)ProductId);
            Request.AddParameter("LicenseKey", (object)LicenseKey);

            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_SUCCESS)
            {
                Response.Content = Result.Data;
                Session = JsDeserializer.Deserialize<SessionData>(Response);
                IsLoggedIn = true;
                if (Session.WelcomeMessage.Length > 0)
                {
                    MessageBox.Show(new Form() { TopMost = true }, Session.WelcomeMessage, "Pesan Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                for (int i = 0; i < Session.GameList.Count; i++)
                {
                    Session.GameList[i].Path = Session.GameList[i].Path.Replace("%SYSDIR%", GetSystemDirectory());
                    Session.GameList[i].Path = Session.GameList[i].Path.Replace("%PATH_PBID%", PathData.PBID);
                    Session.GameList[i].CleanFile = Session.GameList[i].CleanFile.Replace("%SYSDIR%", GetSystemDirectory());
                    Session.GameList[i].CleanFile = Session.GameList[i].CleanFile.Replace("%PATH_PBID%", PathData.PBID);
                    Session.GameList[i].CleanFileList = Session.GameList[i].CleanFile.Split(';').ToList();
                    Session.GameList[i].Property = new GameData.GamePropertyData(Session.GameList[i].Properties);
                }

            }
            else if (Result.Status == EServerResult.E_FAILED)
            {
                MessageBox.Show(new Form() { TopMost = true }, Result.Message, "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Status;
        }

        public EServerResult UpdateHDSN(string NewHdsn, string Password)
        {
            var Request = new RestRequest("XFilesTools/UpdateHdsn", Method.POST);
            Request.AddParameter("NewHdsn", (object)NewHdsn);
            Request.AddParameter("Password", (object)Password);
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
            Request.AddParameter("OldPassword", (object)OldPassword);
            Request.AddParameter("NewPassword", (object)NewPassword);
            IRestResponse Response = Client.Execute(Request);
            RequestResult Result = JsDeserializer.Deserialize<RequestResult>(Response);
            if (Result.Status == EServerResult.E_FAILED)
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
            Request.AddParameter("Package", (object)Package);
            Request.AddParameter("Duration", (object)Duration);
            Request.AddParameter("ClientCount", (object)ClientCount);
            Request.AddParameter("Gateway", (object)Gateway);
            Request.AddParameter("Sender", (object)Sender);
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
