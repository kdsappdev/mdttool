using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using MDT.Tools.Aliyun.Common.Oss;


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
        private static string autoUpdaterUrl = "autoUpdaterUrl.txt";
        //更新文件URL前

        static string clientAutoUpdaterUrl = System.Configuration.ConfigurationSettings.AppSettings["clientAutoUpdaterUrl"];


        static string url = System.Configuration.ConfigurationSettings.AppSettings["url"];



        private static string enableClientStr = System.Configuration.ConfigurationSettings.AppSettings["enableClient"];
        private static string clientPath = System.Configuration.ConfigurationSettings.AppSettings["clientPath"];
        private static bool enableClient = false;

        private static string enableEncrpterRunTimeConfigStr = System.Configuration.ConfigurationSettings.AppSettings["enableEncrpterRunTimeConfig"];
        private static bool enableEncrpterRunTimeConfig = true;

        private static string Type = System.Configuration.ConfigurationSettings.AppSettings["type"];
        private static int type = 0;

        private static string RunTimeConfigPath = System.Configuration.ConfigurationSettings.AppSettings["RunTimeConfigPath"];
        private static string privateKey = System.Configuration.ConfigurationSettings.AppSettings["privateKey"];

        private static string enableAliyunStr = System.Configuration.ConfigurationSettings.AppSettings["enableAliyun"];
        private static bool enableAliyun = true;
        private static string accessId = System.Configuration.ConfigurationSettings.AppSettings["accessId"];
        private static string accessKey = System.Configuration.ConfigurationSettings.AppSettings["accessKey"];
        private static string bucketName = System.Configuration.ConfigurationSettings.AppSettings["bucketName"];
        private static string prefix = System.Configuration.ConfigurationSettings.AppSettings["prefix"];
        static OssHelper ossHelper = new OssHelper();
        private static bool isGenDiff = false;

        private static string md5 = "md5.txt";
        private static string md5_new = "md5_New.txt";
        private static string diffMd5 = "md5_Diff.txt";
        static void Main(string[] args)
        {
            string version = "1.0.0.0";
            string minimumRequiredVersion = "1.0.0.0";
            string comment = "";

            if (args != null && args.Length >= 0)
            {
                for (int i = 0; i < args.Length; i = i + 2)
                {
                    switch (args[i])
                    {
                        case "-u":
                            url = args[i + 1];
                            break;
                        case "-uc":
                            clientAutoUpdaterUrl = args[i + 1];
                            break;
                        case "-v":
                            version = args[i + 1];
                            break;
                        case "-m":
                            minimumRequiredVersion = args[i + 1];
                            break;
                        case "-c":
                            comment = args[i + 1];
                            break;
                        case "-d"://差异检查
                            isGenDiff = true;
                            break;

                    }
                }

            }



            bool.TryParse(enableClientStr, out enableClient);


            int.TryParse(Type, out type);
            bool.TryParse(enableEncrpterRunTimeConfigStr, out enableEncrpterRunTimeConfig);

            bool.TryParse(enableAliyunStr, out enableAliyun);


            if (isGenDiff)
            {
                genMd5Diff();
            }
            else
            {
                genAutoUpdater(version, comment, minimumRequiredVersion);
            }

        }

        //根据md5生成变动文件清单
        private static void genMd5Diff()
        {
            DirectoryInfo dicInfo = new DirectoryInfo(currentDirectory);
            Dictionary<string, string> md5Dic = new Dictionary<string, string>();
            #region md5
            if (File.Exists(md5))
            {
                string[] strs = File.ReadAllLines(md5);
                foreach (var str in strs)
                {
                    string[] s = str.Split('|');
                    if (s.Length == 2)
                    {
                        if (!md5Dic.ContainsKey(str))
                        {
                            md5Dic.Add(str, s[1]);
                        }
                        else
                        {
                            Console.Error.WriteLine(s[1] + " 存在多份,发布将会有问题.");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine(s[1] + " 格式错误.");
                    }
                }
            }

            #endregion
            File.Delete(diffMd5);
            #region diffMd5

            StreamWriter mdt_new = File.CreateText(md5_new);
            using (StreamWriter diffMd5sw= File.CreateText(diffMd5))
                PopuAllDirectory(mdt_new, diffMd5sw, md5Dic, dicInfo);
            mdt_new.Close();
            File.Delete(md5);
            File.Move(md5_new, md5);
            #endregion

        }

        private static void PopuAllDirectory(StreamWriter mdt_new, StreamWriter diffMd5sw, Dictionary<string, string> md5Dic, DirectoryInfo dicInfo)
        {
           
                //排除当前目录中生成xml文件的工具文件
                List<string> lt = new List<string>();
                lt.Add("ConsoleApplication1.exe".ToLower());
                lt.Add("AutoupdateService.xml".ToLower());
                lt.Add("AutoUpdater.config".ToLower());
                lt.Add("MDT.Tools.AutoUpdater.exe".ToLower());
                lt.Add("AutoUpdater.exe".ToLower());
                lt.Add(autoUpdaterUrl.ToLower());
                lt.Add(md5.ToLower());
                lt.Add(md5_new.ToLower());
                lt.Add(diffMd5.ToLower());
                lt.Add("MDT.Tools.AutoUpdater.Config.exe".ToLower());
                lt.Add("MDT.Tools.AutoUpdater.Config.exe.config".ToLower());
                foreach (FileInfo f in dicInfo.GetFiles())
                {

                    if (!lt.Contains(f.Name.ToLower()) && !f.Name.EndsWith("pdb"))
                    {
                        string path = dicInfo.FullName.Replace(currentDirectory, "").Replace("\\", "/").TrimStart('/');
                        if (!string.IsNullOrEmpty(path))
                        {
                            path = path + "/";
                        }
                        string str = url + prefix + path + f.Name;

                        string path1 = f.Name;
                        if (!string.IsNullOrEmpty(path))
                        {
                            path1 = path + f.Name;

                        }
                        path1 = path1.Trim('/');
                        if (!(str.IndexOf("svn") > 0))
                        {
                            string fmd5 = ByteArrayToHexString(HashData(f.OpenRead(), "md5"));
                            if (!md5Dic.ContainsKey(fmd5+"|"+f.Name))
                            {
                                diffMd5sw.WriteLine(string.Format("{0}"
                                                                  , path1));
                            }
                            mdt_new.WriteLine(string.Format("{0}|{1}", fmd5
                                                                  , f.Name));
                        }
                    }
                }

                foreach (DirectoryInfo di in dicInfo.GetDirectories())
                    PopuAllDirectory(mdt_new, diffMd5sw,md5Dic, di);
            
        }

        private static void genAutoUpdater(string version, string comment, string minimumRequiredVersion)
        {
            if (enableAliyun)
            {
                ossHelper.OssConfig = new OssConfig() { AccessId = accessId, AccessKey = accessKey, BucketName = bucketName };
                ossHelper.Delete(prefix);
            }

            #region client

            XmlDocument clientDoc = new XmlDocument();
            XmlDeclaration clientxmldecl = clientDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            clientDoc.AppendChild(clientxmldecl);
            XmlElement clientConfig = clientDoc.CreateElement("Config");
            clientDoc.AppendChild(clientConfig);

            //XmlElement clientEnabled = clientDoc.CreateElement("Enabled");
            //clientEnabled.InnerText = "true";
            //clientConfig.AppendChild(clientEnabled);
            XmlElement clientServerUrl = clientDoc.CreateElement("ServerUrl");
            if (type == 0)
            {
                clientServerUrl.InnerText = clientAutoUpdaterUrl + autoUpdaterUrl;
            }
            else
            {
                clientServerUrl.InnerText = clientAutoUpdaterUrl;
            }
            Console.WriteLine(type);
            clientConfig.AppendChild(clientServerUrl);
            XmlElement clientVersion = clientDoc.CreateElement("Version");
            clientVersion.InnerText = version;
            clientConfig.AppendChild(clientVersion);
            XmlElement clientType = clientDoc.CreateElement("Type");
            clientType.InnerText = type + "";
            clientConfig.AppendChild(clientType);
            XmlElement clientRoot = clientDoc.CreateElement("UpdateFileList");

            var f = File.CreateText(autoUpdaterUrl);
            f.WriteLine(url + prefix + serverXmlName);
            f.Close();

            #endregion

            //创建文档对象
            XmlDocument doc = new XmlDocument();

            //创建根节点
            XmlElement root = doc.CreateElement("config");

            XmlElement versionE = doc.CreateElement("version");
            versionE.InnerText = version;
            XmlElement commentE = doc.CreateElement("comment");
            commentE.InnerText = comment;

            XmlElement minimumRequiredVersionE = doc.CreateElement("minimumrequiredversion");
            minimumRequiredVersionE.InnerText = minimumRequiredVersion;

            XmlElement updateFilesE = doc.CreateElement("updateFiles");
            //头声明
            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xmldecl);
            //获取当前目录对象
            DirectoryInfo dicInfo = new DirectoryInfo(currentDirectory);
            //调用递归方法组装xml文件
            PopuAllDirectory(doc, updateFilesE, clientDoc, clientRoot, dicInfo);
            root.AppendChild(versionE);
            root.AppendChild(minimumRequiredVersionE);
            root.AppendChild(commentE);
            root.AppendChild(updateFilesE);
            //追加节点
            doc.AppendChild(root);
            //保存文档

            doc.Save(serverXmlName);
            Console.WriteLine(serverXmlName + " 保存成功");
            clientConfig.AppendChild(clientRoot);
            if (enableClient)
            {
                clientDoc.Save(clientPath + clientXmlName);
                Console.WriteLine(clientPath + clientXmlName + " 保存成功");
                if (enableAliyun)
                {
                    ossHelper.UpLoad(clientPath + clientXmlName, prefix + clientPath + clientXmlName);
                }
            }

            if (enableAliyun)
            {
                ossHelper.UpLoad(serverXmlName, prefix + serverXmlName);
            }

            encryptRunTimeConfig();
        }

        private static void encryptRunTimeConfig()
        {
            if (enableEncrpterRunTimeConfig)
            {
                var fi = new FileInfo(RunTimeConfigPath);
                StreamReader sr = fi.OpenText();
                string content = sr.ReadToEnd();
                sr.Close();
                //File.WriteAllText(RunTimeConfigPath, EncrypterHelper.EncryptRASString(content, privateKey));
            }
        }

        //递归组装xml文件方法
        private static void PopuAllDirectory(XmlDocument doc, XmlElement root, XmlDocument clientDoc, XmlElement clientRoot, DirectoryInfo dicInfo)
        {
            //排除当前目录中生成xml文件的工具文件
            List<string> lt = new List<string>();
            lt.Add("ConsoleApplication1.exe".ToLower());
            lt.Add("AutoupdateService.xml".ToLower());
            lt.Add("AutoUpdater.config".ToLower());
            lt.Add("MDT.Tools.AutoUpdater.exe".ToLower());
            lt.Add("AutoUpdater.exe".ToLower());
            lt.Add(autoUpdaterUrl.ToLower());
            lt.Add(md5.ToLower());
            lt.Add(md5_new.ToLower());
            lt.Add(diffMd5.ToLower());
            //lt.Add("MDT.Tools.AutoUpdater.exe.config".ToLower());
            lt.Add("MDT.Tools.AutoUpdater.Config.exe".ToLower());
            lt.Add("MDT.Tools.AutoUpdater.Config.exe.config".ToLower());
            foreach (FileInfo f in dicInfo.GetFiles())
            {

                if (!lt.Contains(f.Name.ToLower()) && !f.Name.EndsWith("pdb"))
                {
                    string path = dicInfo.FullName.Replace(currentDirectory, "").Replace("\\", "/").TrimStart('/');
                    if (!string.IsNullOrEmpty(path))
                    {
                        path = path + "/";
                    }
                    string str = url + prefix + path + f.Name;

                    string path1 = f.Name;
                    if (!string.IsNullOrEmpty(path))
                    {
                        path1 = path + f.Name;

                    }
                    path1 = path1.Trim('/');
                    if (!(str.IndexOf("svn") > 0))
                    {
                        string versionStr = FileVersionInfo.GetVersionInfo(f.FullName).FileVersion;
                        Version version = new Version(1, 0, 0, 0);
                        try
                        {
                            if (!string.IsNullOrEmpty(versionStr))
                            {
                                version = new Version(versionStr);
                            }
                        }
                        catch
                        {


                        }
                        XmlElement child = doc.CreateElement("file");
                        child.SetAttribute("path", f.Name);
                        child.SetAttribute("url", url + prefix + path + HttpUtility.UrlEncode(f.Name).Replace("+", "%20"));
                        child.SetAttribute("lastver", version.ToString());
                        child.SetAttribute("size", f.Length.ToString());
                        child.SetAttribute("md5", ByteArrayToHexString(HashData(f.OpenRead(), "md5")));
                        child.SetAttribute("needRestart", "true");
                        root.AppendChild(child);


                        XmlElement clientChild = clientDoc.CreateElement("LocalFile");
                        clientChild.SetAttribute("path", f.Name);

                        clientChild.SetAttribute("lastver", version.ToString());
                        clientChild.SetAttribute("size", f.Length.ToString());
                        clientChild.SetAttribute("md5", ByteArrayToHexString(HashData(f.OpenRead(), "md5")));

                        clientRoot.AppendChild(clientChild);

                        if (enableAliyun)
                        {
                            path1 = prefix + path1;
                            ossHelper.UpLoad(f, path1);

                        }
                    }
                }
            }

            foreach (DirectoryInfo di in dicInfo.GetDirectories())
                PopuAllDirectory(doc, root, clientDoc, clientRoot, di);


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
