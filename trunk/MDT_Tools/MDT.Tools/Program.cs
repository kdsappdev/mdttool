using System;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;


namespace MDT.Tools
{
    static class Program
    {
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

            bool flag = Core.Utils.MachineHelper.CheckProcessIsMultiple("MDT Smart Kit");
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
        }

        static void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogHelper.Error(e.Exception);

        }
    }
}
