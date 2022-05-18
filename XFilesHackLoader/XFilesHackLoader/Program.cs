using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using XFilesHackLoader.Properties;

namespace XFilesHackLoader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new XFilesMain());
        }
        static Assembly CurrentDomain_AssemblyResolve(object s, ResolveEventArgs a)
        {
            if (a.Name.Substring(0, a.Name.IndexOf(",")) == "RestSharp")
            {
                return Assembly.Load(Resources.RestSharp);
            }

            if (a.Name.Substring(0, a.Name.IndexOf(",")) == "JLibrary")
            {
                return Assembly.Load(Resources.JLibrary);
            }

            return null;
        }   
    }
}
