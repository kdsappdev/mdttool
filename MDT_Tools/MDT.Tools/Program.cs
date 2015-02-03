using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;


namespace MDT.Tools
{
    static class Program
    {
        static string appName = "MDT Smart Kit";
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

            bool flag = Core.Utils.MachineHelper.CheckProcessIsMultiple(appName);
            if (flag)
            {
                MessageBox.Show(@"另一个窗口已在运行，不能重复运行", @"提示");
            }
            else
            {
                Application.Run(new MainForm());
            }
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


        #region ErrorReport
        private static void ErrorReport(string message)
        {
            try
            {
                const string str = "HJGeO3QJyfXXOWln|vbGS5v5xAv0vf3GHkc9lHobMnG83Yb|mdt";
 
                DateTime dt = DateTime.Now;
                string date = string.Format("{0:0000}{1:00}{2:00}.log", dt.Year, dt.Month, dt.Day);
                var errorReport = "logs\\MDT_Error" + date;

                Process.Start("MDT.Tools.ErrorReport.exe", string.Format("-i {0} -n {1}", str, errorReport));
                Application.Exit();
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
            }
        }

        #endregion
    }
}
