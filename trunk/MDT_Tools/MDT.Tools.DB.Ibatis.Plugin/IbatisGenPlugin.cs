using System;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.DB.Common;
using MDT.Tools.DB.Ibatis.Plugin.Gen;

namespace MDT.Tools.DB.Ibatis.Plugin
{
    public class IbatisGenPlugin : DBSubPlugin
    {
        #region 插件信息
        private int _tag=7;
        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 18; }
        }

        public override string PluginName
        {
            get { return "Ibatis生成插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构生成Ibatis."; }
        }

        public override string Author
        {
            get { return "xyb"; }
        }

        #endregion
        protected override void AddContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                Action method = new Action(AddContextMenu);
                this.Application.MainContextMenu.Invoke(method, null);
            }
            else
            {
                base.AddContextMenu();
                this._tsiGen.Text = "Ibatis生成";
                this._tsiGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                this._tsiGen.Click += new EventHandler(_tsiIbatis_Click);
            }
        }

        void _tsiIbatis_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];
            GenIbatis(drTable);
        }

        private void GenIbatis(DataRow[] drTable)
        {
            process(drTable,new GenIbatis());
        }
    }
}