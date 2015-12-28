using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Aliyun.Monitor.Plugin.Monitor;
using System.Runtime.InteropServices;
using MDT.Tools.Core.Utils;
using System.IO;
using MDT.Tools.Aliyun.Monitor.Plugin.Quartz;
using MDT.Tools.Aliyun.Monitor.Plugin.Model;
using MDT.Tools.Aliyun.Monitor.Plugin.Utils;
using WeifenLuo.WinFormsUI.Docking;
using System.Configuration;

namespace MDT.Tools.Aliyun.Monitor.Plugin
{
    public  class MonitorPlugin : AbstractPlugin
    {

        #region 插件信息

        private int _tag = 111;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 111; }
        }

        public override string PluginName
        {
            get { return "MonitorPlugin插件"; }
        }

        public override string Description
        {
            get { return "通过 Aliyun API 请求连接，获取要监控的文件。"; }
        }

        public override string Author
        {
            get { return "刘学文"; }
        }

        #endregion
        internal static string VKInterval = "";
        private static int number = 0;
        internal static string DownloadPath = "";
        private string IsShowOn { get; set; }


        protected override void load()
        {
            AddTool();
        }
        public MonitorPlugin()
        {
           string num = ReadFile.getIniStr("MonitorNum", "MonitorInfoNum", "");
            int.TryParse(num, out number);
            for (int i = 1; i <= number; i++)
            {
                ToolStripMenuItem _tsbmo1 = new ToolStripMenuItem();
                _tsbmo1.Tag = i;
                LoadInfo info = new LoadInfo();
                try
                {
                    string pubulicKey = ReadFile.getIniStr("Monitor", "Accessid" + i, "");
                    string pwd = ReadFile.getIniStr("Monitor", "Accesskeys" + i, "");
                    string[] strs = MDT.Tools.Aliyun.Monitor.Plugin.Utils.BigInteger.DecryptRASString(pwd, pubulicKey).Split('|');
                    if (strs != null && strs.Length >= 3)
                    {
                        info.id = strs[0];
                        info.key = strs[1];
                        info.bucketName = strs[2];
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.Message);
                }
                info.MonitorFolder = ReadFile.ReadIniData("Monitor", "MonitorFolder" + i, "");
                string monitorName = ReadFile.getIniStr("Monitor", "MonitorName" + i, "");
                if (string.IsNullOrEmpty(info.MonitorFolder))
                {
                    info.MonitorFolder = DateTime.Today.ToString("yyyyMMdd");
                }
                if (string.IsNullOrEmpty(monitorName))
                {
                    monitorName = "监控" + i;
                }
                info.MonitorName = monitorName;
                dicLoad.Add(i, info);
            }

            VKInterval = ReadFile.ReadIniData("Quartz", "VKInterval", "");
            DownloadPath = ReadFile.ReadIniData("MonitorNum", "DownloadPath", "");
        }

        void _tsbmo1_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tl = (ToolStripMenuItem)sender;
            int i = (int)tl.Tag;
            IsShowOn = ReadFile.getIniStr("Monitor", "IsShowOn" + i, "");

                if (dicLoad.ContainsKey(i))
                {
                    LoadInfo li = dicLoad[i];
                    if (!IsThere(tl.Text))
                    {
                        var mu = new MonitorUI() { Text = tl.Text };
                        mu.id = li.id;
                        mu.key = li.key;
                        mu.bucketName = li.bucketName;
                        mu.MonitorFolder = li.MonitorFolder;
                        mu.VKInterval = VKInterval;
                        mu.DownloadPath = DownloadPath;

                        if (string.IsNullOrEmpty(mu.id) || string.IsNullOrEmpty(mu.key) || string.IsNullOrEmpty(mu.bucketName)
                            || string.IsNullOrEmpty(mu.MonitorFolder) || string.IsNullOrEmpty(mu.VKInterval))
                        {
                            LogHelper.Info("Parameter space.");
                            MessageBox.Show("参数不能为空。");

                        }
                        else if (IsShowOn == "true")
                        {
                            mu.Show(Application.Panel);
                            mu.clearPage(1);
                        }
                        else if (IsShowOn == "false")
                        {
                            mu.Show(Application.Panel);
                            mu.clearPage(0);
                        }

                        else
                        {
                            mu.Show(Application.Panel);
                        }
                    }
                }
            }
        private bool IsThere(string  text)
        {
            bool b = false;
            IDockContent[] documents = Application.Panel.DocumentsToArray();
            foreach (var v in documents)
            {
                if (text == v.DockHandler.TabText.ToString())
                {
                    b = true;
                    v.DockHandler.Show();
                    
                }
            }
            return b;
        }


        protected delegate void Simple();
        readonly ToolStripMenuItem _tsbMonitor = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsbMonitorb = new ToolStripMenuItem();
        internal static Dictionary<int, LoadInfo>  dicLoad = new Dictionary<int, LoadInfo>();
        protected void AddTool()
        {
            if (Application.MainMenu.InvokeRequired)
            {
                var s = new Simple(AddTool);
                Application.MainMenu.Invoke(s, null);
            }
            else
            {
                string name = ReadFile.getIniStr("MonitorNum", "MonitorName", "");
                foreach (ToolStripMenuItem v in Application.MainMenu.Items)
                {
                    if (v.Text == "工具(&T)")
                    {
                        v.DropDownItems.Add(_tsbMonitor);
                        _tsbMonitor.Text = name;
                        _tsbMonitor.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                      //  _tsbMonitor.Click += new EventHandler(_tsbMonitor_Click);
                        break;
                    }

                }
                _tsbMonitorb.Text = name;
                for (int i = 1; i <= number; i++)
                {
                    ToolStripMenuItem _tsbmo1 = new ToolStripMenuItem();
                    _tsbmo1.Tag = i;
                    LoadInfo info = dicLoad[i];
                    _tsbmo1.Text = info.MonitorName;
                    _tsbmo1.Click += new EventHandler(_tsbmo1_Click);
                    _tsbMonitorb.DropDownItems.Add(_tsbmo1);
                    _tsbMonitor.DropDownItems.Add(_tsbmo1);
                }

                    
            }
        }

    }
}
