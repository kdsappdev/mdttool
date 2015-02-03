using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MDT.Tools.ErrorReport
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                #region 参数

                string info = "";
                string name = "";
                if (args != null && args.Length >= 0)
                {

                    for (int i = 0; i < args.Length; i = i + 2)
                    {
                       
                        switch (args[i])
                        {
                            case "-i":
                                info = args[i + 1];
                                LogHelper.Debug(string.Format("args i:{0}", args[i + 1]));
                                break;
                            case "-n":
                                name = args[i + 1];
                                LogHelper.Debug(string.Format("name i:{0}", args[i + 1]));
                                break;
                           

                        }
                    }

                }
                else
                {
                    LogHelper.Debug(string.Format("args is null"));
                }
                if (!string.IsNullOrEmpty(info))
                {
                    string[] strs = info.Split('|');
                    if (strs.Length >= 3)
                    {
                        string guid = Guid.NewGuid().ToString();
                        var er = new ErrorForm(strs[0], strs[1], strs[2], name);
                        Application.Run(er);
                    }
                }

                #endregion

               
                
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
    }
}
