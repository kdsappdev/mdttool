using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using KnightsWarriorAutoupdater;

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
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          
            bool flag = MDT.Tools.Core.Utils.MachineHelper.CheckProcessIsMultiple("MDT.Tools");
            if (flag)
            {
                MessageBox.Show("另一个窗口已在运行，不能重复运行", "提示");
            }
            else
            {
                #region
                bool bHasError = false;
                IAutoUpdater autoUpdater = new AutoUpdater();
                try
                {
                    autoUpdater.Update();
                }

                catch (Exception e)
                {
                    bHasError = true;
                }
                finally
                {
                    if (bHasError == true)
                    {
                        try
                        {
                            autoUpdater.RollBack();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                #endregion

               
                Application.Run(new MainForm());
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }
}
