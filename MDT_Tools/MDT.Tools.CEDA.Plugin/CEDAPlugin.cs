using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Plugin;
using System.IO;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;
using System.Runtime.InteropServices;

namespace MDT.Tools.CEDA.Plugin
{
    public class CEDAPlugin : AbstractPlugin
    {

        #region 插件信息

        private int _tag = 71;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 71; }
        }

        public override string PluginName
        {
            get { return "CEDA插件"; }
        }

        public override string Description
        {
            get { return "通过CEDA api请求后台服务，订阅后台实时数据，发布数据"; }
        }

        public override string Author
        {
            get { return "谢彦彬"; }
        }

        #endregion

       
        private string clientType = "";
        private static readonly string LocalEnv = System.Windows.Forms.Application.StartupPath + "\\control\\LocalEnv.ini";
        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);
        protected override void load()
        {
            AddTool();
            try
            {
                clientType = ReadIniData("Config", "clientType", "E", LocalEnv);
                   
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }

        #region
        public  string ReadIniData(string Section,string Key,string NoText,string iniFilePath)
        {
            if(File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section,Key,NoText,temp,1024,iniFilePath);
                return temp.ToString();
            }
            else
            {
                return "E";
            }
        }

        #endregion


        protected delegate void Simple();
        readonly ToolStripMenuItem _tsbRequest = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsbCEDA = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsbSubscribe = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsbPublish=new ToolStripMenuItem();
        protected void AddTool()
        {
            if (Application.MainMenu.InvokeRequired)
            {
                var s = new Simple(AddTool);
                Application.MainMenu.Invoke(s, null);
            }
            else
            {

                foreach (ToolStripMenuItem v in Application.MainMenu.Items)
                {
                    if (v.Text == "工具(&T)")
                    {
                        v.DropDownItems.Add(_tsbCEDA);
                        _tsbCEDA.Text = "CEDA";
                        _tsbRequest.Text = "请求/响应";
                        _tsbRequest.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsbRequest.Click += new EventHandler(_tsbRequest_Click);
                        _tsbCEDA.DropDownItems.Add(_tsbRequest);

                        _tsbSubscribe.Text = "订阅/退订";
                        _tsbSubscribe.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsbSubscribe.Click += new EventHandler(_tsbSubscribe_Click);
                        _tsbCEDA.DropDownItems.Add(_tsbSubscribe);

                        _tsbPublish.Text = "发布";
                        _tsbPublish.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsbPublish.Click += new EventHandler(_tsbPublish_Click);
                        _tsbCEDA.DropDownItems.Add(_tsbPublish);
                        break;
                    }

                }

            }
        }

        void _tsbPublish_Click(object sender, EventArgs e)
        {
            var client = new PubClient {Text = _tsbPublish.Text};
            client.clientType = this.clientType;
            client.Show(Application.Panel);
        }

        void _tsbSubscribe_Click(object sender, EventArgs e)
        {
            var client = new SubscribeClient() { Text = _tsbSubscribe.Text };
            client.clientType = this.clientType;
            client.Show(Application.Panel);
        }

        void _tsbRequest_Click(object sender, EventArgs e)
        {
            var client = new RequestClient() { Text = _tsbRequest.Text };
            client.clientType = this.clientType;
            client.Show(Application.Panel);
        }
    }
}
