using System;
using System.Collections.Generic;
using System.Diagnostics;
 
using System.Windows.Forms;
using Atf.Installer.SetUpCheck;
using Atf.Installer.SetupCheck.Util;
using Microsoft.Win32;

namespace MDT.Tools.SysCheck
{
    static class Program
    {
        private static SetupCheckForm scf;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            scf = new SetupCheckForm();
            Application.Run(scf);
        }
        static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error(new Exception(e.ExceptionObject.ToString()));
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                ErrorReport(ex.StackTrace);
            }
        }

        static void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogHelper.Error(e.Exception);
            ErrorReport(e.Exception.StackTrace);

        }

        private static void ErrorReport(string message)
        {
            try
            {
                string str = string.Format("{0}|{1}|{2}", scf.accessId, scf.assessKey, scf.bucketName);

                DateTime dt = DateTime.Now;
               
                string time = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
               
                Process.Start("ErrorReport.exe", string.Format("-i {0} -n {1} -p {2}", str, LogHelper.datePath(), dt.ToString("yyyyMMdd") + "/" + time));
                 
                Application.Exit();
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
            }
        }

        
    }
}
