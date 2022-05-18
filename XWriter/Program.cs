using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace XWriter
{
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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            byte[] Data = File.ReadAllBytes("XFilesHackLoader.exe");
            File.WriteAllBytes("main.txs", iXorWriter.XorIt(Data, Key._data));
        }
    }
}
