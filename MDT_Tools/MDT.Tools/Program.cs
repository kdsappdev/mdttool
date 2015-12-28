using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;


namespace MDT.Tools
{
    static class Program
    {
        internal static EventWaitHandle ewh=null;
        static string appName = "MDT Smart Kit";
        static bool _isSingle = true;
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
            appName = System.Configuration.ConfigurationSettings.AppSettings["App"];
            bool.TryParse(System.Configuration.ConfigurationSettings.AppSettings["IsSingle"], out _isSingle);
            if (_isSingle)
            {
                bool createNew;
                ewh = new EventWaitHandle(false, EventResetMode.AutoReset, appName, out createNew);
                if (!createNew)
                {
                    ewh.Set();
                    return;
                }
                bool flag = Core.Utils.MachineHelper.CheckProcessIsMultiple(appName);
                if (flag)
                {
                    MessageBox.Show(@"另一个窗口已在运行，不能重复运行", @"提示");
                    return;
                }
                 
            }
            Application.Run(new MainForm());
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
        private static string pubulicKey = "AwEAAbTbL7J0sM71/EPEZ9Dnq64b9kxTGWDH7rrdCG1/vnqBAYP+KmsvMS3v6UI/ZMff6enBbncvMhEsjE8dnFmaZcyfgEAUCHB9A0ajJUu19O1zkWpIYiSUbdELsLz/KV8DG7UgyCIiqlNUMCRfXzKLHV3tMEY3JKjeJpmagAwNx4zb";
        private static string pwd = "2E812CE1C33C697BFF759F4DA5C11DD3103ABE791E798EC1B03ABFDD1AEC8169FC1936A225AE56C6B213DC94727D5984C584F79648D5FAC5EFC5C400938342240DD958B5C1255B7586D3D55C947949F601C05F22ECE341AFE08294B52B1CCA7E99E21D25BFE1DE6776EB0278E0D56D8F3E19E71E10D10A08BB7D921466928D87";
      
        private static void ErrorReport(string message)
        {
            try
            {
               string str= EncrypterHelper.DecryptRASString(pwd, pubulicKey);
                
                DateTime dt = DateTime.Now;
                string date = string.Format("_{0:0000}{1:00}{2:00}.log", dt.Year, dt.Month, dt.Day);
                var errorReport = "logs\\" + appName + date;

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
