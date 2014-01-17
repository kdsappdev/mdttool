using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

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
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                #region 配置

                string AutoUpdaterLib = System.Configuration.ConfigurationSettings.AppSettings["AutoUpdaterLib"];
                string AutoUpdaterClass = System.Configuration.ConfigurationSettings.AppSettings["AutoUpdaterClass"];
                string IsUpdate = "IsUpdate";
                string Update = "Update";
                string RollBack = "RollBack";
                #endregion

                #region 检测

                Assembly assembly = readAssembly(AutoUpdaterLib);
                object o = assembly.CreateInstance(AutoUpdaterClass);


                bool bHasError = false;
                bool isUpdate = true;
                if (args != null && args.Length >= 1)
                {
                    bool.TryParse(args[0], out isUpdate);
                }
                else
                {
                    isUpdate = (bool)o.GetType().GetMethod(IsUpdate).Invoke(o, null); ;
                }
                if (isUpdate)
                {
                    try
                    {
                        o.GetType().GetMethod(Update).Invoke(o, null); ;
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
                                o.GetType().GetMethod(RollBack).Invoke(o, null); ;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private static Assembly readAssembly(string file)
        {
            Assembly asm = null;
            byte[] stream = readFileReturnBytes(file);
            if (stream != null)
            {
                asm = Assembly.Load(stream);
            }
            return asm;
        }

        private static byte[] readFileReturnBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var br = new BinaryReader(fs);

            byte[] buff = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return buff;
        }
    }
}
