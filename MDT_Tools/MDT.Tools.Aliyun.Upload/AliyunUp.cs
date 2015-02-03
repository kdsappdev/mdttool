using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Aliyun.Upload.Plugin.UploadUI;

namespace MDT.Tools.Aliyun.Upload.Plugin
{
    public class AliyunUp : AbstractPlugin
    {
        #region 插件信息

        private int _tag = 72;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 72; }
        }

        public override string PluginName
        {
            get { return "AliyunUp插件"; }
        }

        public override string Description
        {
            get { return "通过 Aliyun API 请求连接，可以上传文件或文件夹。"; }
        }

        public override string Author
        {
            get { return "刘学文"; }
        }

        #endregion

        protected override void load()
        {
            AddTool();
        }

        protected delegate void Simple();
        readonly ToolStripMenuItem _tsbUpload = new ToolStripMenuItem();
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
                        v.DropDownItems.Add(_tsbUpload);
                        _tsbUpload.Text = "阿里云上传";
                        _tsbUpload.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsbUpload.Click += new EventHandler(_tsbUpload_Click);
                        break;
                    }

                }

            }
        }

        void _tsbUpload_Click(object sender, EventArgs e)
        {
            var upload = new UploadOSSUI() { Text = _tsbUpload.Text };
            upload.Show(Application.Panel);
        }

        
    }
}
