using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MDT.Tools.Order.Monitor.Plugin
{
    /// <summary>
    /// 获取 系统配置文件 ini
    /// </summary>
    public class IniConfigHelpe
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

        public static Event_ConfigInfo ReadConfigInfo()
        {

            Event_ConfigInfo event_ConfigInfo = new Event_ConfigInfo();

            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString("ServerInfo", "ServerIp", "", sb, sb.Capacity, Consts.ConfigPath);
            event_ConfigInfo.Ip = sb.ToString();
            
            GetPrivateProfileString("ServerInfo", "ServerPort", "", sb, sb.Capacity, Consts.ConfigPath);
            int port = 0;
            if (int.TryParse(sb.ToString(), out port))
            {
                event_ConfigInfo.Port = port;
            }

            GetPrivateProfileString("ServerInfo", "ServerId", "", sb, sb.Capacity, Consts.ConfigPath);
            event_ConfigInfo.svrId = sb.ToString();

            GetPrivateProfileString("OrderMonitor", "topicStr1", "", sb, sb.Capacity, Consts.ConfigPath);
            event_ConfigInfo.topicStr1 = sb.ToString();

            GetPrivateProfileString("OrderMonitor", "topichistoryStr1", "", sb, sb.Capacity, Consts.ConfigPath);
            event_ConfigInfo.topichistoryStr1 = sb.ToString();

            GetPrivateProfileString("OrderMonitor", "topicStr2", "", sb, sb.Capacity, Consts.ConfigPath);
            event_ConfigInfo.topicStr2 = sb.ToString();

            GetPrivateProfileString("OrderMonitor", "topichistoryStr2", "", sb, sb.Capacity, Consts.ConfigPath);
            event_ConfigInfo.topichistoryStr2 = sb.ToString();
            return event_ConfigInfo;
        }
    }
}
