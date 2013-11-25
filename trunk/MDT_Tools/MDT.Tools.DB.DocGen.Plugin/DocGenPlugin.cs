using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.UI;
using MDT.Tools.DB.Doc.Plugin.Gen;

namespace MDT.Tools.DB.Doc.Plugin
{
    public class DocGenPlugin : AbstractPlugin
    {
        #region 插件信息

        private int tag = 2;

        public override int Tag
        {
            get { return tag; }
            set { tag = value; }
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

        private ToolStripItem tsiDocGen = new ToolStripMenuItem();

        public override void OnLoading()
        {
            base.OnLoading();
            load();
        }

        private void load()
        {
            if (!isLoad)
            {
                addContextMenu();
                subscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
            }
        }

        private void unload()
        {
            if (isLoad)
            {
                unsubscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
                Application.MainContextMenu.Items.Remove(tsiDocGen);
            }
        }

        #region 增加上下文菜单

        private delegate void Simple();
        private void addContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                Simple s = new Simple(addContextMenu);
                Application.MainContextMenu.Invoke(s, null);
            }
            else
            {
                tsiDocGen.Text = "生成数据库文档";
                tsiDocGen.Enabled = false;
                tsiDocGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                tsiDocGen.Click += new EventHandler(tsiDocGen_Click);
                Application.MainContextMenu.Items.Add(tsiDocGen);
            }
        }

        void tsiDocGen_Click(object sender, EventArgs e)
        {
            DataRow[] drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];
            
            ThreadPool.QueueUserWorkItem(o=> genDBWord(drTable));
        }
        private void genDBWord(DataRow[] drTable)
        {
            GenDbWord genDbWord = new GenDbWord();
            string dbName = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBName) as string;
            string dbType = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBType) as string;
            DataSet dsTableColumn =getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBAllTablesColumns) as DataSet;
            DataSet dsTablePrimaryKey = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentDBTablesPrimaryKeys) as DataSet;
             
            string DBtable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtable) as string;
            string DBtablesColumns = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtablesColumns) as string;
            
            string DBviews = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBviews) as string;
            string DBtablesPrimaryKeys = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBtablesPrimaryKeys) as string;
            ToolStripStatusLabel tsslMessage = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_tsslMessage) as ToolStripStatusLabel;
            ToolStripProgressBar tspbLoadDBProgress = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_tspbLoadDBProgress) as ToolStripProgressBar;
            Explorer Explorer = Application.Explorer;

            genDbWord.tsiDocGen = tsiDocGen;
            genDbWord.DBtable = DBtable;
            genDbWord.DBtablesColumns = DBtablesColumns;
            genDbWord.DBviews = DBviews;
            genDbWord.DBtablesPrimaryKeys = DBtablesPrimaryKeys;
            genDbWord.dbName = dbName;
            genDbWord.dbType = dbType;
            genDbWord.tsslMessage = tsslMessage;
            genDbWord.tspbLoadDBProgress = tspbLoadDBProgress;
            genDbWord.Explorer = Explorer;
            genDbWord.dsTableColumn = dsTableColumn;
            genDbWord.dsTablePrimaryKey = dsTablePrimaryKey;
            genDbWord.GenCode(drTable, dsTableColumn, dsTablePrimaryKey);
            ;

        }
        #endregion
        public override void BeforeTerminating()
        {
            base.BeforeTerminating();
            unload();
        }



        public override void onNotify(string name, object o)
        {
            base.onNotify(name, o);
            if (PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0.Equals(name) && o.GetType().IsValueType)
            {
                bool flag = (bool)o;
                setEnable(flag);
            }
        }

        private delegate void SimpleBool(bool flag);
        private void setEnable(bool flag)
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                SimpleBool s = new SimpleBool(setEnable);
                Application.MainContextMenu.Invoke(s, new object[] { flag });
            }
            else
            {
                tsiDocGen.Enabled = flag;
            }
        }

        #endregion
    }
}
