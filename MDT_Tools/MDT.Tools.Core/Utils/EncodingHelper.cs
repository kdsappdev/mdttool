using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace MDT.Tools.Core.Utils
{
    public class EncodingHelper
    {
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(
         string ApplicationName, string KeyName, string DefaultString,
         [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, string FileName);

        private static string config = Application.StartupPath + "\\control\\encoding.ini";
        public static string TargetEncoding { get; set; }
        public static string OriginalEncoding { get; set; }
        static EncodingHelper()
        {
            TargetEncoding = ReadIniCN("Encoding", "TargetEncoding", config);
            OriginalEncoding = ReadIniCN("Encoding", "OriginalEncoding", config);
        }

        private static string ReadIniCN(string lpApplicationName, string lpKeyName, string iniPath)
        {
            string str = "";
            int size = 260;
            byte[] buff = new byte[size];
            int ret = GetPrivateProfileString(lpApplicationName, lpKeyName, "", buff, size, iniPath);
            if (ret > 0)
            {
                byte[] arry = new byte[ret];
                for (int i = 0; i < ret; i++)
                {
                    arry[i] = buff[i];
                }
                str = System.Text.Encoding.UTF8.GetString(arry);
            }
            return str;
        }
        public static string ConvertEncoder(Encoding originalEncoding, Encoding targetEncoding, string str)
        {
            string temp = str;

            if (originalEncoding != null && targetEncoding != null && !string.IsNullOrEmpty(temp))
            {
                Byte[] b = originalEncoding.GetBytes(temp);
                temp = targetEncoding.GetString(b);
            }
            return temp;
        }
        public static string ConvertEncoder(string originalEncoding, string targetEncoding, string str)
        {
            string temp = str;

            if (!string.IsNullOrEmpty(originalEncoding)&& !string.IsNullOrEmpty(targetEncoding) && !string.IsNullOrEmpty(temp))
            {
                ConvertEncoder(Encoding.GetEncoding(originalEncoding), Encoding.GetEncoding(targetEncoding), str);
            }
            return temp;
        }
    }
}
