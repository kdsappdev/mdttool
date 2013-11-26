using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.DocGen.Plugin.Gen;
using MDT.Tools.DB.DocGen.Plugin.Utils;

namespace MDT.Tools.DB.DocGen.Plugin
{
    public class DocGenPlugin : AbstractPlugin
    {
        #region 插件信息

        private int _tag = 2;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 11; }
        }

        public override string PluginName
        {
            get { return "数据字典生成插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构生成doc文档."; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion

        #region

      



        protected override void load()
        {
            AddContextMenu();
            subscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
        }

        protected override void unload()
        {
            
            unsubscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
            Application.MainContextMenu.Items.Remove(_tsiDocGen);

        }

        #region 增加上下文菜单
        private readonly ToolStripItem _tsiDocGen = new ToolStripMenuItem();
        private delegate void Simple();
        private void AddContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                var s = new Simple(AddContextMenu);
                Application.MainContextMenu.Invoke(s, null);
            }
            else
            {
                _tsiDocGen.Text = "生成数据库文档";
                _tsiDocGen.Enabled = false;
                _tsiDocGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsiDocGen.Click += TsiDocGenClick;
                Application.MainContextMenu.Items.Add(_tsiDocGen);
            }
        }

        void TsiDocGenClick(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenDBWord(drTable));
        }
        private void GenDBWord(DataRow[] drTable)
        {
            var gen = new GenDbWord();
            var dbName = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBName) as string;
            var dbType = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBType) as string;
            var dsTableColumn = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBAllTablesColumns) as DataSet;
            var dsTablePrimaryKey = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBTablesPrimaryKeys) as DataSet;

            var dBtable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtable) as string;
            var dBtablesColumns = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtablesColumns) as string;

            var dBviews = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBviews) as string;
            var dBtablesPrimaryKeys = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtablesPrimaryKeys) as string;
            var tsslMessage = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_tsslMessage) as ToolStripStatusLabel;
            var tspbLoadDBProgress = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_tspbLoadDBProgress) as ToolStripProgressBar;
            

            gen.tsiDocGen = _tsiDocGen;
            gen.DBtable = dBtable;
            gen.DBtablesColumns = dBtablesColumns;
            gen.DBviews = dBviews;
            gen.DBtablesPrimaryKeys = dBtablesPrimaryKeys;
            gen.dbName = dbName;
            gen.dbType = dbType;
            gen.tsslMessage = tsslMessage;
            gen.tspbLoadDBProgress = tspbLoadDBProgress;
            gen.MainContextMenu = Application.MainContextMenu;
            gen.dsTableColumn = dsTableColumn;
            gen.dsTablePrimaryKey = dsTablePrimaryKey;
            gen.GenCode(drTable, dsTableColumn, dsTablePrimaryKey);
        }
        #endregion

        public override void onNotify(string name, object o)
        {
            base.onNotify(name, o);
            if (PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0.Equals(name) && o.GetType().IsValueType)
            {
                var flag = (bool)o;
                SetEnable(flag);
            }
        }

        private delegate void SimpleBool(bool flag);
        private void SetEnable(bool flag)
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                var s = new SimpleBool(SetEnable);
                Application.MainContextMenu.Invoke(s, new object[] { flag });
            }
            else
            {
                _tsiDocGen.Enabled = flag;
            }
        }

        #endregion
    }
}
