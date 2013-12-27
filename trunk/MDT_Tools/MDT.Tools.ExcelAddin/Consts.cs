using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MDT.Tools.ExcelAddin.Utils
{
    public class Consts
    {

        //public static readonly string Version = typeof(Consts).GetType().Assembly.GetName().Version + "(builder 20130607)";
        //public static readonly string Author = "deshuai.kong";
        //public static readonly string AppPath = System.Windows.Forms.Application.StartupPath;
        //public static readonly string ConfigPath = @"E:\\config.ini";

        public static readonly string ConfigPath = Consts.path();
        public static string path()
        {

            //获取环境变量配置的路径，用环境变量来决定配置文件的路径。
            string sPath = Environment.GetEnvironmentVariable("ConfigPath");
            if (sPath == null)
            {
                sPath = @"E:\Tullett\config.ini";
                return sPath;
            }
            return sPath + @"\config.ini";

        }

    }
}


