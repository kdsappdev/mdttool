using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MDT.Tools.ExcelAddin.Utils;
//using Fiddler.Addin.IBA.Model;
//using Fiddler.Addin.IBA.Out;

namespace MDT.Tools.ExcelAddin
{
    /// <summary>
    /// 获取 系统配置文件 ini
    /// </summary>
    public class IniConfigHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(
            string ApplicationName, string KeyName, string DefaultString,
            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, string FileName);

        //转码
        public static string ReadIniCN(string lpApplicationName, string lpKeyName, string iniPath)
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


        public static ConfigInfo ReadConfigInfo()
        {
            ConfigInfo configInfo = new ConfigInfo();
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString("Data", "Header", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.Header = sb.ToString();
            GetPrivateProfileString("Config", "OutFileName", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.OutFileName = sb.ToString();
            GetPrivateProfileString("Data", "RowsCount", "", sb, sb.Capacity, Consts.ConfigPath);
            int rowsCount = 0;
            if (int.TryParse(sb.ToString(), out rowsCount))
            {
                configInfo.RowsCount = rowsCount;
                for (int i = 1; i <= configInfo.RowsCount; i++)
                {
                    GetPrivateProfileString("Data", "Row" + i, "", sb, sb.Capacity, Consts.ConfigPath);
                    configInfo.Rows.Add(sb.ToString());
                    LogHelper.Info(sb.ToString());
                }
            }
            GetPrivateProfileString("Config", "MenuName", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.MenuName = sb.ToString();
            GetPrivateProfileString("Config", "OutPath", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.OutPath = sb.ToString();
            GetPrivateProfileString("Quartz", "Interval", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.VkInterval = sb.ToString();
            bool temp = true;
            GetPrivateProfileString("Log", "Logon", "", sb, sb.Capacity, Consts.ConfigPath);
            if (bool.TryParse(sb.ToString(), out temp))
            {
                configInfo.Logon = temp;
            }



            GetPrivateProfileString("MqDevice", "Host", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.Host = sb.ToString();

            GetPrivateProfileString("MqDevice", "Port", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.Port = sb.ToString();

            GetPrivateProfileString("MqDevice", "Protocal", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.Protocal = sb.ToString();

            GetPrivateProfileString("MqDevice", "ServerName", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.ServerName = sb.ToString();

            GetPrivateProfileString("MqDevice", "Topic", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.Topic = sb.ToString();

            GetPrivateProfileString("FileDevice", "FileName", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.FileName = sb.ToString();


            GetPrivateProfileString("Smtp", "Host", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.SHost = sb.ToString();
            GetPrivateProfileString("Smtp", "Port", "", sb, sb.Capacity, Consts.ConfigPath);
            int tp = 0;
            if (int.TryParse(sb.ToString(), out tp))
            {
                configInfo.SPort = tp;
            }

            GetPrivateProfileString("Smtp", "EnableSsl", "", sb, sb.Capacity, Consts.ConfigPath);
            if (bool.TryParse(sb.ToString(), out temp))
            {
                configInfo.EnableSsl = temp;
            }

            GetPrivateProfileString("Smtp", "UserName", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.UserName = sb.ToString();
            GetPrivateProfileString("Smtp", "UserPwd", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.UserPwd = sb.ToString();
            //主题
            configInfo.Subject = ReadIniCN("Smtp", "Subject", Consts.ConfigPath);

            GetPrivateProfileString("Mail", "From", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.From = sb.ToString();
            GetPrivateProfileString("Mail", "To", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.To = sb.ToString();
            GetPrivateProfileString("Mail", "Cc", "", sb, sb.Capacity, Consts.ConfigPath);
            configInfo.Cc = sb.ToString();


            return configInfo;

        }
    }
}
