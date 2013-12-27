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

            return configInfo;

        }
    }
}
