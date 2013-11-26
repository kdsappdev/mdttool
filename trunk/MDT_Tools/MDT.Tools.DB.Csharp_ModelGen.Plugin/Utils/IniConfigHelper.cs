using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Csharp_Model.Plugin.Utils;
using MDT.Tools.DB.Csharp_ModelGen.Plugin.Model;

namespace MDT.Tools.DB.Csharp_ModelGen.Plugin.Utils
{
    internal class IniConfigHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);

        private static void CreateFile()
        {
            FileHelper.CreateDirectory(FilePathHelper.SystemConfig);
            if (!File.Exists(FilePathHelper.SystemConfig))
            {
                FileStream fs = File.Create(FilePathHelper.SystemConfig);
                fs.Close();
            }
        }
        private const string Group = "CsharpModelGenConfig";
        private const string NameSpace = "NameSpace";
        private const string OutPut = "OutPut";
        private const string TableFilter = "TableFilter";
        private const string IsShowGenCode = "IsShowGenCode";
        public static bool Write(CsharpModelGenConfig cms, ref string message)
        {
            bool status = false;
            if (cms != null)
            {

                try
                {
                    CreateFile();
                    WritePrivateProfileString(Group, NameSpace, cms.NameSpace, FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, OutPut, cms.OutPut, FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, TableFilter, cms.TableFilter, FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, IsShowGenCode, cms.IsShowGenCode + "", FilePathHelper.SystemConfig);
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
        public static CsharpModelGenConfig ReadCsharpModelGenConfig()
        {
            CsharpModelGenConfig cmc = new CsharpModelGenConfig();

            try
            {
                StringBuilder sb = new StringBuilder(255);
                GetPrivateProfileString(Group, NameSpace, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.NameSpace = sb.ToString();
                GetPrivateProfileString(Group, OutPut, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.OutPut = sb.ToString();
                GetPrivateProfileString(Group, TableFilter, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.TableFilter = sb.ToString();
                GetPrivateProfileString(Group, IsShowGenCode, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.IsShowGenCode = "true".Equals(sb.ToString().ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cmc;
        }
    }
}
