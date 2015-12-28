using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MDT.Tools.User.Monitor.Plugin
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

        public static PFS_ConfigInfo ReadConfigInfo()
        {

            PFS_ConfigInfo fps_ConfigInfo = new PFS_ConfigInfo();

            StringBuilder sb = new StringBuilder(255); 
            GetPrivateProfileString("UserMonitor", "SoundAlert", "", sb, sb.Capacity, Consts.ConfigPath);                                   
            fps_ConfigInfo.SoundAlert = sb.ToString();

            GetPrivateProfileString("UserMonitor", "AlertState", "", sb, sb.Capacity, Consts.ConfigPath);
            int Num = 0;
            if (int.TryParse(sb.ToString(), out Num))
            {
                fps_ConfigInfo.AlertState = Num;
            }

            GetPrivateProfileString("UserMonitor", "PFSConfigInfoNum", "", sb, sb.Capacity, Consts.ConfigPath);
            int pfsNum = 0;
            if (int.TryParse(sb.ToString(), out pfsNum))
            {
                fps_ConfigInfo.PFSConfigInfoNum = pfsNum;
            }

            GetPrivateProfileString("UserMonitor", "Show", "", sb, sb.Capacity, Consts.ConfigPath);
            bool show = true;
            if (bool.TryParse(sb.ToString(), out show))
            {
                fps_ConfigInfo.Show = show;
            }


            for (int i = 1; i <= pfsNum; i++)
            {
                ConfigInfo configInfo = new ConfigInfo();
                //name
                GetPrivateProfileString("Config", "ServerId" + i, "", sb, sb.Capacity, Consts.ConfigPath);
                configInfo.PFSMonitorName = sb.ToString();
                //ip
                GetPrivateProfileString("Config", "Ip" + i, "", sb, sb.Capacity, Consts.ConfigPath);
                configInfo.Ip = sb.ToString();
                //port
                GetPrivateProfileString("Config", "Port" + i, "", sb, sb.Capacity, Consts.ConfigPath);
                int portNm = 19990;
                int.TryParse(sb.ToString(), out portNm);
                configInfo.Port = portNm;
                fps_ConfigInfo.ConfigInfo.Add(configInfo);
            }
            return fps_ConfigInfo;
        }









    }
}
