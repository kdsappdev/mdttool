using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Atf.Installer.SetupCheck.Util;
using System.Runtime.InteropServices;
using MDT.Tools.Aliyun.Common.Oss;
using MDT.Tools.SysCheck;
using MDT.Tools.SysCheck.SetupCheck;
using System.Xml;
using System.Net;

namespace Atf.Installer.SetUpCheck
{
    partial class SetupCheckForm : Form
    {
        private string pubulicKey = ConfigurationSettings.AppSettings["key1"];
        private string pwd = ConfigurationSettings.AppSettings["key2"];
        public string accessId = "";
        public string assessKey = "";
        public string bucketName = "";

        private string helpDesk = ConfigurationSettings.AppSettings["helpDesk"];
        private string local;
        private bool success = true;
        private List<string[]> urlList = new List<string[]>();
        private List<String> osSet = new List<string>();
        private const double NET_FRAMEWORK_VERSION = 2.050727;
        private const double IE_Version = 6.0;
        private string ClientType;

        private void getUrls()
        {
            urlList.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load("control/LocalEnvSetting.xml");    //加载Xml文件  
            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList personNodes = rootElem.GetElementsByTagName("ServerInfos"); //获取ServerInfos子节点集合  
            XmlNodeList SystemEnvspersonNodes = rootElem.GetElementsByTagName("SystemEnvs"); //获取ServerInfos子节点集合 
            XmlNodeList SystemEnvpersonNodes = ((XmlElement)SystemEnvspersonNodes[0]).GetElementsByTagName("SystemEnv");
            XmlNodeList person1Nodes = ((XmlElement)personNodes[0]).GetElementsByTagName("ServerInfo");  //获取ServerInfo子XmlElement集合 
            ClientType = ((XmlElement)SystemEnvpersonNodes[6]).GetElementsByTagName("Value")[0].InnerText; //获取ip地址
            foreach (XmlNode node in person1Nodes)
            {
                string Name = ((XmlElement)node).GetAttribute("Name");   //获取name属性值  
                string Host = ((XmlElement)node).GetElementsByTagName("Host")[0].InnerText; //获取ip地址
                string Port = ((XmlElement)node).GetElementsByTagName("Port")[0].InnerText; // 获取端口号
                string Protocal = ((XmlElement)node).GetElementsByTagName("Protocal")[0].InnerText; //获取通信协议
                string[] hosts = Host.Split(';');
                string[] Ports = Port.Split(';');
                string[] Protocals = Protocal.Split(';');
                string[] urlinfo = new string[4];
                urlinfo[0] = Name + "(通信服务器):";
                urlinfo[1] = Protocals[0];
                urlinfo[2] = hosts[0].Contains("/") ? hosts[0].Substring(0, hosts[0].IndexOf("/")) : hosts[0];
                urlinfo[3] = Ports[0];
                urlList.Add(urlinfo);
                string[] urlinfo1 = new string[4];
                urlinfo1[0] = Name + "(查询服务器):";
                urlinfo1[1] = Protocals[1];
                urlinfo1[2] = hosts[1].Contains("/") ? hosts[1].Substring(0, hosts[1].IndexOf("/")) : hosts[1];
                urlinfo1[3] = Ports[1];
                urlList.Add(urlinfo1);

            }
        }

        public SetupCheckForm()
        {
            try
            {


                osSet.Add("5.0");
                osSet.Add("5.1");
                osSet.Add("5.2");
                osSet.Add("6.0");
                osSet.Add("6.1");
                osSet.Add("6.2");
                osSet.Add("6.3");
                LogHelper.Info("1");
                InitializeComponent();
                LogHelper.Info("2");
                string[] strs = BigInteger.DecryptRASString(pwd, pubulicKey).Split('|');
                accessId = strs[0];
                assessKey = strs[1];
                bucketName = strs[2];
                LogHelper.Info("3");
            }
            catch (Exception e)
            {
                LogHelper.Info("SetupCheckForm");
                LogHelper.Error(e);
            }

        }


