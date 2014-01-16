using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Resources;
using MDT.Tools.DB.Plugin.Model;
using MDT.Tools.DB.Plugin.UI;
using MDT.Tools.DB.Plugin.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.DB.Plugin
{
    public class DbPlugin : AbstractPlugin
    {
        #region 插件信息
        private int _tag = 1;
        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 1; }
        }

        public override string PluginName
        {
            get { return "数据库信息插件"; }
        }

        public override string Description
        {
            get { return "加载数据库所有脚步信息，目前获取表名，主键，字段等信息"; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }
        #endregion

        #region 数据库配置信息

        private readonly DataSet _dsTable = new DataSet();//所有数据库表信息
        private readonly DataSet _dsTableColumn = new DataSet();//所有表对应的列信息
        private readonly DataSet _dsTablePrimaryKey = new DataSet();//所有表对应的主键信息

        private const string Tables = "_TABLES";
        private const string TablesColumns = "_TABLES_COLUMNS";
        private const string Views = "_VIEWS";
        private const string TablesPrimaryKeys = "_TABLES_PK";

        #endregion

        private readonly TreeView _tvDb = new TreeView();
        private Explorer _explorer = null;


        protected override void unload()
        {

            _dsTable.Clear();
            _dsTableColumn.Clear();
            _dsTablePrimaryKey.Clear();
            ClearTree();
            RemoveShareData();
             
            RemoveTool();
            RemoveStatus();
            RemoveTreeControl();
            _backgroundWorkerLoadDb.DoWork -= BackgroundWorkerLoadDbDoWork;
            _backgroundWorkerLoadDb.RunWorkerCompleted -=
                BackgroundWorkerLoadDbRunWorkerCompleted;
            _backgroundWorkerLoadDb.ProgressChanged -=
                BackgroundWorkerLoadDbProgressChanged;

        }
        private void RemoveShareData()
        {
            remove(PluginShareHelper.TargetEncoding);
            remove(PluginShareHelper.OriginalEncoding);
            remove(PluginShareHelper.DBtable);
            remove(PluginShareHelper.DBtablesColumns);
            remove(PluginShareHelper.DBviews);
            remove(PluginShareHelper.DBtablesPrimaryKeys);

            remove(PluginShareHelper.DBCurrentDBName);
            remove(PluginShareHelper.DBCurrentDBConnectionString);
            remove(PluginShareHelper.DBCurrentDBType);
            remove(PluginShareHelper.DBCurrentCheckTable);
            remove(PluginShareHelper.DBCurrentDBAllTable);
            remove(PluginShareHelper.DBCurrentDBAllTablesColumns);
            remove(PluginShareHelper.DBCurrentDBTablesPrimaryKeys);
        }

        #region 工具栏 增加数据参数配置

        readonly ToolStripButton _tsbDbSet = new ToolStripButton();
        readonly ToolStripComboBox _tscbDbConfig = new ToolStripComboBox();
        readonly ToolStripButton _tsbDbReSet = new ToolStripButton();
        readonly ToolStripSeparator _toolStripSeparator = new ToolStripSeparator();
        readonly ToolStripMenuItem _tsmiSystem = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsmiDbSet = new ToolStripMenuItem();
        readonly ToolStripMenuItem _tsmiDbConfig = new ToolStripMenuItem();
        readonly ToolStripSeparator _tsmiSeparator = new ToolStripSeparator();
        readonly ToolStripMenuItem _tsmiExit = new ToolStripMenuItem();
        private MenuStrip _mainTool = null;
        private void AddTool()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(AddTool);
                _mainTool.Invoke(s, null);
            }
            else
            {

               
                _tsmiSystem.Text = "系统(&S)";
                _tsmiDbSet.Text=_tsbDbSet.Text = "数据库配置";
                _tsmiDbConfig.DisplayStyle = _tsbDbSet.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsbDbSet.Click += tsbDBSet_Click;
                _tsmiDbSet.Click += tsbDBSet_Click;
                _tsmiDbSet.Image = _tsbDbSet.Image = Resources.dbConfig;
                _tscbDbConfig.DropDownStyle = ComboBoxStyle.DropDownList;
                _tscbDbConfig.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tscbDbConfig.SelectedIndexChanged += TscbDbConfigSelectedIndexChanged;

                _tsmiDbConfig.Text=_tsbDbReSet.Text = "重新加载数据库";
                _tsmiDbConfig.DisplayStyle=_tsbDbReSet.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsbDbReSet.Click += TsbDbReSetClick;
                _tsmiDbConfig.Click += TsbDbReSetClick;
                _tsmiDbConfig.Image = _tsbDbReSet.Image = Resources.reload;
                _tsmiExit.Text = "退出(&X)";
                _tsmiExit.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsmiExit.Image = Resources.exit;
                _tsmiExit.Click += new EventHandler(_tsmiExit_Click);
                _tsmiSystem.DropDownItems.Add(_tsmiDbSet);
                _tsmiSystem.DropDownItems.Add( _tsmiDbConfig);
                _tsmiSystem.DropDownItems.Add(_tsmiSeparator);
                _tsmiSystem.DropDownItems.Add(_tsmiExit);
                Application.MainMenu.Items.Insert(0, _tsmiSystem);

                Application.MainTool.Items.Insert(0, _tsbDbSet);
                Application.MainTool.Items.Insert(1, _tscbDbConfig);
                Application.MainTool.Items.Insert(2, _tsbDbReSet);
                Application.MainTool.Items.Insert(3, _toolStripSeparator);
            }
        }

        void _tsmiExit_Click(object sender, EventArgs e)
        {
            Application.MainTool.Items["tsbExit"].PerformClick();
        }
        private void RemoveTool()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(RemoveTool);
                _mainTool.Invoke(s, null);
            }
            else
            {
                Application.MainTool.Items.Remove(_tsbDbSet);
                Application.MainTool.Items.Remove(_tscbDbConfig);
                Application.MainTool.Items.Remove(_tsbDbReSet);
                Application.MainTool.Items.Remove(_toolStripSeparator);
            }
        }

        void TsbDbReSetClick(object sender, EventArgs e)
        {
            Run(true);
        }


        void TscbDbConfigSelectedIndexChanged(object sender, EventArgs e)
        {
            Run(false);
        }
        public DbPlugin()
        {
            _configForm.DBConfigInfoChanged += new ConfigForm.dbConfigInfoChanged(_configForm_DBConfigInfoChanged);
        }

        void _configForm_DBConfigInfoChanged()
        {
            GetDbConfigList();
        }

        ConfigForm _configForm = new ConfigForm();
        void tsbDBSet_Click(object sender, EventArgs e)
        {
            _configForm.ShowDialog();
        }

        #endregion

        #region 增加状态栏

        readonly ToolStripStatusLabel _tsslMessage = new ToolStripStatusLabel();
        readonly ToolStripProgressBar _tspbLoadDbProgress = new ToolStripProgressBar();
        private void AddStatus()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(AddStatus);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _tspbLoadDbProgress.AutoSize = false;
                _tspbLoadDbProgress.Visible = false;
                _tspbLoadDbProgress.Width = 250;

                _tsslMessage.AutoSize = false;
                _tsslMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                _tsslMessage.Width = Application.StatusBar.Width - _tspbLoadDbProgress.Width - 20;
                Application.StatusBar.SizeChanged += StatusBarSizeChanged;
                Application.StatusBar.Items.Insert(0, _tsslMessage);
                Application.StatusBar.Items.Insert(1, _tspbLoadDbProgress);
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

        #region 增加树形控件
        private void AddTreeControl()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(AddTreeControl);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _tvDb.Dock = DockStyle.Fill;
                _tvDb.CheckBoxes = true;
                _tvDb.AfterCheck += TvDbAfterCheck;
                _tvDb.MouseClick += TvDbMouseClick;
                _explorer.Controls.Add(_tvDb);
            }
        }


        void TvDbAfterCheck(object sender, TreeViewEventArgs e)
        {
            bool check = e.Node.Checked;
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = check;
            }

            DataRow[] dr = GetCheckTable();
            registerObject(PluginShareHelper.DBCurrentCheckTable, dr);
            bool flag = false;
            if (dr != null && dr.Length > 0)
            {
                flag = true;
            }
            broadcast(PluginShareHelper.BroadCastCheckTableNumberIsGreaterThan0, flag);
        }

        void TvDbMouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(_tvDb, e.Location);
            }
        }
        private void RemoveTreeControl()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(RemoveTreeControl);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _explorer.Controls.Remove(_tvDb);
            }

        }
        #endregion

        #region 清除树形结构

        private void ClearTree()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(ClearTree);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _tvDb.Nodes.Clear();
            }
        }
        #endregion

        private delegate void SimpleBool(bool flag);
        private void SetEnable(bool flag)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleBool(SetEnable);
                _mainTool.Invoke(s, new object[] { flag });
            }
            else
            {
                _tsbDbSet.Enabled = flag;
                _tsbDbReSet.Enabled = flag;
                _tscbDbConfig.Enabled = flag;
                _tspbLoadDbProgress.Visible = !flag;
                SetTbDbEnable(flag);
            }

        }

        #region 增加节点
        private void BindTreeBySync()
        {
            ThreadPool.QueueUserWorkItem(state =>
                                             {
                                                 AddNodes(_tvDb.Nodes);
                                                 ExandAllTreeNode();
                                             });
        }
        private void ExandAllTreeNode()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(ExandAllTreeNode);
                _mainTool.Invoke(s, null);

            }
            else
            {
                _tvDb.ExpandAll();
            }

        }
        private void AddNodes(TreeNodeCollection collection)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new CreateRootNodeDel(AddNodes);
                _mainTool.Invoke(s, new object[] { collection });

            }
            else
            {
                DbConfigInfo dbConfigInfo = GetCurenctDbConfigInfo();
                if (dbConfigInfo != null)
                {

                    for (int i = 0; i < _dsTable.Tables.Count; i++)
                    {
                        if (_dsTable.Tables[i].TableName.Equals(dbConfigInfo.DbConfigName + Tables))
                        {
                            AddNodes(collection[0].Nodes, _dsTable.Tables[i]);
                        }
                        if (_dsTable.Tables[i].TableName.Equals(dbConfigInfo.DbConfigName + Views))
                        {
                            AddNodes(collection[1].Nodes, _dsTable.Tables[i]);
                        }
                    }
                }
            }
        }

        private delegate void CreateRootNodeDel(TreeNodeCollection collection);
        private void CreateRootNode(TreeNodeCollection collection)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new CreateRootNodeDel(CreateRootNode);
                _mainTool.Invoke(s, new object[] { collection });

            }
            else
            {
                var tablesNode = new TreeNode { Text = TagType.Tables.ToString(), Tag = TagType.Tables };
                AddTreeNode(collection, tablesNode);

                var viewsNode = new TreeNode { Text = TagType.Views.ToString(), Tag = TagType.Views };
                AddTreeNode(collection, viewsNode);
            }

        }

        private delegate void AddNodesDel(TreeNodeCollection collection, DataTable treeDataTable);
        private void AddNodes(TreeNodeCollection collection, DataTable treeDataTable)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new AddNodesDel(AddNodes);
                _mainTool.Invoke(s, new object[] { collection, treeDataTable });

            }
            else
            {
                DataRowCollection rows = treeDataTable.Rows;

                foreach (DataRow row in rows)
                {
                    //新建一个结点 =                 
                    TreeNode node = CreateTreeNode(row);

                    TreeNodeimageIndex(node, _isSelected);

                    AddTreeNode(collection, node); //加入到结点集合中              

                }
                return;
            }
        }

        private bool _isSelected = false;
        private delegate TreeNode CreateTreeNodeDel(DataRow row);
        private TreeNode CreateTreeNode(DataRow row)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new CreateTreeNodeDel(CreateTreeNode);
                return _mainTool.Invoke(s, new object[] { row }) as TreeNode;

            }
            else
            {
                if (row != null)
                {
                    var node = new TreeNode { Text = row["name"] as string };


                    string strTag = row["type"].ToString();
                    if (!string.IsNullOrEmpty(strTag))
                    {
                        var tag = (TagType)Enum.Parse(typeof(TagType), strTag, true);
                        node.Tag = new NodeTag(tag, row);
                    }
                    return node;
                }
            }
            return null;
        }

        private delegate void AddTreeNodeDel(TreeNodeCollection collection, TreeNode node);
        private void AddTreeNode(TreeNodeCollection collection, TreeNode node)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new AddTreeNodeDel(AddTreeNode);
                _mainTool.Invoke(s, new object[] { collection, node });
            }
            else
            {
                collection.Add(node);
            }

        }

        private delegate void TreeNodeimageIndexDel(TreeNode node, bool selected);
        private void TreeNodeimageIndex(TreeNode node, bool selected)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new TreeNodeimageIndexDel(TreeNodeimageIndex);
                _mainTool.Invoke(s, new object[] { node, selected });
            }
            else
            {
                var nodeTag = node.Tag as NodeTag;
                selected = false;
                if (nodeTag != null)
                {
                    switch (nodeTag.Tag)
                    {
                        case TagType.Db:
                            node.ImageIndex = 7;
                            node.SelectedImageIndex = node.ImageIndex;
                            break;
                        case TagType.Tables:
                            if (selected)
                            {
                                node.ImageIndex = 0;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            else
                            {
                                node.ImageIndex = 1;
                                node.SelectedImageIndex = node.ImageIndex;
                            }

                            break;
                        case TagType.Views:
                            if (selected)
                            {
                                node.ImageIndex = 3;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            else
                            {
                                node.ImageIndex = 4;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            break;
                        case TagType.Procedures:
                            if (selected)
                            {
                                node.ImageIndex = 5;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            else
                            {
                                node.ImageIndex = 6;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            break;
                    }
                }
            }
        }
        #endregion

        #region 绑定数据库配置信息
        IList<DbConfigInfo> _dbConfigList = new List<DbConfigInfo>();
        readonly Dictionary<string, DbConfigInfo> _dbConfigDic = new Dictionary<string, DbConfigInfo>();
        delegate void Simple();
        public void GetDbConfigList()
        {
            _dbConfigList.Clear();
            _tscbDbConfig.Items.Clear();
            _dbConfigDic.Clear();
            _dbConfigList = IniConfigHelper.ReadDBInfo();
            BindDbConfig();

        }
        private void BindDbConfig()
        {
            foreach (DbConfigInfo dc in _dbConfigList)
            {
                if (!_dbConfigDic.ContainsKey(dc.DbConfigName))
                {
                    _tscbDbConfig.Items.Add(dc.DbConfigName);
                    _dbConfigDic.Add(dc.DbConfigName, dc);
                }

            }
            if (_dbConfigList != null && _dbConfigList.Count > 0)
            {
                if (String.IsNullOrEmpty(_tscbDbConfig.Text))
                {
                    _tscbDbConfig.Text = _dbConfigList[0].DbConfigName;
                }
            }
        }
        #endregion

        #region 初始化
        private readonly BackgroundWorker _backgroundWorkerLoadDb = new BackgroundWorker();
       ContextMenuStrip contextMenuStrip=new ContextMenuStrip();
        protected override void load()
        {
         
            registerObject(PluginShareHelper.DBtable, Tables);
            registerObject(PluginShareHelper.DBtablesColumns, TablesColumns);
            registerObject(PluginShareHelper.DBviews, Views);
            registerObject(PluginShareHelper.DBtablesPrimaryKeys, TablesPrimaryKeys);
            
            _mainTool = Application.MainMenu;
            foreach (IDockContent content in Application.Panel.Contents)
            {
                _explorer = content as Explorer;
            }
            if (_explorer == null)
            {
                _explorer = new Explorer();
                _explorer.Show(Application.Panel, DockState.DockLeft);
            }
            _explorer.Text = "数据库信息";
            registerObject(PluginShareHelper.TapControl, _configForm.tcConfig);
            registerObject(PluginShareHelper.BtnSave, _configForm.btnSave);
            registerObject(PluginShareHelper.CmcSubPlugin, contextMenuStrip);
            AddTool();
            AddStatus();
            AddTreeControl();
            _backgroundWorkerLoadDb.DoWork += BackgroundWorkerLoadDbDoWork;
            _backgroundWorkerLoadDb.RunWorkerCompleted += BackgroundWorkerLoadDbRunWorkerCompleted;
            _backgroundWorkerLoadDb.ProgressChanged += BackgroundWorkerLoadDbProgressChanged;
            GetDbConfigList();
            isLoad = true;



        }
        private void Run(bool flag)
        {
            //if (!_backgroundWorkerLoadDb.IsBusy)
            {
                //_backgroundWorkerLoadDb.RunWorkerAsync(flag);
                ThreadPool.QueueUserWorkItem(o =>
                                                 {
                                                     DoWork(flag);
                                                     BackgroundWorkerLoadDbRunWorkerCompleted(null, null);
                                                 });
                ;
            }
        }

        void BackgroundWorkerLoadDbProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetProgress(e.ProgressPercentage);
        }

        void BackgroundWorkerLoadDbRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isLoadSuccess)
            {
                DbConfigInfo dbConfigInfo = GetCurenctDbConfigInfo();
                if (dbConfigInfo != null)
                {
                    if (_dsTable.Tables.Count == 0)
                    {
                        SetStatusBar(string.Format("{0}数据加载完毕，共{1}表", dbConfigInfo.DbConfigName, 0));
                    }
                    foreach (DataTable dt in _dsTable.Tables)
                    {
                        if (dt.TableName.Equals(dbConfigInfo.DbConfigName + Tables))
                        {
                            SetStatusBar(string.Format("{0}数据加载完毕，共{1}表", dbConfigInfo.DbConfigName,
                                                       _dsTable.Tables[dbConfigInfo.DbConfigName + Tables].Rows.Count));
                        }
                    }
                }

                registerObject(PluginShareHelper.DBCurrentDBAllTable, _dsTable);
                registerObject(PluginShareHelper.DBCurrentDBAllTablesColumns, _dsTableColumn);
                registerObject(PluginShareHelper.DBCurrentDBTablesPrimaryKeys, _dsTablePrimaryKey);
                SetEnable(true);
            }
            else
            {
                SetEnable(true);
                SetTbDbEnable(false);
            }


        }
        private void SetTbDbEnable(bool flag)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleBool(SetTbDbEnable);
                _mainTool.Invoke(s, new object[] { flag });
            }
            else
            {
                _tvDb.Enabled = flag;
                Application.MainContextMenu.Enabled = flag;
            }
        }

        void BackgroundWorkerLoadDbDoWork(object sender, DoWorkEventArgs e)
        {
            var flag = (bool)e.Argument;
            DoWork(flag);
        }
        private void DoWork(bool flag)
        {
            SetEnable(false);
            ClearTree();
            CreateRootNode(_tvDb.Nodes);
            SetProgreesEditValue(0);
            SetProgress(0);
            LoadDbInfoBySync(flag);
        }

        #endregion

        #region 加载当前选中数据库信息

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
        delegate void SimpleInt(int i);
        delegate void SimpleStr(string str);
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

        private delegate string Simple2();
        public string GetCurrentDBName()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple2(GetCurrentDBName);
                return _mainTool.Invoke(s, null) as string;
            }
            return _tscbDbConfig.Text;
        }

        private DbConfigInfo GetCurenctDbConfigInfo()
        {
            DbConfigInfo dbConfigInfo = null;
            string dbConfigName = GetCurrentDBName();
            if (!string.IsNullOrEmpty(dbConfigName) && _dbConfigDic.ContainsKey(dbConfigName))
            {
                dbConfigInfo = _dbConfigDic[dbConfigName];
            }
            return dbConfigInfo;
        }

        private bool _isLoadSuccess;
        private void LoadDbInfoBySync(bool reloadDb)
        {
            DbConfigInfo dbConfigInfo = GetCurenctDbConfigInfo();
            if (dbConfigInfo != null)
            {
                try
                {
                    #region 贡献当前数据库配置信息
                    if ("Oracle".Equals(dbConfigInfo.DbType))
                    {
                        registerObject(PluginShareHelper.TargetEncoding, "GBK");
                        registerObject(PluginShareHelper.OriginalEncoding, "ISO-8859-1");
                    }
                    else
                    {
                        registerObject(PluginShareHelper.TargetEncoding, "");
                        registerObject(PluginShareHelper.OriginalEncoding, "");
                    }
                    registerObject(PluginShareHelper.DBCurrentDBName, dbConfigInfo.DbConfigName);
                    registerObject(PluginShareHelper.DBCurrentDBConnectionString, dbConfigInfo.ConnectionString);
                    registerObject(PluginShareHelper.DBCurrentDBType, dbConfigInfo.DbType);
                    #endregion

                    #region 加载所有表信息
                    SetStatusBar(string.Format("正在获取{0}中所有表信息", dbConfigInfo.DbConfigName));
                    SetProgress(0);

                    if (_dsTable.Tables.Contains(dbConfigInfo.DbConfigName + Tables))
                    {
                        _dsTable.Tables.Remove(dbConfigInfo.DbConfigName + Tables);
                    }

                    #region 从本地读取数据
                    bool status = FilePathHelper.IsExist(dbConfigInfo.DbConfigName, Tables);
                    //MessageBox.Show(status + ":" + reloadDb);
                    if (!reloadDb)
                    {
                        if (status)
                        {
                            FilePathHelper.ReadXml(_dsTable, dbConfigInfo.DbConfigName, Tables);
                        }
                    }
                    #endregion

                    #region  从数据库中读取数据
                    if (!status || reloadDb)
                    {
                        DNCCFrameWork.DataAccess.IDbHelper db = new DNCCFrameWork.DataAccess.DbFactory(dbConfigInfo.ConnectionString.Trim(new[] { '"' }), DBType.GetDbProviderString(dbConfigInfo.DbType)).IDbHelper;
                        string sql = SqlDefHelper.GetTableNames(dbConfigInfo.DbType);                       
                        db.Fill(sql, _dsTable, new[] { dbConfigInfo.DbConfigName + Tables });
                        FilePathHelper.WriteXml(_dsTable);//缓存表数据到本地
                    }
                    #endregion

                    SetStatusBar(string.Format("{0}所有表信息加载完毕", dbConfigInfo.DbConfigName));
                    SetProgress(10);
                    #endregion

                    #region 加载所有表主键
                    SetStatusBar(string.Format("正在获取{0}中所有表主键信息", dbConfigInfo.DbConfigName));

                    if (_dsTablePrimaryKey.Tables.Contains(dbConfigInfo.DbConfigName + TablesPrimaryKeys))
                    {
                        _dsTablePrimaryKey.Tables.Remove(dbConfigInfo.DbConfigName + TablesPrimaryKeys);
                    }

                    #region 从本地读取数据
                    status = FilePathHelper.IsExist(dbConfigInfo.DbConfigName, TablesPrimaryKeys);
                    if (!reloadDb)
                    {
                        if (status)
                        {
                            FilePathHelper.ReadXml(_dsTablePrimaryKey, dbConfigInfo.DbConfigName, TablesPrimaryKeys);
                        }
                    }
                    #endregion

                    #region  从数据库中读取数据
                    if (!status || reloadDb)
                    {
                        DNCCFrameWork.DataAccess.IDbHelper db = new DNCCFrameWork.DataAccess.DbFactory(dbConfigInfo.ConnectionString.Trim(new[] { '"' }), DBType.GetDbProviderString(dbConfigInfo.DbType)).IDbHelper;
                        string sql = SqlDefHelper.GetAllTablePrimaryKeys(dbConfigInfo.DbType);
                        db.Fill(sql, _dsTablePrimaryKey, new[] { dbConfigInfo.DbConfigName + TablesPrimaryKeys });
                        FilePathHelper.WriteXml(_dsTablePrimaryKey);//缓存表数据到本地
                    }
                    #endregion

                    SetStatusBar(string.Format("{0}所有表主键信息加载完毕", dbConfigInfo.DbConfigName));
                    SetProgress(10);
                    #endregion

                    #region 构造树形结构
                    BindTreeBySync();
                    #endregion

                    #region 加载所有表字段信息

                    SetStatusBar(string.Format("正在获取{0}中所有表字段信息", dbConfigInfo.DbConfigName));

                    if (_dsTableColumn.Tables.Contains(dbConfigInfo.DbConfigName + TablesColumns))
                    {
                        _dsTableColumn.Tables.Remove(dbConfigInfo.DbConfigName + TablesColumns);
                    }

                    #region 从本地读取表字段信息
                    status = FilePathHelper.IsExist(dbConfigInfo.DbConfigName, TablesColumns);
                    if (!reloadDb)
                    {
                        if (status)
                        {
                            FilePathHelper.ReadXml(_dsTableColumn, dbConfigInfo.DbConfigName, TablesColumns);
                            SetProgress(80);
                        }
                    }
                    #endregion

                    #region 从数据库读取表字段信息
                    if (!status || reloadDb)
                    {
                        DNCCFrameWork.DataAccess.IDbHelper db = new DNCCFrameWork.DataAccess.DbFactory(dbConfigInfo.ConnectionString.Trim(new[] { '"' }), DBType.GetDbProviderString(dbConfigInfo.DbType)).IDbHelper;
                        int count = _dsTable.Tables[dbConfigInfo.DbConfigName + Tables].Rows.Count;
                        int temp1 = count / 7;
                        bool isDivisible = count % temp1 == 0;
                        string sql = SqlDefHelper.GetTableColumnNames(dbConfigInfo.DbType);
                        for (int i = 0; i < count; i++)
                        {
                            DataRow dr = _dsTable.Tables[dbConfigInfo.DbConfigName + Tables].Rows[i];
                            var temp = new DataSet();
                            var dicPar = new Dictionary<string, string> { { "@tableName", dr["name"] as string } };
                            SetStatusBar(string.Format("正在获取{0}中{1}字段信息", dbConfigInfo.DbConfigName, dr["name"] as string));
                            db.Fill(sql, temp, new[] { dbConfigInfo.DbConfigName + TablesColumns }, dicPar);
                            _dsTableColumn.Merge(temp);
                            if (i % temp1 == 0)
                            {
                                SetProgress(10);
                            }
                        }
                        FilePathHelper.WriteXml(_dsTableColumn);//缓存表字段数据到本地，
                        if (!isDivisible)
                        {
                            SetProgress(10);
                        }
                    }
                    #endregion

                    SetEnable(true);

                    #endregion

                    _isLoadSuccess = true;

                }
                catch (System.Data.Common.DbException ex)
                {
                    SetStatusBar(string.Format("加载数据失败[{0}]",ex.Message));

                }
            }
            else
            {
                SetStatusBar("加载数据失败[没有获取到数据库配置]");
            }
        }

        #endregion

        #region 获取选择项

        private delegate DataRow[] GetCheckTableDel();
        private DataRow[] GetCheckTable()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new GetCheckTableDel(GetCheckTable);
                return _mainTool.Invoke(s, null) as DataRow[];
            }
            var tnCheckList = new List<TreeNode>();
            GetTnCheck(_tvDb.Nodes, tnCheckList);
            var drTable = new DataRow[0];
            if (tnCheckList.Count > 0)
            {
                drTable = new DataRow[tnCheckList.Count];
                for (int i = 0; i < drTable.Length; i++)
                {
                    drTable[i] = ((NodeTag)tnCheckList[i].Tag).Dr;
                }
            }
            return drTable;
        }

        private delegate void GetTnCheckDel(TreeNodeCollection collection, IList<TreeNode> tnCheckList);

        private void GetTnCheck(TreeNodeCollection collection, IList<TreeNode> tnCheckList)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new GetTnCheckDel(GetTnCheck);
                _mainTool.Invoke(s, null);
            }
            else
            {
                foreach (TreeNode tn in collection)
                {
                    if (tn.Checked && tn.Tag is NodeTag)
                    {
                        tnCheckList.Add(tn);
                    }
                    if (tn.Nodes.Count > 0)
                    {
                        GetTnCheck(tn.Nodes, tnCheckList);
                    }
                }
            }
        }

        #endregion
    }

    #region
    internal class NodeTag
    {
        public NodeTag()
        { }
        public NodeTag(TagType tag, DataRow dr)
        {

            _tag = tag;
            _dr = dr;
        }


        readonly TagType _tag = TagType.None;

        public TagType Tag
        {
            get { return _tag; }
        }

        readonly DataRow _dr;

        public DataRow Dr
        {
            get { return _dr; }
        }
    }
    internal enum TagType
    {
        Db,
        Tables,
        Table,
        Views,
        View,
        Procedures,
        Procedure,
        None

    }
    #endregion
}
