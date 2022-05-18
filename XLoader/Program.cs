using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.Threading;
namespace XLoader
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
    public class iXorWriter
    {
        public static byte[] XorIt(byte[] arr1, byte[] arr2)
        {

            byte[] result = new byte[arr1.Length];

            for (int i = 0; i < arr1.Length; ++i)
                result[i] = (byte)(arr1[i] ^ arr2[i % arr2.Length]);

            return result;
        }
    }

    static class Program
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.xfiles.co"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex mutex = new System.Threading.Mutex(false, "XFilesHack_V5R1");
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    /*
                    if (CheckForInternetConnection() == false)
                    {
                        
                    }
                     * */
                    WebClient WC = new WebClient();
                    WC.Proxy = SimpleWebProxy.Default;
                    Uri FileUrl = new Uri("http://www.xfiles.co/v5/files/main.txs");
                    byte[] data = null;
                    try
                    {
                        data = WC.DownloadData(FileUrl);
                    }
                    catch (WebException e)
                    {
                        MessageBox.Show("Gagal menghubungkan ke internet, Harap periksa koneksi Internet anda.\nApabila normal, kemungkinan server sedang down. Silahkan tunggu beberapa saat lagi\nError : " + e.ToString(), "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    byte[] Res = iXorWriter.XorIt(data, Key._data);
                    Assembly exeAssembly = Assembly.Load(Res);
                    exeAssembly.EntryPoint.Invoke(null, new object[] { });
                }
                else
                {
                    MessageBox.Show("Loader sudah berjalan.\nApabila tidak ada tampilan muncul coba terminate process di task manager.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.Close();
                    mutex = null;
                }
            }
            
        }
    }
}
