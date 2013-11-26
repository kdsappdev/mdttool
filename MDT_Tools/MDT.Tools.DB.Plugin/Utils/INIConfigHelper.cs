using System;
using System.Collections.Generic;
using System.IO;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Plugin.Model;

namespace MDT.Tools.DB.Plugin.Utils
{
    internal static class IniConfigHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);


        private static void CreateFile()
        {
            FileHelper. CreateDirectory(FilePathHelper.SystemConfig);
            if (!File.Exists(FilePathHelper.SystemConfig))
            {
                FileStream fs = File.Create(FilePathHelper.SystemConfig);
                fs.Close();
            }
        }
      
        public static bool WriteDBInfo(DbConfigInfo dbConfigInfo, ref string message)
        {
            bool status = false;
            if (dbConfigInfo != null)
            {

                try
                {
                    CreateFile();
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
            IList<DbConfigInfo> dbConfigList = new List<DbConfigInfo>();
            try
            {
                CreateFile();               
                var fi = new FileInfo(FilePathHelper.SystemConfig);
                StreamReader sr = fi.OpenText();
                string content = sr.ReadToEnd();
                string[] temps = content.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sr.Close();
                string dbType = "";
                foreach (string str in temps)
                {
                    if (str.Contains("[") && str.Contains("]"))
                    {
                        dbType = str.Replace("[", "").Replace("]", "");
                        continue;
                    }
                    int index = str.IndexOf('=');
                    if (index >= 1)
                    {
                        try
                        {
                            var dbConfigInfo = new DbConfigInfo {DbType = dbType};

                            string dbConfigName = str.Substring(0, index);
                            string connectionString = str.Substring(index + 1, str.Length - index - 1).Trim(new[] { '"' });
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dbConfigList;
        }
    }
}
