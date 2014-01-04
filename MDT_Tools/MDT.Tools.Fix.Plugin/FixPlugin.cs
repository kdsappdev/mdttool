using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Fix.Plugin.UI;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.Fix.Plugin
{
    public class FixPlugin : AbstractPlugin
    {
        #region 插件信息
        private int _tag = 21;
        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 21; }
        }

        public override string PluginName
        {
            get { return "Fix信息插件"; }
        }

        public override string Description
        {
            get { return "加载Fix Xml信息"; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }
        #endregion

        private FixExplorer _explorer = null;
        private MenuStrip _mainTool = null;
        protected override void load()
        {
            _mainTool = Application.MainMenu;
            foreach (IDockContent content in Application.Panel.Contents)
            {
                _explorer = content as FixExplorer;
            }
            if (_explorer == null)
            {
                _explorer = new FixExplorer();
                _explorer.Show(Application.Panel, DockState.DockLeftAutoHide);
            }
            _explorer.Text = "Fix协议信息";

        }

       

    }
}
