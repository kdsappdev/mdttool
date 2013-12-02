using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Gen;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils;
using MDT.Tools.DB.Csharp_ModelGen.Plugin.UI;
 

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin
{
    public class Csharp_CodeGenPlugin : AbstractPlugin
    {
        #region 插件信息

        private int _tag = 3;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 13; }
        }

        public override string PluginName
        {
            get { return "Csharp代码生成插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构生成Csharp_Model代码类."; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion

        #region load,unload
        protected override void load()
        {
            AddConfig();
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

        #region 增加配置
        Csharp_CodeGenConfigUI cmcUI=new Csharp_CodeGenConfigUI();
        private void AddConfig()
        {
            var tabControl = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TapControl) as TabControl;
            TabPage page=new TabPage("Csharp项目配置");
            cmcUI.Dock = DockStyle.Fill;
            page.Controls.Add(cmcUI);
            tabControl.Controls.Add(page);
            var button = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_BtnSave) as Button;
            button.Click += button_Click;
        }

        void button_Click(object sender, EventArgs e)
        {
            try
            {
                cmcUI.Save();
            }
            catch (Exception ex)
            {

                MessageBox.Show("保存失败["+ex.Message+"]", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        #endregion

        #region 增加上下文菜单
        private readonly ToolStripMenuItem _tsiGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsiModelGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsiDALGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsiBLLGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsispringConfigGen = new ToolStripMenuItem();
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
                _tsiGen.Text = "Csharp代码生成";
                _tsiModelGen.Text = "Model代码生成";
                _tsiDALGen.Text = "DALWebService代码生成";
                _tsiBLLGen.Text = "BLL&GUI代码生成";
                _tsispringConfigGen.Text = "Spring配置生成";
                _tsiGen.Enabled = false;
                _tsiGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsiGen.DropDownItems.AddRange(new[] { _tsiModelGen, _tsiDALGen,_tsiBLLGen, _tsispringConfigGen });
                 _tsiModelGen.Click +=(_tsiModelGen_Click);
                 _tsiDALGen.Click += (_tsiDALGen_Click);
                 _tsiBLLGen.Click += new EventHandler(_tsiBLLGen_Click);
                 _tsispringConfigGen.Click += (_tsispringConfigGen_Click);
                Application.MainContextMenu.Items.Add(_tsiGen);
            }
        }

        void _tsiBLLGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenBLLAndGUI(drTable));
        }
        void _tsispringConfigGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenCsharpSpringConfig(drTable));
        }

        void _tsiDALGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenDALServer(drTable));
        }

        void _tsiModelGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => Gen(drTable));
        }
        private void GenCsharpSpringConfig(DataRow[] drTable)
        {
            var gen = new GenCsharpSpringConfig();
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
            var originalEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_OriginalEncoding) as string;
            var targetEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TargetEncoding) as string;

            gen.tsiGen = _tsiGen;
            gen.DBtable = dBtable;
            gen.DBtablesColumns = dBtablesColumns;
            gen.DBviews = dBviews;
            gen.DBtablesPrimaryKeys = dBtablesPrimaryKeys;
            gen.dbName = dbName;
            gen.dbType = dbType;
            gen.tsslMessage = tsslMessage;
            gen.tspbLoadDBProgress = tspbLoadDBProgress;
            gen.MainContextMenu = Application.MainContextMenu;
            gen.Panel = Application.Panel;
            gen.dsTableColumn = dsTableColumn;
            gen.dsTablePrimaryKey = dsTablePrimaryKey;
            gen.PluginName = PluginName + "(V" + Version + ")";
            if (!string.IsNullOrEmpty(originalEncoding) && !string.IsNullOrEmpty(targetEncoding))
            {
                gen.OriginalEncoding = Encoding.GetEncoding(originalEncoding);
                gen.TargetEncoding = Encoding.GetEncoding(targetEncoding);
            }
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();

            gen.GetSpringConfig(drTable, dsTableColumn, dsTablePrimaryKey);
        }
        private void GenDALServer(DataRow[] drTable)
        {
            var gen = new GenCsharpDALWebService ();
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
            var originalEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_OriginalEncoding) as string;
            var targetEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TargetEncoding) as string;

            gen.tsiGen = _tsiGen;
            gen.DBtable = dBtable;
            gen.DBtablesColumns = dBtablesColumns;
            gen.DBviews = dBviews;
            gen.DBtablesPrimaryKeys = dBtablesPrimaryKeys;
            gen.dbName = dbName;
            gen.dbType = dbType;
            gen.tsslMessage = tsslMessage;
            gen.tspbLoadDBProgress = tspbLoadDBProgress;
            gen.MainContextMenu = Application.MainContextMenu;
            gen.Panel = Application.Panel;
            gen.dsTableColumn = dsTableColumn;
            gen.dsTablePrimaryKey = dsTablePrimaryKey;
            gen.PluginName = PluginName + "(V" + Version + ")";
            if (!string.IsNullOrEmpty(originalEncoding) && !string.IsNullOrEmpty(targetEncoding))
            {
                gen.OriginalEncoding = Encoding.GetEncoding(originalEncoding);
                gen.TargetEncoding = Encoding.GetEncoding(targetEncoding);
            }
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();

            gen.GenCode(drTable, dsTableColumn, dsTablePrimaryKey);
        }
        private void GenBLLAndGUI(DataRow[] drTable)
        {
            var gen = new GenCsharpBLLAndGUI();
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
            var originalEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_OriginalEncoding) as string;
            var targetEncoding = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TargetEncoding) as string;

            gen.tsiGen = _tsiGen;
            gen.DBtable = dBtable;
            gen.DBtablesColumns = dBtablesColumns;
            gen.DBviews = dBviews;
            gen.DBtablesPrimaryKeys = dBtablesPrimaryKeys;
            gen.dbName = dbName;
            gen.dbType = dbType;
            gen.tsslMessage = tsslMessage;
            gen.tspbLoadDBProgress = tspbLoadDBProgress;
            gen.MainContextMenu = Application.MainContextMenu;
            gen.Panel = Application.Panel;
            gen.dsTableColumn = dsTableColumn;
            gen.dsTablePrimaryKey = dsTablePrimaryKey;
            gen.PluginName = PluginName + "(V" + Version + ")";
            if (!string.IsNullOrEmpty(originalEncoding) && !string.IsNullOrEmpty(targetEncoding))
            {
                gen.OriginalEncoding = Encoding.GetEncoding(originalEncoding);
                gen.TargetEncoding = Encoding.GetEncoding(targetEncoding);
            }
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();

            gen.GenCode(drTable, dsTableColumn, dsTablePrimaryKey);
        }
        private void Gen(DataRow[] drTable)
        {
            var gen = new GenCsharpModel();
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
            var originalEncoding=getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_OriginalEncoding) as string;
            var targetEncoding=getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TargetEncoding) as string;
             
            gen.tsiGen = _tsiGen;
            gen.DBtable = dBtable;
            gen.DBtablesColumns = dBtablesColumns;
            gen.DBviews = dBviews;
            gen.DBtablesPrimaryKeys = dBtablesPrimaryKeys;
            gen.dbName = dbName;
            gen.dbType = dbType;
            gen.tsslMessage = tsslMessage;
            gen.tspbLoadDBProgress = tspbLoadDBProgress;
            gen.MainContextMenu = Application.MainContextMenu;
            gen.Panel = Application.Panel;
            gen.dsTableColumn = dsTableColumn;
            gen.dsTablePrimaryKey = dsTablePrimaryKey;
            gen.PluginName = PluginName+"(V"+Version+")";
            if (!string.IsNullOrEmpty(originalEncoding) && !string.IsNullOrEmpty(targetEncoding))
            {
                gen.OriginalEncoding = Encoding.GetEncoding(originalEncoding);
                gen.TargetEncoding = Encoding.GetEncoding(targetEncoding);
            }
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            
            gen.GenCode(drTable, dsTableColumn, dsTablePrimaryKey);
        }
        #endregion
    }
}
