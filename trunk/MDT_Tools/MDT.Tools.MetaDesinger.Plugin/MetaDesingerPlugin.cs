using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Resources;
using MDT.Tools.DB.Common;
using MDT.Tools.MetaDesinger.Plugin.UI;

namespace MDT.Tools.MetaDesinger.Plugin
{
    public class MetaDesingerPlugin : DBSubPlugin
    {
        #region 插件信息
        private int _tag = 31;
        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 31; }
        }

        public override string PluginName
        {
            get { return "数据设计器"; }
        }

        public override string Description
        {
            get { return "根据数据库表信息，设计服务模型，并生成对应服务代码。"; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }
        #endregion

        private MenuStrip _mainTool = null;
        readonly ToolStripButton _tsbDesinger = new ToolStripButton();
        private void AddTool()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Action(AddTool);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _tsbDesinger.Text = PluginName;
                _tsbDesinger.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsbDesinger.Click += new EventHandler(_tsbDesinger_Click);
                Application.MainTool.Items.Add(_tsbDesinger);
           
            }
        }

        protected override void load()
        {
            _mainTool = Application.MainMenu;
            AddTool();
            isLoad = true;
        }

        void _tsbDesinger_Click(object sender, EventArgs e)
        {
            DataDesingerUI ui=new DataDesingerUI();
            ui.Text = PluginName;
            ui.tableDesingerLayer1.DBSubPlugin = this;
            //ui.desingerUI1.desingerLayer1.DBSubPlugin = this;
            ui.Show(Application.Panel);
        }

    }
}
