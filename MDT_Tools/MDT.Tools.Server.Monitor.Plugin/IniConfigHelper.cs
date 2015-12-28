using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MDT.Tools.Server.Monitor.Plugin.Model;

namespace MDT.Tools.Server.Monitor.Plugin
{
    /// <summary>
    /// 获取 系统配置文件 ini
    /// </summary>
    public class IniConfigHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(
            string ApplicationName, string KeyName, string DefaultString,
            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, string FileName);
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
        public static S_ConfigInfo ReadConfigInfo()
        {

            S_ConfigInfo s_ConfigInfo = new S_ConfigInfo();

            StringBuilder sb = new StringBuilder(255); 
            GetPrivateProfileString("ConfigNum", "SoundAlert", "", sb, sb.Capacity, Consts.ConfigPath);                                   
            s_ConfigInfo.SoundAlert = sb.ToString();

            
           

            GetPrivateProfileString("ConfigNum", "SConfigInfoNum", "", sb, sb.Capacity, Consts.ConfigPath);
            int sNum = 0;
            if (int.TryParse(sb.ToString(), out sNum))
            {
                s_ConfigInfo.SConfigInfoNum = sNum;
            }

            GetPrivateProfileString("ConfigNum", "Show", "", sb, sb.Capacity, Consts.ConfigPath);
            bool show = true;
            if (bool.TryParse(sb.ToString(), out show))
            {
                s_ConfigInfo.Show = show;
            }


            for (int i = 1; i <= sNum; i++)
            {
                ConfigInfo configInfo = new ConfigInfo();
                //name
                configInfo.SMonitorName=ReadIniCN("Config", "SMonitorName" + i, Consts.ConfigPath);
                
                //ip
                GetPrivateProfileString("Config", "Ip" + i, "", sb, sb.Capacity, Consts.ConfigPath);
                configInfo.Ip = sb.ToString();
                //port
                GetPrivateProfileString("Config", "Port" + i, "", sb, sb.Capacity, Consts.ConfigPath);
                int portNm = 19990;
                int.TryParse(sb.ToString(), out portNm);
                configInfo.Port = portNm;
                s_ConfigInfo.ConfigInfo.Add(configInfo);
            }
            return s_ConfigInfo;
        }









    }
}
