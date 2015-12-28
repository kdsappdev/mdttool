using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.Server.Monitor.Plugin
{
    public class Consts
    {
        //public static readonly string AppPath = System.Windows.Forms.Application.StartupPath;
        //路径默认会在OFFICE文件下面，所以先配置到固定文件下，稍后可以根据需求改动。
        public static readonly string ConfigPath = Application.StartupPath + "\\control\\SMonitorConfig.ini";
        public static   string SoundLocation = "";

        //获取环境变量配置的路径，用环境变量来决定配置文件的路径。
        //string sPath = Environment.GetEnvironmentVariable("ConfigPath");
        //public static readonly string ConfigPath = sPath;
    }
}
