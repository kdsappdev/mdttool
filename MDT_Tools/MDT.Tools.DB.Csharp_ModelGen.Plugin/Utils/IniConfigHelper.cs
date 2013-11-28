using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Model;


namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils
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
        private const string ModelNameSpace = "ModelNameSpace";
        private const string DALNameSpace = "DALNameSpace";
        private const string OutPut = "OutPut";
        private const string TableFilter = "TableFilter";
        private const string IsShowGenCode = "IsShowGenCode";
        private const string CodeRule = "CodeRule";
        private const string Ibatis = "Ibatis";
        public static bool Write(CsharpCodeGenConfig cms, ref string message)
        {
            bool status = false;
            if (cms != null)
            {

                try
                {
                    CreateFile();
                    WritePrivateProfileString(Group, ModelNameSpace, cms.ModelNameSpace, FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, DALNameSpace, cms.DALNameSpace + "", FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, OutPut, cms.OutPut, FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, TableFilter, cms.TableFilter, FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, IsShowGenCode, cms.IsShowGenCode + "", FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, CodeRule, cms.CodeRule, FilePathHelper.SystemConfig);
                    WritePrivateProfileString(Group, Ibatis, cms.Ibatis + "", FilePathHelper.SystemConfig);

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
        public static CsharpCodeGenConfig ReadCsharpModelGenConfig()
        {
            CsharpCodeGenConfig cmc = new CsharpCodeGenConfig();

            try
            {
                StringBuilder sb = new StringBuilder(255);
                GetPrivateProfileString(Group, ModelNameSpace, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.ModelNameSpace = sb.ToString();
                GetPrivateProfileString(Group, DALNameSpace, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.DALNameSpace = sb.ToString();
                GetPrivateProfileString(Group, OutPut, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.OutPut = sb.ToString();
                GetPrivateProfileString(Group, TableFilter, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.TableFilter = sb.ToString();
                GetPrivateProfileString(Group, IsShowGenCode, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.IsShowGenCode = "true".Equals(sb.ToString().ToLower());
                GetPrivateProfileString(Group, CodeRule, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.CodeRule = sb.ToString();
                GetPrivateProfileString(Group, Ibatis, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                cmc.Ibatis = sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cmc;
        }
    }
}
