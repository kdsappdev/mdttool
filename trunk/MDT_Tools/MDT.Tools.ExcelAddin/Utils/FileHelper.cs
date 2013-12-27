using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace MDT.Tools.ExcelAddin.Utils
{
    public class FileHelper
    {
        #region 写入文件

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
            LogHelper.Info("输出数据：" + str);
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region 删除原始文件
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

        #region 创建文件
        public static FileInfo CreateFileInfo(string path, string fileName)
        {

            string fullPath = path + fileName;
            FileInfo fi = new FileInfo(fullPath);
            return fi;
        }
        #endregion
    }
}
