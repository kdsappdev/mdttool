using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Plugin.Model;

namespace MDT.Tools.DB.Plugin.Utils
{
    internal sealed class INIConfigHelper
    {
        private INIConfigHelper()
        { }
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileSectionNames(System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);

   
        private static void createFile()
        {
            FileHelper. CreateDirectory(FilePathHelper.SystemConfig);
            if (!File.Exists(FilePathHelper.SystemConfig))
            {
                FileStream fs = File.Create(FilePathHelper.SystemConfig);
                fs.Close();
            }
        }
      
        public static bool WriteDBInfo(Model.DbConfigInfo dbConfigInfo, ref string message)
        {
            bool status = false;
            if (dbConfigInfo != null)
            {

                try
                {
                    createFile();
                    WritePrivateProfileString(dbConfigInfo.DbType, dbConfigInfo.DbConfigName, dbConfigInfo.ConnectionString, FilePathHelper.SystemConfig);
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    message = ex.Message;
                }
            }
            return status;
        }
        public static IList<DbConfigInfo> ReadDBInfo()
        {
            IList<Model.DbConfigInfo> dbConfigList = new List<Model.DbConfigInfo>();
            try
            {
                createFile();               
                FileInfo fi = new FileInfo(FilePathHelper.SystemConfig);
                StreamReader sr = fi.OpenText();
                string content = sr.ReadToEnd();
                string[] temps = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sr.Close();
                string dbType = "";
                foreach (string str in temps)
                {
                    if (str.Contains("[") && str.Contains("]"))
                    {
                        dbType = str.Replace("[", "").Replace("]", "");
                        continue;
                    }
                    else
                    {
                    
                        int index = str.IndexOf('=');
                        if (index >= 1)
                        {
                            try
                            {
                                DbConfigInfo dbConfigInfo = new DbConfigInfo();
                                dbConfigInfo.DbType = dbType;
                               
                                string dbConfigName = str.Substring(0, index);
                                string connectionString = str.Substring(index + 1, str.Length - index - 1).Trim(new char[] { '"' });
                                dbConfigInfo.ConnectionString = connectionString;
                                dbConfigInfo.DbConfigName = dbConfigName;
                                dbConfigList.Add(dbConfigInfo);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dbConfigList;
        }
    }
}
