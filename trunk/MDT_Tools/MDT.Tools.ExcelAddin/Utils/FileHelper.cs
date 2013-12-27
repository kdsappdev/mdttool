using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace MDT.Tools.ExcelAddin.Utils
{
    public class FileHelper
    {
        #region д���ļ�

        public static void Write(string path, string fileName, string str, bool append)
        {

            string fullPath = path;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            fullPath += fileName;
            var sw = new StreamWriter(fullPath, append);
            sw.WriteLine(str);
            LogHelper.Info("������ݣ�" + str);
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region ɾ��ԭʼ�ļ�
        public static void Delete(string path, string fileName)
        {

            string fullPath = path;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            fullPath += fileName;
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        #endregion

        #region �����ļ�
        public static FileInfo CreateFileInfo(string path, string fileName)
        {

            string fullPath = path + fileName;
            FileInfo fi = new FileInfo(fullPath);
            return fi;
        }
        #endregion
    }
}
