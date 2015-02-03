using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
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
                ServicePointManager.DefaultConnectionLimit = 200;
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                LogHelper.Debug(string.Format("DefaultConnectionLimit:{0}", ServicePointManager.DefaultConnectionLimit));
                string AutoUpdaterLib = System.Configuration.ConfigurationSettings.AppSettings["AutoUpdaterLib"];
                string AutoUpdaterClass = System.Configuration.ConfigurationSettings.AppSettings["AutoUpdaterClass"];
                string AppName = System.Configuration.ConfigurationSettings.AppSettings["AppName"];
                string AppExe = System.Configuration.ConfigurationSettings.AppSettings["AppExe"];

                string IsUpdate = "IsUpdate";
                string Update = "Update";
                string RollBack = "RollBack";
                string SetAppName = "SetAppName";
                string SetAppExe = "SetAppExe";
                string SetServerUrl = "SetServerUrl";
                string autoUpdaterUrl = "";
                #endregion
                LogHelper.Info("读取配置成功");
                int isUpdate = 0;
                bool isCheck = true;
                #region 参数
                if (args != null && args.Length >= 0)
                {
                  
                    for (int i = 0; i < args.Length; i = i + 2)
                    {
                        
                        switch (args[i])
                        {
                            case "-i":
                                int.TryParse(args[i + 1], out isUpdate);
                                LogHelper.Info(string.Format("isUpdate:{0}", args[i + 1]));
                                break;
                            
                            case "-c":
                                bool.TryParse(args[i + 1], out isCheck);
                                LogHelper.Info(string.Format("isCheck:{0}", args[i + 1]));
                                break;
                            case "-uc":
                                autoUpdaterUrl = args[i + 1];
                                LogHelper.Info(string.Format("autoUpdaterUrl:{0}", args[i + 1]));
                                break;

                        }
                    }

                }
                else
                {
                    LogHelper.Info(string.Format("args is null"));
                }
                //MessageBox.Show(string.Format("{0}{1}{2}",isCheck,isUpdate,autoUpdaterUrl));
                #endregion



                #region 检测
                LogHelper.Debug(string.Format("assembly Begin"));
                Assembly assembly = readAssembly(AutoUpdaterLib);
                LogHelper.Debug(string.Format("assembly:{0}", assembly));
                LogHelper.Debug(string.Format("assembly End"));
                object o = assembly.CreateInstance(AutoUpdaterClass);
                LogHelper.Debug(string.Format("AutoUpdaterClass:{0}",o));
                if (o != null)
                {
                    o.GetType().GetMethod(SetAppName).Invoke(o, new object[] { AppName });
                    LogHelper.Debug(string.Format("SetAppName Invoke success[{0}]",AppName));
                    o.GetType().GetMethod(SetAppExe).Invoke(o, new object[] { AppExe });
                    LogHelper.Debug(string.Format("SetAppExe Invoke success[{0}]", AppExe));
                    if(!string.IsNullOrEmpty(autoUpdaterUrl))
                    {
                        o.GetType().GetMethod(SetServerUrl).Invoke(o, new object[] { autoUpdaterUrl });
                        LogHelper.Debug(string.Format("SetServerUrl Invoke success[{0}]", autoUpdaterUrl));
                    }

                    bool bHasError = false;
                   

                   if(isCheck)
                   {
                       LogHelper.Debug(string.Format("IsUpdate Invoke Begin")); ;
                        isUpdate = (int)o.GetType().GetMethod(IsUpdate).Invoke(o, null);
                        LogHelper.Debug(string.Format("IsUpdate Invoke End[{0}]", isUpdate)); ;
                    }
                    if (isUpdate > 0)
                    {
                        try
                        {
                            LogHelper.Debug(string.Format("Update Invoke Begin")); ;
                            o.GetType().GetMethod(Update).Invoke(o, null); ;
                            LogHelper.Debug(string.Format("Update Invoke End")); 
                        }
                        catch(Exception e)
                        {
                            bHasError = true;
                        }

                        finally
                        {
                            if (bHasError)
                            {
                                LogHelper.Debug(string.Format("RollBack Invoke Begin")); ;
                                o.GetType().GetMethod(RollBack).Invoke(o, null); ;
                                LogHelper.Debug(string.Format("RollBack Invoke End")); 

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0}已经是最新版本", AppName), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("MDT.Tools.AutoUpdater", ex.Message, EventLogEntryType.Error);
                LogHelper.Error(ex); 
                MessageBox.Show(ex.StackTrace, "失败", MessageBoxButtons.OK, MessageBoxIcon.Information);

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


    public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
    {
        public bool CheckValidationResult(System.Net.ServicePoint sp,
           System.Security.Cryptography.X509Certificates.X509Certificate cert,
           System.Net.WebRequest req, int problem)
        {
            return true;

        }

    }

}
