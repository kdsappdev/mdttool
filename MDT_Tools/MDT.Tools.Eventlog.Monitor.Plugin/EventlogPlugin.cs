using System;
using System.Linq;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Eventlog.Monitor.Plugin.Model;


namespace MDT.Tools.Eventlog.Monitor.Plugin
{
    public class EventlogPlugin : AbstractPlugin
    {
        protected readonly ToolStripMenuItem _tsiGen = new ToolStripMenuItem();
        protected delegate void Simple();
        
        #region 插件信息

        private int _tag = 12315;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 12315; }
        }

        public override string PluginName
        {
            get { return "事件监听插件"; }
        }

        public override string Description
        {
            get { return "根据服务器发过来的信息，分等级的显示信息"; }
        }

        public override string Author
        {
            get { return "王兴龙"; }
        }

        #endregion

        #region



        private Event_ConfigInfo configInfo;
        private  ConfigInfo config;
        protected override void load()
        {
            configInfo = IniConfigHelper.ReadConfigInfo();
            if (configInfo.Show)
            {
                AddContextMenu();
           }
        }


        #region 增加上下文菜单

        

        protected  void AddContextMenu()
        {
             if (Application.MainContextMenu.InvokeRequired)
            {
                var s = new Simple(AddContextMenu);
                Application.MainContextMenu.Invoke(s, null);
            }
            else
            {
                foreach (ToolStripMenuItem v in Application.MainMenu.Items)
                {

                    if (v.Text == "工具(&T)")
                    {
                        v.DropDownItems.Add(_tsiGen);
                        _tsiGen.Text = "事件监听日志查询";
                        _tsiGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsiGen.Click+=new EventHandler(_tsiGen_Click);
                        break;
                    }
                }
            }
        }

        private void _tsiGen_Click(object sender, EventArgs e)
        {
            string[] ipAndport = new string[3] { configInfo.Ip,configInfo.Port.ToString(),configInfo.SerId};
            if (ipAndport.Count() == 3)
            {
                int port = 0;
                int.TryParse(ipAndport[1], out port);
                var form = new Form3(ipAndport[0], port) { Text = _tsiGen.Text };

                form.Show(Application.Panel);
            }
        }
        
        #endregion


        #endregion
    }
}