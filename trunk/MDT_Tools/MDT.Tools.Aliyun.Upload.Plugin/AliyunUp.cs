using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Aliyun.Upload.Plugin.UploadUI;
using MDT.Tools.Aliyun.Upload.Plugin.Download;
using MDT.Tools.Aliyun.Upload.Plugin.Utils;

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

        private MenuStrip _mainTool = null;
        protected override void load()
        {
            _mainTool = Application.MainMenu;
            AddTool();
            AddStatus();
        }


        protected override void unload()
        {
            RemoveStatus();
        }
        protected delegate void Simple();
        readonly ToolStripMenuItem _tsbAliyun = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsbUpload = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsbDownload = new ToolStripMenuItem();
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
                        v.DropDownItems.Add(_tsbAliyun);
                        _tsbAliyun.Text = "阿里云";

                        _tsbUpload.Text = "上传";
                        _tsbUpload.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsbUpload.Click += new EventHandler(_tsbUpload_Click);
                        _tsbAliyun.DropDownItems.Add(_tsbUpload);

                        _tsbDownload.Text = "下载";
                        _tsbDownload.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsbDownload.Click += new EventHandler(_tsbDownload_Click);
                        _tsbAliyun.DropDownItems.Add(_tsbDownload);
                        break;
                    }

                }

            }
        }

        void _tsbDownload_Click(object sender, EventArgs e)
        {
            var download = new DownloadOSSUI() { Text = _tsbDownload.Text };
            download.AliyunUp= this;
            download.Show(Application.Panel);
        }

        void _tsbUpload_Click(object sender, EventArgs e)
        {
            var upload = new UploadOSSUI() { Text = _tsbUpload.Text };
            upload.Show(Application.Panel);
        }


        #region 增加状态栏

        ToolStripStatusLabel _tsslMessage = new ToolStripStatusLabel();
        ToolStripProgressBar _tspbLoadDbProgress = new ToolStripProgressBar();
        private void AddStatus()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(AddStatus);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _tspbLoadDbProgress = getObject(43, "tspbLoadDbProgress") as ToolStripProgressBar;
                _tsslMessage = getObject(43, "tsslMessage") as ToolStripStatusLabel;
                registerObject(PluginShareHelper.TsslMessage, _tsslMessage);
                registerObject(PluginShareHelper.TspbLoadDBProgress, _tspbLoadDbProgress);
            }
        }

        private void RemoveStatus()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(RemoveStatus);
                _mainTool.Invoke(s, null);

            }
            else
            {
                Application.StatusBar.Items.Remove(_tsslMessage);
                Application.StatusBar.Items.Remove(_tspbLoadDbProgress);
                remove(PluginShareHelper.TsslMessage);
                remove(PluginShareHelper.TspbLoadDBProgress);
            }

        }

        void StatusBarSizeChanged(object sender, EventArgs e)
        {
            _tsslMessage.Width = Application.StatusBar.Width - _tspbLoadDbProgress.Width - 20;

        }

        #endregion

        public void SetStatusBar(string str)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleStr(SetStatusBar);
                _mainTool.Invoke(s, new object[] { str });

            }
            else
            {
                _tsslMessage.Text = str;
            }
        }
        public void SetProgreesEditValue(int i)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleInt(SetProgreesEditValue);
                _mainTool.Invoke(s, new object[] { i });
            }
            else
            {
                _tspbLoadDbProgress.Value = i;
            }

        }
        public void SetProgressMax(int i)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleInt(SetProgressMax);
                _mainTool.Invoke(s, new object[] { i });

            }
            else
            {
                _tspbLoadDbProgress.Maximum = i;
            }

        }
        delegate void SimpleInt(int i);
        delegate void SimpleStr(string str);
        public void SetProgress(int i)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleInt(SetProgress);
                _mainTool.Invoke(s, new object[] { i });

            }
            else
            {
                if (i + _tspbLoadDbProgress.Value > _tspbLoadDbProgress.Maximum)
                {
                    _tspbLoadDbProgress.Value = _tspbLoadDbProgress.Maximum;
                }
                else
                {
                    _tspbLoadDbProgress.Value = _tspbLoadDbProgress.Value + i;
                }
            }

        }

        public delegate void SimpleBool(bool flag);
        public void SetEnable(bool flag)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleBool(SetEnable);
                _mainTool.Invoke(s, new object[] { flag });
            }
            else
            {
                _tspbLoadDbProgress.Visible = !flag;
                SetTbDbEnable(flag);
                
              
            }

        }


        public void SetTbDbEnable(bool flag)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleBool(SetTbDbEnable);
                _mainTool.Invoke(s, new object[] { flag });
            }
            else
            {
                Application.MainContextMenu.Enabled = flag;
            }
        }
    }
}
