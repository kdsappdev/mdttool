using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Utils
{
   public class ReadFile
    {
        private static readonly string monitorPath = System.Windows.Forms.Application.StartupPath + "\\control\\AlyunMonitor.ini";
        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key,
            string def, byte[] retVal, int size, string filePath);

        #region ReadIniData
        public static string ReadIniData(string Section, string Key, string NoText)
        {
            if (File.Exists(monitorPath))
            {
                try
                {
                    StringBuilder temp = new StringBuilder(10000);
                    GetPrivateProfileString(Section, Key, NoText, temp, 255, monitorPath);
                    return temp.ToString();
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }


        public static string getIniStr(string Section, string Key, string NoText)
        {
            if (File.Exists(monitorPath))
            {
                Byte[] Buffer = new Byte[10000];
                int bufLen = GetPrivateProfileString(Section, Key, NoText, Buffer, Buffer.GetUpperBound(0), monitorPath);
                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                string s = enc.GetString(Buffer);
                s= s.Replace("\0","");
                return s.Trim();
            }
            else
            {
                return "";
            }
        }

        #endregion

    }
}
