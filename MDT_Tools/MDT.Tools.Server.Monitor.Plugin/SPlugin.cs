using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Resources;
using MDT.Tools.Server.Monitor.Plugin.Model;


namespace MDT.Tools.Server.Monitor.Plugin
{
    public class SPlugin : AbstractPlugin
    {
        #region 插件信息


        private int _tag = 1101;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 1101; }
        }

        public override string PluginName
        {
            get { return "进程监控插件"; }
        }

        public override string Description
        {
            get { return "通过连接监控服务器，实时监控服务进程网络情况"; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion
        internal static S_ConfigInfo configInfo;
       public SPlugin()
       {
           configInfo = IniConfigHelper.ReadConfigInfo();
           
       }

        protected override void load()
        {
            
            Consts.SoundLocation = System.Windows.Forms. Application.StartupPath + configInfo.SoundAlert;
            if (configInfo.Show)
            {
                AddLoad();
            }
        }

        private void AddLoad()
        {


            for (int i = 0; i < configInfo.SConfigInfoNum; i++)
            {
                ConfigInfo config = configInfo.ConfigInfo[i];
                var client = new SRControl() { Text = config.SMonitorName,ip = config.Ip,port = config.Port};
                //client.Show(Application.Panel);

                ToolStripMenuItem btnMonitor = new ToolStripMenuItem();
                btnMonitor.Text = config.SMonitorName;
                btnMonitor.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                btnMonitor.Image = Resources.monitor;
                btnMonitor.Click += new EventHandler(btnMonitor_Click);
                btnMonitor.Tag = config.Ip + "|" + config.Port + "|" + config.SMonitorName;
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
                var client = new SRControl() { Text = ipAndport[2], ip=ipAndport[0], port=port };
                client.Show(Application.Panel);
            }

        }

    }
}
