using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MDT.Tools.AutoUpdater.Config
{
    class Program
    {

        //获取当前目录
        //static string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string currentDirectory = System.Environment.CurrentDirectory;
        //服务端xml文件名称
        static string serverXmlName = "AutoUpdateService.xml";
        private static string clientXmlName = "autoupdater.config";
        //更新文件URL前
        static string url = System.Configuration.ConfigurationSettings.AppSettings["url"];
        private static string enableClientStr = System.Configuration.ConfigurationSettings.AppSettings["enableClient"];
        private static string clientPath = System.Configuration.ConfigurationSettings.AppSettings["clientPath"];
        private static bool enableClient = bool.TryParse(enableClientStr, out enableClient);
        static void Main(string[] args)
        {
            if (args != null && args.Length >= 1)
            {
                url = args[0];
            }


            #region client

            XmlDocument clientDoc = new XmlDocument();
            XmlDeclaration clientxmldecl = clientDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            clientDoc.AppendChild(clientxmldecl);
            XmlElement clientConfig = clientDoc.CreateElement("Config");
            clientDoc.AppendChild(clientConfig);

            XmlElement clientEnabled = clientDoc.CreateElement("Enabled");
            clientEnabled.InnerText = "true";
            clientConfig.AppendChild(clientEnabled);
            XmlElement clientServerUrl = clientDoc.CreateElement("ServerUrl");
            clientServerUrl.InnerText = url + "/" + serverXmlName;
            clientConfig.AppendChild(clientServerUrl);
            XmlElement clientRoot = clientDoc.CreateElement("UpdateFileList");

            #endregion

            //创建文档对象
            XmlDocument doc = new XmlDocument();
            //创建根节点
            XmlElement root = doc.CreateElement("updateFiles");
            //头声明
            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xmldecl);
            //获取当前目录对象
            DirectoryInfo dicInfo = new DirectoryInfo(currentDirectory);
            //调用递归方法组装xml文件
            PopuAllDirectory(doc, root, clientDoc, clientRoot, dicInfo);
            //追加节点
            doc.AppendChild(root);
            //保存文档
            doc.Save(serverXmlName);

            clientConfig.AppendChild(clientRoot);
            if (enableClient)
            {
                clientDoc.Save(clientPath + clientXmlName);
            }
        }

        //递归组装xml文件方法
        private static void PopuAllDirectory(XmlDocument doc, XmlElement root,XmlDocument clientDoc,XmlElement clientRoot, DirectoryInfo dicInfo)
        {
            foreach (FileInfo f in dicInfo.GetFiles())
            {
                //排除当前目录中生成xml文件的工具文件
                List<string> lt = new List<string>();
                lt.Add("ConsoleApplication1.exe".ToLower());
                lt.Add("AutoupdateService.xml".ToLower());
                lt.Add("AutoUpdater.config".ToLower());
                lt.Add("MDT.Tools.AutoUpdater.exe".ToLower());
                //lt.Add("MDT.Tools.AutoUpdater.exe.config".ToLower());
                lt.Add("MDT.Tools.AutoUpdater.Config.exe".ToLower());
                lt.Add("MDT.Tools.AutoUpdater.Config.exe.config".ToLower());
                if (!lt.Contains(f.Name.ToLower()) && !f.Name.EndsWith("pdb"))
                {
                    string path = dicInfo.FullName.Replace(currentDirectory, "").Replace("\\", "/");
                    string str = url + path + "/" + f.Name;
                    if (!(str.IndexOf("svn") > 0))
                    {
                        string version = FileVersionInfo.GetVersionInfo(f.FullName).FileVersion;
                        XmlElement child = doc.CreateElement("file");
                        child.SetAttribute("path", f.Name);
                        child.SetAttribute("url", str);
                        child.SetAttribute("lastver", string.IsNullOrEmpty(version) ? "1.0.0.0" : version);
                        child.SetAttribute("size", f.Length.ToString());
                        child.SetAttribute("md5", ByteArrayToHexString(HashData(f.OpenRead(),"md5")));
                        child.SetAttribute("needRestart", "true");
                        root.AppendChild(child);


                        XmlElement clientChild = clientDoc.CreateElement("LocalFile");
                        clientChild.SetAttribute("path", f.Name);
                         
                        clientChild.SetAttribute("lastver", string.IsNullOrEmpty(version) ? "1.0.0.0" : version);
                        clientChild.SetAttribute("size", f.Length.ToString());
                        clientChild.SetAttribute("md5", ByteArrayToHexString(HashData(f.OpenRead(),"md5")));
                       
                        clientRoot.AppendChild(clientChild);
                    }
                }
            }

            foreach (DirectoryInfo di in dicInfo.GetDirectories())
                PopuAllDirectory(doc, root,clientDoc, clientRoot, di);


        }

        #region
        /// <summary>
        /// 计算文件的 MD5 值
        /// </summary>
        /// <param name="fileName">要计算 MD5 值的文件名和路径</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string MD5File(string fileName)
        {
            return HashFile(fileName, "md5");
        }

        /// <summary>
        /// 计算文件的哈希值
        /// </summary>
        /// <param name="fileName">要计算哈希值的文件名和路径</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值16进制字符串</returns>
        public static string HashFile(string fileName, string algName)
        {
            if (!System.IO.File.Exists(fileName))
                return string.Empty;

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hashBytes = HashData(fs, algName);
            fs.Close();
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        public static byte[] HashData(Stream stream, string algName)
        {
            HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName 不能为 null");
            }
            if (string.Compare(algName, "sha1", true) == 0)
            {
                algorithm = SHA1.Create();
            }
            else
            {
                if (string.Compare(algName, "md5", true) != 0)
                {
                    throw new Exception("algName 只能使用 sha1 或 md5");
                }
                algorithm = MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }
        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        public static string ByteArrayToHexString(byte[] buf)
        {
            int iLen = 0;

            // 通过反射获取 MachineKeySection 中的 ByteArrayToHexString 方法，该方法用于将字节数组转换为16进制表示的字符串。
            Type type = typeof(System.Web.Configuration.MachineKeySection);
            MethodInfo byteArrayToHexString = type.GetMethod("ByteArrayToHexString", BindingFlags.Static | BindingFlags.NonPublic);

            // 字节数组转换为16进制表示的字符串
            return (string)byteArrayToHexString.Invoke(null, new object[] { buf, iLen });
        }
        #endregion
    }
}
