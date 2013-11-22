using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
namespace MDT.Tools.Core.Utils
{
   public class FileHelper
    {
        #region ReadFileReturnBytes
        /// <summary>
        /// ReadFileReturnBytes 从文件中读取二进制数据
        /// </summary>      
        public static byte[] ReadFileReturnBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            BinaryReader br = new BinaryReader(fs);

            byte[] buff = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return buff;
        }
        #endregion

        public static string GetFileNameNoPath(string fileName_path)
        {
            return Path.GetFileName(fileName_path);
        }
       
        public static bool Write(string fileName, string[] strs)
        {
            bool status = true;
            try
            {
                CreateDirectory(fileName);
                File.Delete(fileName);
                FileInfo f = new FileInfo(fileName);
                StreamWriter sw = f.AppendText();
                foreach (string str in strs)
                {
                    sw.WriteLine(str);
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public static string CreateDirectory(string fileName)
        {
            string path = "";
            if (!string.IsNullOrEmpty(fileName))
            {
                fileName = fileName.Trim(new char[] { '\\' });
                string[] paths = fileName.Split(new char[] { '\\' });

                for (int i = 0; i < paths.Length - 1; i++)
                {
                    path = path + paths[i];
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = path + "\\";
                }
            }
            return path;
        }
    }
}
