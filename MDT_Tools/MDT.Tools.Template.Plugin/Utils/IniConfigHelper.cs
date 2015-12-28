using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;
using MDT.Tools.Template.Plugin.Model;
namespace MDT.Tools.Template.Plugin.Utils
{
    internal class IniConfigHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(
            string ApplicationName, string KeyName, string DefaultString,
            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, string FileName);
        private static void CreateFile()
        {
            FileHelper.CreateDirectory(FilePathHelper.SystemConfig);
            if (!File.Exists(FilePathHelper.SystemConfig))
            {
                FileStream fs = File.Create(FilePathHelper.SystemConfig);
                fs.Close();
            }

        }
        private const string Group1 = "TemplateConfig";
        private const string Group2 = "TemplateParas";
        private const string TemplateNum = "TemplateNum";
        private const string MenuName = "MenuName";
        private const string DataTye = "DataTye";
        private const string TemplateName = "TemplateName";
        private const string SaveFileEncoding = "SaveFileEncoding";
        private const string SaveFilePath = "SaveFilePath";
        private const string IsAutoGenSaveFileName = "IsAutoGenSaveFileName";
        private const string SaveFileName = "SaveFileName";
        private const string IsShowGenCode = "IsShowGenCode";
        private const string CodeLanguage = "CodeLanguage";
        private const string Language = "Language";
        private const string MenuVisable = "MenuVisable";
        public static string ReadIniCN(string lpApplicationName, string lpKeyName, string iniPath)
        {
            string str = "";
            int size = 260;
            byte[] buff = new byte[size];
            int ret = GetPrivateProfileString(lpApplicationName, lpKeyName, "", buff, size, iniPath);
            if (ret > 0)
            {
                byte[] arry = new byte[ret];
                for (int i = 0; i < ret; i++)
                {
                    arry[i] = buff[i];
                }
                str = System.Text.Encoding.UTF8.GetString(arry);
            }
            return str;
        }

        public static TemplateConfig ReadTemplateConfig()
        {
            TemplateConfig templateConfig = new TemplateConfig();

            try
            {
                StringBuilder sb = new StringBuilder(255);
                GetPrivateProfileString(Group1, TemplateNum, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                int templateNum = 0;
                if (int.TryParse(sb.ToString(), out templateNum))
                {
                    templateConfig.TemplateNum = templateNum;

                }
                for (int i = 1; i <= templateConfig.TemplateNum; i++)
                {
                    TemplateParas templateParas = new TemplateParas();
                    templateConfig.TemplateParas.Add(templateParas);
                    templateParas.MenuName = ReadIniCN(Group2, MenuName + i, FilePathHelper.SystemConfig);

                    GetPrivateProfileString(Group2, DataTye + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                    templateParas.DataTye = sb.ToString();

                    GetPrivateProfileString(Group2, TemplateName + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                    templateParas.TemplateName = sb.ToString();



                    GetPrivateProfileString(Group2, SaveFileEncoding + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                    templateParas.SaveFileEncoding = sb.ToString();



                    GetPrivateProfileString(Group2, Language + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                    templateParas.Language = sb.ToString();





                    templateParas.SaveFilePath = ReadIniCN(Group2, SaveFilePath, FilePathHelper.SystemConfig);
                    if (string.IsNullOrEmpty(templateParas.SaveFilePath))
                    {
                        templateParas.SaveFilePath = FilePathHelper.ExportTemplatePath;
                    }
                    GetPrivateProfileString(Group2, SaveFileName + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                    templateParas.SaveFileName = sb.ToString();


                    GetPrivateProfileString(Group2, IsAutoGenSaveFileName + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);

                    bool isAutoGenSaveFileName = true;
                    if (bool.TryParse(sb.ToString(), out isAutoGenSaveFileName))
                    {
                        templateParas.IsAutoGenSaveFileName = isAutoGenSaveFileName;
                    }



                    GetPrivateProfileString(Group2, IsShowGenCode + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);

                    bool isShowGenCode = true;
                    if (bool.TryParse(sb.ToString(), out isShowGenCode))
                    {
                        templateParas.IsShowGenCode = isShowGenCode;
                    }
                    GetPrivateProfileString(Group2, CodeLanguage + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);
                    templateParas.CodeLanguage = sb.ToString();

                    GetPrivateProfileString(Group2, MenuVisable + i, "", sb, sb.Capacity, FilePathHelper.SystemConfig);

                    bool isMenuVisable = true;
                    if(!bool.TryParse(sb.ToString(), out isMenuVisable))
                    {
                        isMenuVisable = true;
                    }
                    templateParas.IsMenuVisable = isMenuVisable;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return templateConfig;
        }
    }
}
