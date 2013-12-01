using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.SetComment.Plugin.Utils;
namespace MDT.Tools.DB.SetComment.Plugin
{
    public class SetCommentPlugin : AbstractPlugin
    {
        #region 插件信息

        private int _tag = 5;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 15; }
        }

        public override string PluginName
        {
            get { return "设置表及列描述信息插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构设置表及列描述信息."; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion

        #region load,unload
        protected override void load()
        { 
            AddContextMenu();
            subscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
        }
        public override void BeforeTerminating()
        {
            unsubscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
            Application.MainContextMenu.Items.Remove(_tsiGen);
        }
        #endregion

        #region onNotify
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
                _tsiGen.Enabled = flag;
            }
        }
        #endregion

        #region 增加上下文菜单
        private readonly ToolStripMenuItem _tsiGen = new ToolStripMenuItem();
 
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
                _tsiGen.Text = "表备注修改";
            
                _tsiGen.Enabled = false;
                _tsiGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsiGen.Click += new EventHandler(_tsiGen_Click);
                Application.MainContextMenu.Items.Add(_tsiGen);
            }
        }

        void _tsiGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

             GenJavaSpringConfig(drTable);
            
        }

       

 

        private void GenJavaSpringConfig(DataRow[] drTable)
        {
            var sc = new setComment();
            var dbName = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBName) as string;
            var dbType = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBType) as string;
            var dbConnectionString = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBConnectionString) as string;
            var dsTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBAllTable) as DataSet;
            var dsTableColumn = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBAllTablesColumns) as DataSet;
            var dsTablePrimaryKey = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBTablesPrimaryKeys) as DataSet;

            var dBtable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtable) as string;
            var dBtablesColumns = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtablesColumns) as string;

            var dBviews = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBviews) as string;
            var dBtablesPrimaryKeys = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtablesPrimaryKeys) as string;
            var tsslMessage = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_tsslMessage) as ToolStripStatusLabel;
            var tspbLoadDBProgress = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_tspbLoadDBProgress) as ToolStripProgressBar;
            var originalEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_OriginalEncoding) as string;
            var targetEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TargetEncoding) as string;

            sc.tsiGen = _tsiGen;
            sc.DBtable = dBtable;
            sc.DBtablesColumns = dBtablesColumns;
            sc.DBviews = dBviews;
            sc.DBtablesPrimaryKeys = dBtablesPrimaryKeys;
            sc.dbName = dbName;
            sc.dbType = dbType;
            sc.dbConnectionString = dbConnectionString;
            sc.tsslMessage = tsslMessage;
            sc.tspbLoadDBProgress = tspbLoadDBProgress;
            sc.MainContextMenu = Application.MainContextMenu;
            sc.Panel = Application.Panel;
            sc.dsTable = dsTable;
            sc.dsTableColumn = dsTableColumn;
            sc.dsTablePrimaryKey = dsTablePrimaryKey;
            sc.PluginName = PluginName + "(V" + Version + ")";
            if (!string.IsNullOrEmpty(originalEncoding) && !string.IsNullOrEmpty(targetEncoding))
            {
                sc.OriginalEncoding = Encoding.GetEncoding(originalEncoding);
                sc.TargetEncoding = Encoding.GetEncoding(targetEncoding);
            }
           

            sc.set(drTable, dsTableColumn, dsTablePrimaryKey);
        }
        
        #endregion
    }
}