        //[DllImport("user32", EntryPoint = "SetWindowPos")]
        //public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, int flags);

        private void SetupCheckForm_Load(object sender, EventArgs e)
        {
            try
            {

                dgvCheckItem.AutoGenerateColumns = false;//是否自动创建列
                this.getUrls();
                this.init();

            }
            catch (Exception ex)
            {
                LogHelper.Info("SetupCheckForm_Load");
                LogHelper.Error(ex);
            }
        }

        private List<CheckItemModel> lt = new List<CheckItemModel>();
        private List<CheckItemModel> initlt = new List<CheckItemModel>();

        private void initCheckItem()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                dgvCheckItem.DataSource = lt;
                dgvCheckItem.Refresh();
            }));
        }

        private void changeClTipText(string text)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                clTip.Text = text;
            }));
        }

        private List<string> frameworkList;
        private void check()
        {
            int par = urlList.Count * 3 + 10;
            int num = 0;
            lt.Clear();
            try
            {
                changeClTipText("正在检测  操作系统");
                LogHelper.Debug("check OS.");
                string[] os = MachineHelper.GetOperatingSystem();
                if (osSet.Contains(os[0]))
                {
                    initlt[0].Name = "操作系统";
                    initlt[0].IsInstall = "是";
                    initlt[0].CheckResult = os[1] + (MachineHelper.getPhysicalMemory() ? " 64位" : " 32位");
                    initlt[0].IsOk = "符合";
                    initlt[0].AResult = "无建议";
                }
                else
                {
                    initlt[0].Name = "操作系统";
                    initlt[0].IsInstall = "是";
                    initlt[0].CheckResult = os[1] + (MachineHelper.getPhysicalMemory() ? " 64位" : " 32位");
                    initlt[0].IsOk = "不符合";
                    initlt[0].AResult = "您使用了不支持的操作系统,系统支持的操作系统包括：Windows XP,Windows Server 2003,Windows Vista, Windows Server 2008,Windows 7, Windows Server 2008 R2,Windows 8";
                }
                this.setLoading(100 / (10 + par + num) * 1);

                //操作系统语言
                LogHelper.Debug("check Language.");
                if (MachineHelper.ContainsChineseLanguage())
                {

                    initlt[1].Name = "操作系统语言";
                    initlt[1].IsInstall = "是";
                    initlt[1].CheckResult = "简体中文";
                    initlt[1].IsOk = "符合";
                    initlt[1].AResult = "无建议";

                }
                else
                {
                    initlt[1].Name = "操作系统语言";
                    initlt[1].IsInstall = "否";
                    initlt[1].CheckResult = "简体中文";
                    initlt[1].IsOk = "不符合";
                    initlt[1].AResult = "No Chinese language package supported!";

                    LogHelper.Warn("No Chinese language package supported.");
                }
                this.setLoading(100 / (10 + par + num) * 2);


                //Settings.Default.ie_version;
                changeClTipText("正在检测  系统内存");
                double memory = MachineHelper.GetPhysicalMemory();
                if (memory < 0.512)
                {
                    initlt[2].Name = "内存";
                    initlt[2].IsInstall = "是";
                    initlt[2].CheckResult = memory * 1000 + "M";
                    initlt[2].IsOk = "不符合";
                    initlt[2].AResult = "内存最少不低于512M";
                    LogHelper.Error("内存最少不低于512M.");
                }
                else if (memory < 2.00)
                {
                    initlt[2].Name = "内存";
                    initlt[2].IsInstall = "是";
                    initlt[2].CheckResult = memory.ToString("N2") + "G";
                    initlt[2].IsOk = "符合";
                    initlt[2].AResult = "内存在2G以上,系统运行更好";
                }
                else
                {
                    initlt[2].Name = "内存";
                    initlt[2].IsInstall = "是";
                    initlt[2].CheckResult = memory.ToString("N2") + "G";
                    initlt[2].IsOk = "符合";
                    initlt[2].AResult = "无建议";
                }
                this.setLoading(100 / (10 + par + num) * 3);

                //.net framework
                changeClTipText("正在检测  .net framework版本");
                LogHelper.Debug("check .Net.");
                int visions = 0;
                frameworkList = MachineHelper.GetNetFramework(out visions);
                int counter = 0;
                foreach (string framework in frameworkList)
                {
                    string[] str = framework.Split(';');
                    if (initlt[3].CheckResult.Contains("Windows 7"))
                    {
                        switch (visions)
                        {
                            case 1:
                                initlt[3].Name = ".NET FRAMEWORK";
                                initlt[3].IsInstall = "否";
                                initlt[3].CheckResult = framework;
                                initlt[3].IsOk = "不符合";
                                initlt[3].AResult = "请先安装.Net Framework 3.5完整软件包";
                                break;
                            case 2:
                                if (!framework.Contains("v3.5"))
                                {
                                    initlt[3].Name = ".NET FRAMEWORK";
                                    initlt[3].IsInstall = "是";
                                    initlt[3].CheckResult = framework;
                                    initlt[3].IsOk = "符合";
                                    initlt[3].AResult = "请单击开始-控制面板-程序和功能-打开或关闭Windows功能，开启.Net Framework 3.5服务";
                                }
                                else
                                {
                                    initlt[3].Name = ".NET FRAMEWORK";
                                    initlt[3].IsInstall = "否";
                                    initlt[3].CheckResult = framework;
                                    initlt[3].IsOk = "不符合";
                                    initlt[3].AResult = "请先安装.Net Framework 3.5完整软件包";
                                }
                                break;
                            case 3:
                                initlt[3].Name = ".NET FRAMEWORK";
                                initlt[3].IsInstall = "是";
                                initlt[3].CheckResult = framework;
                                initlt[3].IsOk = "符合";
                                initlt[3].AResult = "无建议";
                                break;
                            case -1:
                                initlt[3].Name = ".NET FRAMEWORK";
                                initlt[3].IsInstall = "否";
                                initlt[3].CheckResult = framework;
                                initlt[3].IsOk = "不符合";
                                initlt[3].AResult = "请先卸载.Net Framework，再安装.Net Framework 3.5完整软件包";
                                break;
                        }
                    }
                    else
                    {
                        switch (visions)
                        {   
                            case 3:
                                initlt[3].Name = ".NET FRAMEWORK";
                                initlt[3].IsInstall = "是";
                                initlt[3].CheckResult = framework;
                                initlt[3].IsOk = "符合";
                                initlt[3].AResult = "无建议";
                                break;
                            case -1:
                                initlt[3].Name = ".NET FRAMEWORK";
                                initlt[3].IsInstall = "否";
                                initlt[3].CheckResult = framework;
                                initlt[3].IsOk = "不符合";
                                initlt[3].AResult = "请先卸载.Net Framework，再安装.Net Framework 3.5完整软件包";
                                break;
                            default:
                                initlt[3].Name = ".NET FRAMEWORK";
                                initlt[3].IsInstall = "否";
                                initlt[3].CheckResult = framework;
                                initlt[3].IsOk = "不符合";
                                initlt[3].AResult = "请先安装.Net Framework 3.5完整软件包";
                                break;
                        }
                    }
                    counter++;
                    this.setLoading(100 / (10 + par + num) * 4);
                }


                //分辨率
                changeClTipText("正在检测  分辨率");
                LogHelper.Debug("check resolution.");
                Rectangle resolution = MachineHelper.GetResolution();
                if (true)
                {
                    initlt[4].Name = "分辨率";
                    initlt[4].IsInstall = "是";
                    initlt[4].CheckResult = resolution.Width + "*" + resolution.Height;
                    initlt[4].IsOk = "符合";
                    initlt[4].AResult = "无建议";
                }
                this.setLoading(100 / (10 + par + num) * 5);
                //颜色
                changeClTipText("正在检测  系统颜色");
                LogHelper.Debug("check color.");
                int color = MachineHelper.GetColorDepth();
                if (color >= 16)
                {
                    initlt[5].Name = "系统色彩深度";
                    initlt[5].IsInstall = "是";
                    initlt[5].CheckResult = color + "-bit";
                    initlt[5].IsOk = "符合";
                    initlt[5].AResult = "无建议";
                }
                else if (color >= 8)
                {
                    initlt[5].Name = "系统色彩深度";
                    initlt[5].IsInstall = "是";
                    initlt[5].CheckResult = color + "-bit";
                    initlt[5].IsOk = "不符合";
                    initlt[5].AResult = "为了更好使用本系统，请设置计算机色彩至少在16-bit。";

                    LogHelper.Warn("为了更好使用本系统，请设置计算机色彩至少在16-bit。");
                }
                else
                {
                    initlt[5].Name = "系统色彩深度";
                    initlt[5].IsInstall = "是";
                    initlt[5].CheckResult = color + "-bit";
                    initlt[5].IsOk = "不符合";
                    initlt[5].AResult = "目前系统颜色设置小于8-bit，为了更好使用本系统，请设置计算机色彩至少在16-bit。";
                }
                this.setLoading(100 / (10 + par + num) * 6);
                //操作系统



                //浏览器
                changeClTipText("正在检测  浏览器版本");
                object[] Browser = MachineHelper.GetBrowser();
                double browserVer = (double)Browser[0];
                if (browserVer >= IE_Version)
                {
                    initlt[6].Name = "浏览器";
                    initlt[6].IsInstall = "是";
                    initlt[6].CheckResult = (string)Browser[1];
                    initlt[6].IsOk = "符合";
                    initlt[6].AResult = "无建议";
                }
                else
                {
                    //WarningWin win = new WarningWin("警告", WARN, (string)Browser[1], "需要安装Internet Explorer 6.x以上版本，这样才能在本系统使用农行签约、解约和出入金功能。");

                    initlt[6].Name = "浏览器";
                    initlt[6].IsInstall = "是";
                    initlt[6].CheckResult = (string)Browser[1];
                    initlt[6].IsOk = "不符合";
                    initlt[6].AResult = "需要安装Internet Explorer 6.x以上版本，这样才能在本系统使用农行签约、解约和出入金功能。";

                    LogHelper.Warn("需要安装Internet Explorer 6.x以上版本，这样才能在本系统使用农行签约、解约和出入金功能。");
                }
                this.setLoading(100 / (10 + par + num) * 7);

                //http1.1
                changeClTipText("正在检测  http1.1");
                LogHelper.Debug("check HTTP.");
                if (MachineHelper.IsEnableHttp1_1())
                {
                    //SuccessWin win = new SuccessWin("HTTP", "HTTP1.1 Enabled.");

                    initlt[7].Name = "HTTP";
                    initlt[7].IsInstall = "是";
                    initlt[7].CheckResult = "HTTP1.1 Enabled.";
                    initlt[7].IsOk = "符合";
                    initlt[7].AResult = "无建议";

                }
                else
                {


                    initlt[7].Name = "HTTP";
                    initlt[7].IsInstall = "是";
                    initlt[7].CheckResult = "未启用.";
                    initlt[7].IsOk = "不符合";
                    initlt[7].AResult = "需要在IE中设置使用HTTP 1.1。打开Internet Explorer，菜单中选择工具-> 选项，选择高级，打开使用HTTP1.1。";

                    LogHelper.Warn("HTTP1.1 检查不通过.");
                }
                this.setLoading(100 / (10 + par + num) * 8);
                changeClTipText("正在检测  操作系统语言");

                //字体大小
                changeClTipText("正在检测  字体大小");
                LogHelper.Debug("check size(not).");
                string size = MachineHelper.GetFontSize();
                if (true)
                {
                    //SuccessWin win = new SuccessWin("字体大小", size);

                    initlt[8].Name = "字体大小";
                    initlt[8].IsInstall = "是";
                    initlt[8].CheckResult = size;
                    initlt[8].IsOk = "符合";
                    initlt[8].AResult = "无建议";

                }
                this.setLoading(100 / (10 + par + num) * 9);


                //字体检查
                changeClTipText("正在检测  包含字体");
                LogHelper.Debug("check YaHei Font.");
                if (MachineHelper.ContainsYaHeiFont())
                {

                    initlt[9].Name = "字体";
                    initlt[9].IsInstall = "是";
                    initlt[9].CheckResult = "微软雅黑";
                    initlt[9].IsOk = "符合";
                    initlt[9].AResult = "无建议";

                }
                else
                {

                    initlt[9].Name = "字体";
                    initlt[9].IsInstall = "否";
                    initlt[9].CheckResult = "微软雅黑";
                    initlt[9].IsOk = "不符合";
                    initlt[9].AResult = "请先下载微软雅黑字体";
                    LogHelper.Warn("未检测到微软雅黑, 没有安装微软雅黑字体.");
                }
                this.setLoading(100 / (10 + par + num) * 10);
                int count = 0;
                //检查ip地址通讯
                foreach (string[] urlinfo in urlList)
                {
                    changeClTipText("正在检测  " + urlinfo[0].Replace("：", "") + "网络通信");
                    String Msg = "";
                    string telnetMsg = MachineHelper.canTelnet(urlinfo[2], Convert.ToInt32(urlinfo[3]));
                    if (telnetMsg == "0000")
                    {

                        initlt[10 + num + count].Name = urlinfo[0] + "端口通信";
                        initlt[10 + num + count].IsInstall = "是";
                        initlt[10 + num + count].CheckResult = "目标端口通信正常";
                        initlt[10 + num + count].IsOk = "符合";
                        initlt[10 + num + count].AResult = "无建议";
                        initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                        this.setLoading((100 / (10 + par + num)) * (11 + counter + count));
                        count++;




                        MachineHelper.canPing(urlinfo[2], out Msg);
                        LogHelper.Info("msg:" + Msg);
                        string msg = "";
                        string lossStr = "";
                        if (Msg.IndexOf("Average = ") >= 0)
                        {
                            msg = Msg.Substring(Msg.IndexOf("Average = ") + 10);
                            lossStr = Msg.Substring(Msg.IndexOf("Lost = "), Msg.IndexOf("%") - Msg.IndexOf("Lost = "));
                        }
                        else
                        {
                            msg = Msg.Substring(Msg.IndexOf("平均 = ") + 5);
                            lossStr = Msg.Substring(Msg.IndexOf("丢失 = "), Msg.IndexOf("%") - Msg.IndexOf("丢失 = "));
                        }
                        LogHelper.Info("msg:" + msg);
                        LogHelper.Info("lossStr:" + lossStr);
                        int ms = Convert.ToInt32(msg.Substring(0, msg.IndexOf("ms")));

                        int lossRate = int.Parse(lossStr.Substring(lossStr.IndexOf("(") + 1));

                        initlt[10 + num + count].Name = urlinfo[0] + "丢包率";
                        initlt[10 + num + count].IsInstall = "是";
                        initlt[10 + num + count].CheckResult = lossRate == 0 ? "无丢包" : "丢包率:" + lossRate;
                        initlt[10 + num + count].IsOk = "符合";
                        initlt[10 + num + count].AResult = lossRate == 0 ? "无建议" : "建议优化网络:" + lossRate;
                        initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                        this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                        count++;

                        initlt[10 + num + count].Name = urlinfo[0] + "网络质量";
                        initlt[10 + num + count].IsInstall = "是";
                        initlt[10 + num + count].CheckResult = (ms <= 100 ? "优" : ms <= 300 ? "中等" : "差");
                        initlt[10 + num + count].IsOk = "符合";
                        initlt[10 + num + count].AResult = (ms <= 100 ? "无建议" : "建议优化网络");
                        initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                        initlt[10 + num + count].Delay = " 延迟" + ms + "ms";
                        this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                        count++;

                        if (urlinfo[0].Contains("查询服务器"))
                        {
                            if (MachineHelper.canUpdate(urlinfo[1] + "://" + urlinfo[2] + ":" + urlinfo[3] + "/" + ClientType.ToLower() + "autoUpdaterUrl.txt"))
                            {
                                initlt[10 + num + count].Name = urlinfo[0].Replace("(查询服务器)", "(更新服务器)") + "端口通信";
                                initlt[10 + num + count].IsInstall = "是";
                                initlt[10 + num + count].CheckResult = "目标端口通信正常";
                                initlt[10 + num + count].IsOk = "符合";
                                initlt[10 + num + count].AResult = "无建议";
                                initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                                this.setLoading(100 / (10 + par + num) * (12 + counter + count));
                                count++;

                                initlt[10 + num + count].Name = urlinfo[0].Replace("(查询服务器)", "(更新服务器)") + "更新服务";
                                initlt[10 + num + count].IsInstall = "是";
                                initlt[10 + num + count].CheckResult = "可以更新客户端";
                                initlt[10 + num + count].IsOk = "符合";
                                initlt[10 + num + count].AResult = "无建议";
                                initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                                this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                                count++;
                            }
                            else
                            {
                                initlt[10 + num + count].Name = urlinfo[0].Replace("(查询服务器)", "(更新服务器)") + "端口通信";
                                initlt[10 + num + count].IsInstall = "是";
                                initlt[10 + num + count].CheckResult = "目标端口无法连通";
                                initlt[10 + num + count].IsOk = "不符合";
                                initlt[10 + num + count].AResult = "建议：检查本地网络";
                                initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                                this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                                count++;

                                initlt[10 + num + count].Name = urlinfo[0].Replace("(查询服务器)", "(更新服务器)") + "更新服务";
                                initlt[10 + num + count].IsInstall = "是";
                                initlt[10 + num + count].CheckResult = "无法更新客户端";
                                initlt[10 + num + count].IsOk = "不符合";
                                initlt[10 + num + count].AResult = "建议：检查本地网络";
                                initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                                this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                                count++;
                            }
                        }
                    }
                    else
                    {
                        initlt[10 + num + count].Name = urlinfo[0] + "端口通信";
                        initlt[10 + num + count].IsInstall = "是";
                        initlt[10 + num + count].CheckResult = "目标端口无法连通";
                        initlt[10 + num + count].IsOk = "不符合";
                        initlt[10 + num + count].AResult = "建议：检查本地网络";
                        initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                        this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                        count++;


                        initlt[10 + num + count].Name = urlinfo[0] + "丢包率";
                        initlt[10 + num + count].IsInstall = "是";
                        initlt[10 + num + count].CheckResult = "丢包率：100%";
                        initlt[10 + num + count].IsOk = "不符合";
                        initlt[10 + num + count].AResult = "建议：检查本地网络";
                        initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                        this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                        count++;

                        initlt[10 + num + count].Name = urlinfo[0] + "网络质量";
                        initlt[10 + num + count].IsInstall = "是";
                        initlt[10 + num + count].CheckResult = "无";
                        initlt[10 + num + count].IsOk = "不符合";
                        initlt[10 + num + count].AResult = "建议：检查本地网络";
                        initlt[10 + num + count].Delay = " 延迟" + "999" + "ms";
                        initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                        this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                        count++;

                        if (urlinfo[0].Contains("查询服务器"))
                        {

                            initlt[10 + num + count].Name = urlinfo[0].Replace("(查询服务器)", "(更新服务器)") + "端口通信";
                            initlt[10 + num + count].IsInstall = "是";
                            initlt[10 + num + count].CheckResult = "目标端口无法连通";
                            initlt[10 + num + count].IsOk = "不符合";
                            initlt[10 + num + count].AResult = "建议：检查本地网络";
                            initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                            this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                            count++;

                            initlt[10 + num + count].Name = urlinfo[0].Replace("(查询服务器)", "(更新服务器)") + "更新服务";
                            initlt[10 + num + count].IsInstall = "是";
                            initlt[10 + num + count].CheckResult = "无法更新客户端";
                            initlt[10 + num + count].IsOk = "不符合";
                            initlt[10 + num + count].AResult = "建议：检查本地网络";
                            initlt[10 + num + count].Ip = " " + urlinfo[2] + ":" + urlinfo[3] + " ";
                            this.setLoading(100 / (10 + par + num) * (11 + counter + count));
                            count++;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);

            }
            finally
            {
                this.setLoading(100);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("检查项目" + "\t" + "是否安装" + "\t" + "检查结果" + "\t" + "是否符合" + "\t" + "建议结果");
                sb.AppendLine();
                foreach (CheckItemModel mode in initlt)
                {
                    sb.AppendLine(mode.ToString());
                }
                ThreadPool.QueueUserWorkItem(o =>
                                                 {
                                                     try
                                                     {
                                                         sb.AppendLine("报告时间:" + DateTime.Now.ToString() +
                                                                       Environment.NewLine);

                                                         OssHelper ossHelper = new OssHelper();
                                                         ossHelper.OssConfig = new OssConfig()
                                                                                   {
                                                                                       AccessId =
                                                                                           accessId,
                                                                                       AccessKey =
                                                                                           assessKey,
                                                                                       BucketName = bucketName
                                                                                   };
                                                         byte[] bt = Encoding.UTF8.GetBytes(sb.ToString());
                                                         string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                                                         string date = DateTime.Now.ToString("yyyyMMdd");
                                                         using (var ms = new MemoryStream())
                                                         {
                                                             ms.Write(bt, 0, bt.Length);
                                                             ms.Flush();
                                                             ms.Seek(0, SeekOrigin.Begin);

                                                             ossHelper.UpLoad(ms,
                                                                              string.Format("{1}/SysCheck_{0}.txt", time,
                                                                                            date));
                                                         }
                                                     }
                                                     catch (Exception ex)
                                                     {
                                                         LogHelper.Error(ex);
                                                     }
                                                 });
            }
        }

        private void setLoading(int val)
        {
            dgvRefresh();
            if (val > 100)
            {
                val = 100;
            }
            this.BeginInvoke(new MethodInvoker(delegate
            {
                progressBar1.Value = val;
            }));
        }

        private void init()
        {
            initlt.Add(new CheckItemModel()
            {
                Name = "操作系统",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            initlt.Add(new CheckItemModel()
            {
                Name = "操作系统语言",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            initlt.Add(new CheckItemModel()
            {
                Name = "内存",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            initlt.Add(new CheckItemModel()
            {
                Name = ".NET FRAMEWORK",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            initlt.Add(new CheckItemModel()
            {
                Name = "分辨率",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            initlt.Add(new CheckItemModel()
            {
                Name = "系统色彩深度",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            initlt.Add(new CheckItemModel()
            {
                Name = "浏览器",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });



            initlt.Add(new CheckItemModel()
            {
                Name = "HTTP",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });



            initlt.Add(new CheckItemModel()
            {
                Name = "字体大小 ",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            initlt.Add(new CheckItemModel()
            {
                Name = "字体",
                IsInstall = "",
                CheckResult = "",
                IsOk = "",
                AResult = ""
            });

            foreach (string[] str in urlList)
            {

                initlt.Add(new CheckItemModel()
                {
                    Name = str[0] + "端口通信",
                    IsInstall = "",
                    CheckResult = "",
                    IsOk = "",
                    AResult = ""
                });

                initlt.Add(new CheckItemModel()
                {
                    Name = str[0] + "丢包率",
                    IsInstall = "",
                    CheckResult = "",
                    IsOk = "",
                    AResult = ""
                });

                initlt.Add(new CheckItemModel()
                {
                    Name = str[0] + "网络质量",
                    IsInstall = "",
                    CheckResult = "",
                    IsOk = "",
                    AResult = ""
                });

                if (str[0].Contains("查询服务器"))
                {

                    initlt.Add(new CheckItemModel()
                    {
                        Name = str[0].Replace("(查询服务器)", "(更新服务器)") + "端口通信",
                        IsInstall = "",
                        CheckResult = "",
                        IsOk = "",
                        AResult = ""
                    });

                    initlt.Add(new CheckItemModel()
                    {
                        Name = str[0].Replace("(查询服务器)", "(更新服务器)") + "更新服务",
                        IsInstall = "",
                        CheckResult = "",
                        IsOk = "",
                        AResult = ""
                    });
                }
            }

            dgvCheckItem.DataSource = initlt;
            dgvCheckItem.Refresh();
        }



        private void btnCheck_Click(object sender, EventArgs e)
        {
            btnCheck.Enabled = false;
            this.clearList();
            backgroundWorker1.RunWorkerAsync();

        }

        private void clearList()
        {
            if (initlt[0].AResult + "" != "")
            {
                foreach (CheckItemModel mo in initlt)
                {
                    mo.IsInstall = "";
                    mo.CheckResult = "";
                    mo.IsOk = "";
                    mo.AResult = "";
                }
                this.dgvRefresh();
            }
        }

        private delegate void Simple();
        private void dgvRefresh()
        {
            if (this.InvokeRequired)
            {
                Simple s = dgvRefresh;
                this.Invoke(s);
            }
            else
            {
                dgvCheckItem.Refresh();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHelpDesk_Click(object sender, EventArgs e)
        {
            Process.Start(helpDesk);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            check();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            setProcess(e.ProgressPercentage);
        }

        private delegate void setProcessDel(int i);
        private void setProcess(int i)
        {
            if (this.InvokeRequired)
            {
                setProcessDel s = setProcess;
                this.Invoke(s, new object[] { i });
            }
            else
            {

                progressBar1.Value += i;
            }
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clTip.Text = "系统信息检查完毕！";
            btnCheck.Enabled = true;
        }

        private void dgvCheckItem_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < initlt.Count)
            {
                if (initlt.Count > 0)
                {
                    if (initlt[e.RowIndex].IsOk == "不符合")
                    {
                        dgvCheckItem.Rows[e.RowIndex].Cells[3].Style.ForeColor = Color.Red;
                    }
                    if (initlt[e.RowIndex].AResult.Contains("安装.Net Framework 3.5完整软件包") || initlt[e.RowIndex].AResult == "请先下载微软雅黑字体")
                    {
                        if (dgvCheckItem.Rows[e.RowIndex].Cells[4].Style.ForeColor != Color.Red)
                        {
                            dgvCheckItem.Rows[e.RowIndex].Cells[4].Style.Font = new Font(Font.Name, Font.Size, FontStyle.Underline);
                            dgvCheckItem.Rows[e.RowIndex].Cells[4].Style.ForeColor = Color.Blue;
                        }
                    }
                }
            }
        }

        private void dgvCheckItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string text = dgvCheckItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (text.Contains("安装.Net Framework 3.5完整软件包"))
                {
                    dgvCheckItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    System.Diagnostics.Process.Start("http://www.microsoft.com/zh-cn/download/details.aspx?id=25150");

                }
                else if (text == "请先下载微软雅黑字体")
                {
                    dgvCheckItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    System.Diagnostics.Process.Start("http://neapme-client-install.oss.aliyuncs.com/MSYH_V4_Install.rar");

                }
            }
        }

        private void dgvCheckItem_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string text = Convert.ToString(dgvCheckItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (text.Contains("安装.Net Framework 3.5完整软件包") || text == "请先下载微软雅黑字体")
                {
                    this.Cursor = Cursors.Hand;
                }
            }
        }

        private void dgvCheckItem_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}