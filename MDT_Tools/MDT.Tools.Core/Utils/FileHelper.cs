using System;
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

            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var br = new BinaryReader(fs);

            byte[] buff = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return buff;
        }
        #endregion

        public static string GetFileNameNoPath(string fileNamePath)
        {
            return Path.GetFileName(fileNamePath);
        }

        public static bool Write(string fileName, string[] strs)
        {
            bool status = true;
            try
            {
                CreateDirectory(fileName);
                File.Delete(fileName);
                var f = new FileInfo(fileName);
                StreamWriter sw = f.AppendText();
                foreach (string str in strs)
                {
                    sw.WriteLine(str);
                }
                sw.Close();
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public static void DeleteDirectory(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
            }
        }

        public static string CreateDirectory(string fileName)
        {
            string path = "";
            if (!string.IsNullOrEmpty(fileName))
            {
                fileName = fileName.Trim(new[] { '\\' });
                string[] paths = fileName.Split(new[] { '\\' });

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
