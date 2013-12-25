using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using KnightsWarriorAutoupdater;

namespace MDT.Tools.AutoUpdater
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            #region
            IAutoUpdater autoUpdater = new KnightsWarriorAutoupdater.AutoUpdater();
            bool bHasError = false;
            bool isUpdate = true;
            if (args != null && args.Length>=1)
            {
                bool.TryParse(args[0], out isUpdate);
            }
            else
            {
               isUpdate= autoUpdater.IsUpdate();
            }
            if (isUpdate)
            {
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
            }
            else
            {
                MessageBox.Show(string.Format("{0}已经是最新版本", System.Configuration.ConfigurationSettings.AppSettings["exe"]), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            #endregion

        }
    }
}
