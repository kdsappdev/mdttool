using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        //更新文件URL前
        static string url = System.Configuration.ConfigurationSettings.AppSettings["url"];

        static void Main(string[] args)
        {
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
            PopuAllDirectory(doc, root, dicInfo);
            //追加节点
            doc.AppendChild(root);
            //保存文档
            doc.Save(serverXmlName);
        }

        //递归组装xml文件方法
        private static void PopuAllDirectory(XmlDocument doc, XmlElement root, DirectoryInfo dicInfo)
        {
            foreach (FileInfo f in dicInfo.GetFiles())
            {
                //排除当前目录中生成xml文件的工具文件
                List<string> lt = new List<string>();
                lt.Add("ConsoleApplication1.exe".ToLower());
                lt.Add("AutoupdateService.xml".ToLower());
                lt.Add("AutoUpdater.config".ToLower());
                lt.Add("MDT.Tools.AutoUpdater.exe".ToLower());
                lt.Add("MDT.Tools.AutoUpdater.exe.config".ToLower());
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
                        child.SetAttribute("needRestart", "true");
                        root.AppendChild(child);
                    }
                }
            }

            foreach (DirectoryInfo di in dicInfo.GetDirectories())
                PopuAllDirectory(doc, root, di);


        }
    }
}
