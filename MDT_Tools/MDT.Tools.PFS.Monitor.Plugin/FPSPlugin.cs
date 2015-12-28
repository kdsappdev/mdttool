using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Resources;
using MDT.Tools.PFS.Monitor.Plugin.ConfigMsg;

namespace MDT.Tools.PFS.Monitor.Plugin
{
    public class FPSPlugin : AbstractPlugin
    {
        #region 插件信息


        private int _tag = 99;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 99; }
        }

        public override string PluginName
        {
            get { return "FPS监控插件"; }
        }

        public override string Description
        {
            get { return "通过CEDA api请求后台服务，订阅后台实时数据，发布数据"; }
        }

        public override string Author
        {
            get { return "黄镇"; }
        }

        #endregion
        private PFS_ConfigInfo configInfo;
        protected override void load()
        {
            configInfo = IniConfigHelper.ReadConfigInfo();
            if (configInfo.Show)
            {
                AddLoad();
            }
        }

        private void AddLoad()
        {


            for (int i = 0; i < configInfo.PFSConfigInfoNum; i++)
            {
                ConfigInfo config = configInfo.ConfigInfo[i];
                var client = new PriceFeederMonitorUI(config.Ip, config.Port) { Text = config.PFSMonitorName };
                //client.Show(Application.Panel);

                ToolStripMenuItem btnMonitor = new ToolStripMenuItem();
                btnMonitor.Text = config.PFSMonitorName;
                btnMonitor.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                btnMonitor.Image = Resources.monitor;
                btnMonitor.Click += new EventHandler(btnMonitor_Click);
                btnMonitor.Tag = config.Ip + "|" + config.Port + "|" + config.PFSMonitorName;
                Application.MainTool.Items.Insert(i, btnMonitor);

            }


        }

        void btnMonitor_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string[] ipAndport = item.Tag.ToString().Split('|');

            if (ipAndport.Count() == 3)
            {
                int port = 0;
                int.TryParse(ipAndport[1], out port);
                var client = new PriceFeederMonitorUI(ipAndport[0], port) { Text = ipAndport[2] };
                client.Show(Application.Panel);
            }

        }

    }
}
