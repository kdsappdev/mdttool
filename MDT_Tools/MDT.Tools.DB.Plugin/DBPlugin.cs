using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.UI;
using MDT.Tools.DB.Plugin.Model;
using MDT.Tools.DB.Plugin.UI;
using MDT.Tools.DB.Plugin.Utils;

namespace MDT.Tools.DB.Plugin
{
    public class DBPlugin : AbstractPlugin
    {
        #region 插件信息
        private int tag = 1;
        public override int Tag
        {
            get { return tag; }
            set { tag = value; }
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
        DataSet dsTable = new DataSet();//所有数据库表信息
        DataSet dsTableColumn = new DataSet();//所有表对应的列信息
        DataSet dsTablePrimaryKey = new DataSet();//所有表对应的主键信息

        public readonly string tables = "_TABLES";
        public readonly string tablesColumns = "_TABLES_COLUMNS";
        public readonly string views = "_VIEWS";
        public readonly string tablesPrimaryKeys = "_TABLES_PK";

        #endregion

        private TreeView tvDB = new TreeView();
        private Explorer Explorer;
        public override void OnLoading()
        {
            base.OnLoading();
            load();
        }
        public override void BeforeTerminating()
        {
            unLoad();
        }
        private void unLoad()
        {
            if (isLoad)
            {
                dsTable.Clear();
                dsTableColumn.Clear();
                dsTablePrimaryKey.Clear();
                clearTree();
                removeShareData();
                Explorer.Text = "Explorer";
                removeTool();
                removeStatus();
                removeTreeControl();
                backgroundWorkerLoadDb.DoWork -= new DoWorkEventHandler(backgroundWorkerLoadDb_DoWork);
                backgroundWorkerLoadDb.RunWorkerCompleted -=
                    backgroundWorkerLoadDb_RunWorkerCompleted;
                backgroundWorkerLoadDb.ProgressChanged -=
                    backgroundWorkerLoadDb_ProgressChanged;
            }
        }
        private void removeShareData()
        {
            Application.Remove(DBPluginShareHelper.DBCurrentDBConfig);
            Application.Remove(DBPluginShareHelper.DBCurrentCheckTable);
            Application.Remove(DBPluginShareHelper.DBCurrentDBAllTable);
            Application.Remove(DBPluginShareHelper.DBCurrentDBAllTablesColumns);
            Application.Remove(DBPluginShareHelper.DBCurrentDBTablesPrimaryKeys);
        }

        #region 工具栏 增加数据参数配置
        ToolStripButton tsbDBSet = new ToolStripButton();
        ToolStripComboBox tscbDBConfig = new ToolStripComboBox();
        ToolStripButton tsbDBReSet = new ToolStripButton();
        ToolStripSeparator toolStripSeparator = new ToolStripSeparator();

        private void addTool()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(addTool);
                Explorer.Invoke(s, null);
            }
            else
            {
                tsbDBSet.Text = "数据库配置";
                tsbDBSet.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                tsbDBSet.Click += new EventHandler(tsbDBSet_Click);


                tscbDBConfig.DropDownStyle = ComboBoxStyle.DropDownList;
                tscbDBConfig.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                tscbDBConfig.SelectedIndexChanged += new EventHandler(tscbDBConfig_SelectedIndexChanged);

                tsbDBReSet.Text = "重新加载数据库";
                tsbDBReSet.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                tsbDBReSet.Click += new EventHandler(tsbDBReSet_Click);


                Application.MainTool.Items.Insert(0, tsbDBSet);
                Application.MainTool.Items.Insert(1, tscbDBConfig);
                Application.MainTool.Items.Insert(2, tsbDBReSet);
                Application.MainTool.Items.Insert(3, toolStripSeparator);
            }
        }
        private void removeTool()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(removeTool);
                Explorer.Invoke(s, null);
            }
            else
            {
                Application.MainTool.Items.Remove(tsbDBSet);
                Application.MainTool.Items.Remove(tscbDBConfig);
                Application.MainTool.Items.Remove(tsbDBReSet);
                Application.MainTool.Items.Remove(toolStripSeparator);
            }
        }

        void tsbDBReSet_Click(object sender, EventArgs e)
        {
            run(true);
        }


        void tscbDBConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            run(false);
        }


        void tsbDBSet_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }

        #endregion

        #region 增加状态栏
        ToolStripStatusLabel tsslMessage = new ToolStripStatusLabel();
        ToolStripProgressBar tspbLoadDBProgress = new ToolStripProgressBar();
        private void addStatus()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(addStatus);
                Explorer.Invoke(s, null);
            }
            else
            {
                tspbLoadDBProgress.AutoSize = false;
                tspbLoadDBProgress.Visible = false;
                tspbLoadDBProgress.Width = 250;

                tsslMessage.AutoSize = false;
                tsslMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                tsslMessage.Width = Application.StatusBar.Width - tspbLoadDBProgress.Width - 20;
                Application.StatusBar.SizeChanged += new EventHandler(StatusBar_SizeChanged);
                Application.StatusBar.Items.Insert(0, tsslMessage);
                Application.StatusBar.Items.Insert(1, tspbLoadDBProgress);
            }
        }

        private void removeStatus()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(removeStatus);
                Explorer.Invoke(s, null);

            }
            else
            {
                Application.StatusBar.Items.Remove(tsslMessage);
                Application.StatusBar.Items.Remove(tspbLoadDBProgress);
            }

        }

        void StatusBar_SizeChanged(object sender, EventArgs e)
        {
            tsslMessage.Width = Application.StatusBar.Width - tspbLoadDBProgress.Width - 20;

        }

        #endregion

        #region 增加树形控件
        private void addTreeControl()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(addTreeControl);
                Explorer.Invoke(s, null);
            }
            else
            {
                tvDB.Dock = DockStyle.Fill;
                tvDB.CheckBoxes = true;
                tvDB.AfterCheck += new TreeViewEventHandler(tvDB_AfterCheck);
                tvDB.MouseClick += new MouseEventHandler(tvDB_MouseClick);
                Application.Explorer.Controls.Add(tvDB);
            }
        }


        void tvDB_AfterCheck(object sender, TreeViewEventArgs e)
        {
            DataRow[] dr = getCheckTable();
            Application.RegisterObject(DBPluginShareHelper.DBCurrentCheckTable,dr);
            bool flag = false;
            if(dr!=null&&dr.Length>0)
            {
                flag = true;
            }
            broadcast(DBPluginShareHelper.BroadCast_CheckTableNumberIsGreaterThan0, flag);
        }

        void tvDB_MouseClick(object sender, MouseEventArgs e)
        {
            DataRow[] dr= getCheckTable();
            if (e.Button == MouseButtons.Right)
            {
                
                if (dr != null && dr.Length > 0)
                {
                    Application.MainContextMenu.Show(tvDB, e.Location);
                }
            }
        }
        private void removeTreeControl()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(removeTreeControl);
                Explorer.Invoke(s, null);
            }
            else
            {
                Application.Explorer.Controls.Remove(tvDB);
            }

        }
        #endregion

        #region 清除树形结构

        private void clearTree()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(clearTree);
                Explorer.Invoke(s, null);
            }
            else
            {
                tvDB.Nodes.Clear();
            }
        }
        #endregion

        private delegate void SimpleBool(bool flag);
        private void setEnable(bool flag)
        {
            if (Explorer.InvokeRequired)
            {
                SimpleBool s = new SimpleBool(setEnable);
                Explorer.Invoke(s, new object[] { flag });
            }
            else
            {
                tsbDBSet.Enabled = flag;
                tsbDBReSet.Enabled = flag;
                tscbDBConfig.Enabled = flag;
                tspbLoadDBProgress.Visible = !flag;
            }

        }

        #region 增加节点
        private void bindTreeBySync()
        {
            ThreadPool.QueueUserWorkItem(delegate(object o)
           {
               addNodes(tvDB.Nodes);
               exandAllTreeNode();
           });
        }
        private void exandAllTreeNode()
        {
            if (Explorer.InvokeRequired)
            {
                Simple s = new Simple(exandAllTreeNode);
                Explorer.Invoke(s, null);

            }
            else
            {
                tvDB.ExpandAll();
            }

        }
        private void addNodes(TreeNodeCollection collection)
        {
            if (Explorer.InvokeRequired)
            {
                createRootNodeDel s = new createRootNodeDel(addNodes);
                Explorer.Invoke(s, new object[] { collection });

            }
            else
            {
                DbConfigInfo dbConfigInfo = getCurenctDbConfigInfo();
                if (dbConfigInfo != null)
                {

                    for (int i = 0; i < dsTable.Tables.Count; i++)
                    {
                        if (dsTable.Tables[i].TableName.Equals(dbConfigInfo.DbConfigName + tables))
                        {
                            addNodes(collection[0].Nodes, dsTable.Tables[i]);
                        }
                        if (dsTable.Tables[i].TableName.Equals(dbConfigInfo.DbConfigName + views))
                        {
                            addNodes(collection[1].Nodes, dsTable.Tables[i]);
                        }
                    }
                }
            }
        }

        private delegate void createRootNodeDel(TreeNodeCollection collection);
        private void createRootNode(TreeNodeCollection collection)
        {
            if (Explorer.InvokeRequired)
            {
                createRootNodeDel s = new createRootNodeDel(createRootNode);
                Explorer.Invoke(s, new object[] { collection });

            }
            else
            {
                TreeNode tablesNode = new TreeNode();
                tablesNode.Text = TagType.Tables.ToString();
                tablesNode.Tag = TagType.Tables;
                addTreeNode(collection, tablesNode);

                TreeNode viewsNode = new TreeNode();
                viewsNode.Text = TagType.Views.ToString();
                viewsNode.Tag = TagType.Views;
                addTreeNode(collection, viewsNode);
            }

        }

        private delegate void addNodesDel(TreeNodeCollection collection, DataTable treeDataTable);
        private void addNodes(TreeNodeCollection collection, DataTable treeDataTable)
        {
            if (Explorer.InvokeRequired)
            {
                addNodesDel s = new addNodesDel(addNodes);
                Explorer.Invoke(s, new object[] { collection, treeDataTable });

            }
            else
            {
                DataRowCollection rows = treeDataTable.Rows;

                foreach (DataRow row in rows)
                {
                    //新建一个结点 =                 
                    TreeNode node = createTreeNode(row);

                    treeNodeimageIndex(node, false);

                    addTreeNode(collection, node); //加入到结点集合中              

                }
                return;
            }
        }

        private delegate TreeNode createTreeNodeDel(DataRow row);
        private TreeNode createTreeNode(DataRow row)
        {
            if (Explorer.InvokeRequired)
            {
                createTreeNodeDel s = new createTreeNodeDel(createTreeNode);
                return Explorer.Invoke(s, new object[] { row }) as TreeNode;

            }
            else
            {
                TreeNode node = new TreeNode();

                node.Text = row["name"] as string;
                string strTag = row["type"].ToString();
                if (!string.IsNullOrEmpty(strTag))
                {
                    TagType tag = (TagType)Enum.Parse(typeof(TagType), strTag, true);
                    node.Tag = new NodeTag(tag, row);
                }
                return node;
            }
        }
        private delegate void addTreeNodeDel(TreeNodeCollection collection, TreeNode node);
        private void addTreeNode(TreeNodeCollection collection, TreeNode node)
        {
            if (Explorer.InvokeRequired)
            {
                addTreeNodeDel s = new addTreeNodeDel(addTreeNode);
                Explorer.Invoke(s, new object[] { collection, node });
            }
            else
            {
                collection.Add(node);
            }

        }

        private delegate void treeNodeimageIndexDel(TreeNode node, bool selected);
        private void treeNodeimageIndex(TreeNode node, bool selected)
        {
            if (Explorer.InvokeRequired)
            {
                treeNodeimageIndexDel s = new treeNodeimageIndexDel(treeNodeimageIndex);
                Explorer.Invoke(s, new object[] { node, selected });
            }
            else
            {
                NodeTag nodeTag = node.Tag as NodeTag;
                selected = false;
                if (nodeTag != null)
                {
                    switch (nodeTag.Tag)
                    {
                        case TagType.DB:
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
        IList<DbConfigInfo> dbConfigList = new List<DbConfigInfo>();
        Dictionary<string, DbConfigInfo> dbConfigDic = new Dictionary<string, DbConfigInfo>();
        delegate void Simple();
        public void getDbConfigList()
        {
            dbConfigList.Clear();
            tscbDBConfig.Items.Clear();

            dbConfigList = INIConfigHelper.ReadDBInfo();
            bindDbConfig();

        }
        private void bindDbConfig()
        {
            foreach (DbConfigInfo dc in dbConfigList)
            {
                if (!dbConfigDic.ContainsKey(dc.DbConfigName))
                {
                    tscbDBConfig.Items.Add(dc.DbConfigName);
                    dbConfigDic.Add(dc.DbConfigName, dc);
                }

            }
            if (dbConfigList != null && dbConfigList.Count > 0)
            {
                if (String.IsNullOrEmpty(tscbDBConfig.Text))
                {
                    tscbDBConfig.Text = dbConfigList[0].DbConfigName;
                }
            }
        }
        #endregion

        #region 初始化

        private bool isLoad = false;
        private object isLoadLock = new object();
        private BackgroundWorker backgroundWorkerLoadDb = new BackgroundWorker();
        private void load()
        {
            if (!isLoad)
            {
                lock (isLoadLock)
                {
                    if (!isLoad)
                    {
                        Explorer = Application.Explorer;
                        Explorer.Text = "数据库信息";
                        addTool();
                        addStatus();
                        addTreeControl();
                        backgroundWorkerLoadDb.DoWork += new DoWorkEventHandler(backgroundWorkerLoadDb_DoWork);
                        backgroundWorkerLoadDb.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerLoadDb_RunWorkerCompleted);
                        backgroundWorkerLoadDb.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerLoadDb_ProgressChanged);
                        getDbConfigList();
                        isLoad = true;
                    }
                }
            }
        }
        private void run(bool flag)
        {
            if (!backgroundWorkerLoadDb.IsBusy)
            {
                backgroundWorkerLoadDb.RunWorkerAsync(flag);
            }
        }

        void backgroundWorkerLoadDb_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            setProgress(e.ProgressPercentage);
        }

        void backgroundWorkerLoadDb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DbConfigInfo dbConfigInfo = getCurenctDbConfigInfo();
            if (dbConfigInfo != null)
            {
                if (dsTable.Tables.Count == 0)
                {
                    setStatusBar(string.Format("{0}数据加载完毕，共{1}表", dbConfigInfo.DbConfigName, 0));
                }
                foreach (DataTable dt in dsTable.Tables)
                {
                    if (dt.TableName.Equals(dbConfigInfo.DbConfigName + tables))
                    {
                        setStatusBar(string.Format("{0}数据加载完毕，共{1}表", dbConfigInfo.DbConfigName, dsTable.Tables[dbConfigInfo.DbConfigName + tables].Rows.Count));
                    }
                }

                setEnable(true);
            }
            Application.RegisterObject(DBPluginShareHelper.DBCurrentDBConfig, dbConfigInfo);
            Application.RegisterObject(DBPluginShareHelper.DBCurrentDBAllTable,dsTable);
            Application.RegisterObject(DBPluginShareHelper.DBCurrentDBAllTablesColumns, dsTableColumn);
            Application.RegisterObject(DBPluginShareHelper.DBCurrentDBTablesPrimaryKeys, dsTablePrimaryKey);
        }

        void backgroundWorkerLoadDb_DoWork(object sender, DoWorkEventArgs e)
        {
            bool flag = (bool)e.Argument;
            doWork(flag);
        }
        private void doWork(bool flag)
        {
            setEnable(false);
            clearTree();
            createRootNode(tvDB.Nodes);
            removeShareData();
            setProgreesEditValue(0);
            setProgress(0);
            loadDbInfoBySync(flag);
        }

        #endregion

        #region 加载当前选中数据库信息

        public void setProgreesEditValue(int i)
        {
            if (Explorer.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgreesEditValue);
                Explorer.Invoke(s, new object[] { i });
            }
            else
            {
                tspbLoadDBProgress.Value = i;
                ;
            }

        }
        delegate void SimpleInt(int i);
        delegate void SimpleStr(string str);
        public void setProgressMax(int i)
        {
            if (Explorer.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgressMax);
                Explorer.Invoke(s, new object[] { i });

            }
            else
            {
                tspbLoadDBProgress.Maximum = i;
            }

        }
        public void setProgress(int i)
        {
            if (Explorer.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgress);
                Explorer.Invoke(s, new object[] { i });

            }
            else
            {
                if (i + (int)tspbLoadDBProgress.Value > tspbLoadDBProgress.Maximum)
                {
                    tspbLoadDBProgress.Value = tspbLoadDBProgress.Maximum;
                }
                else
                {
                    tspbLoadDBProgress.Value += i;
                };
            }

        }
        public void setStatusBar(string str)
        {
            if (Explorer.InvokeRequired)
            {
                SimpleStr s = new SimpleStr(setStatusBar);
                Explorer.Invoke(s, new object[] { str });

            }
            else
            {
                tsslMessage.Text = str;
            }
        }

        private delegate string Simple2();
        public string getCurrentDBName()
        {
            if (Explorer.InvokeRequired)
            {
                Simple2 s = new Simple2(getCurrentDBName);
                return Explorer.Invoke(s, null) as string;
            }
            else
            {
                return tscbDBConfig.Text;
            }

        }

        private DbConfigInfo getCurenctDbConfigInfo()
        {
            DbConfigInfo dbConfigInfo = null;
            string dbConfigName = getCurrentDBName();
            if (!string.IsNullOrEmpty(dbConfigName) && dbConfigDic.ContainsKey(dbConfigName))
            {
                dbConfigInfo = dbConfigDic[dbConfigName];
            }
            return dbConfigInfo;
        }
        private void loadDbInfoBySync(bool reloadDb)
        {
            DbConfigInfo dbConfigInfo = getCurenctDbConfigInfo();
            if (dbConfigInfo != null)
            {
                try
                {

                    #region 加载所有表信息
                    setStatusBar(string.Format("正在获取{0}中所有表信息", dbConfigInfo.DbConfigName));
                    setProgress(0);

                    if (dsTable.Tables.Contains(dbConfigInfo.DbConfigName + tables))
                    {
                        dsTable.Tables.Remove(dbConfigInfo.DbConfigName + tables);
                    }

                    #region 从本地读取数据
                    bool status = FilePathHelper.isExist(dbConfigInfo.DbConfigName, tables);
                    //MessageBox.Show(status + ":" + reloadDb);
                    if (!reloadDb)
                    {
                        if (status)
                        {
                            FilePathHelper.ReadXml(dsTable, dbConfigInfo.DbConfigName, tables);
                        }
                    }
                    #endregion

                    #region  从数据库中读取数据
                    if (!status || reloadDb)
                    {
                        DNCCFrameWork.DataAccess.IDbHelper db = new DNCCFrameWork.DataAccess.DbFactory(dbConfigInfo.ConnectionString.Trim(new char[] { '"' }), DBType.GetDbProviderString(dbConfigInfo.DbType)).IDbHelper;
                        string sql = SqlDefHelper.GetTableNames(dbConfigInfo.DbType);
                        //MessageBox.Show(sql);
                        db.Fill(sql, dsTable, new string[] { dbConfigInfo.DbConfigName + tables });
                        FilePathHelper.WriteXml(dsTable);//缓存表数据到本地
                    }
                    #endregion

                    setStatusBar(string.Format("{0}所有表信息加载完毕", dbConfigInfo.DbConfigName));
                    setProgress(10);
                    #endregion

                    #region 加载所有表主键
                    setStatusBar(string.Format("正在获取{0}中所有表主键信息", dbConfigInfo.DbConfigName));

                    if (dsTablePrimaryKey.Tables.Contains(dbConfigInfo.DbConfigName + tablesPrimaryKeys))
                    {
                        dsTablePrimaryKey.Tables.Remove(dbConfigInfo.DbConfigName + tablesPrimaryKeys);
                    }

                    #region 从本地读取数据
                    status = FilePathHelper.isExist(dbConfigInfo.DbConfigName, tablesPrimaryKeys);
                    if (!reloadDb)
                    {
                        if (status)
                        {
                            FilePathHelper.ReadXml(dsTablePrimaryKey, dbConfigInfo.DbConfigName, tablesPrimaryKeys);
                        }
                    }
                    #endregion

                    #region  从数据库中读取数据
                    if (!status || reloadDb)
                    {
                        DNCCFrameWork.DataAccess.IDbHelper db = new DNCCFrameWork.DataAccess.DbFactory(dbConfigInfo.ConnectionString.Trim(new char[] { '"' }), DBType.GetDbProviderString(dbConfigInfo.DbType)).IDbHelper;
                        string sql = Utils.SqlDefHelper.GetAllTablePrimaryKeys(dbConfigInfo.DbType);
                        db.Fill(sql, dsTablePrimaryKey, new string[] { dbConfigInfo.DbConfigName + tablesPrimaryKeys });
                        FilePathHelper.WriteXml(dsTablePrimaryKey);//缓存表数据到本地
                    }
                    #endregion

                    setStatusBar(string.Format("{0}所有表主键信息加载完毕", dbConfigInfo.DbConfigName));
                    setProgress(10);
                    #endregion

                    #region 构造树形结构
                    bindTreeBySync();
                    #endregion

                    #region 加载所有表字段信息

                    setStatusBar(string.Format("正在获取{0}中所有表字段信息", dbConfigInfo.DbConfigName));

                    if (dsTableColumn.Tables.Contains(dbConfigInfo.DbConfigName + tablesColumns))
                    {
                        dsTableColumn.Tables.Remove(dbConfigInfo.DbConfigName + tablesColumns);
                    }

                    #region 从本地读取表字段信息
                    status = FilePathHelper.isExist(dbConfigInfo.DbConfigName, tablesColumns);
                    if (!reloadDb)
                    {
                        if (status)
                        {
                            FilePathHelper.ReadXml(dsTableColumn, dbConfigInfo.DbConfigName, tablesColumns);
                            setProgress(80);
                        }
                    }
                    #endregion

                    #region 从数据库读取表字段信息
                    if (!status || reloadDb)
                    {
                        DNCCFrameWork.DataAccess.IDbHelper db = new DNCCFrameWork.DataAccess.DbFactory(dbConfigInfo.ConnectionString.Trim(new char[] { '"' }), DBType.GetDbProviderString(dbConfigInfo.DbType)).IDbHelper;
                        int count = dsTable.Tables[dbConfigInfo.DbConfigName + tables].Rows.Count;
                        int temp1 = count / 7;
                        bool isDivisible = count % temp1 == 0 ? true : false;
                        string sql = Utils.SqlDefHelper.GetTableColumnNames(dbConfigInfo.DbType);
                        for (int i = 0; i < count; i++)
                        {
                            DataRow dr = dsTable.Tables[dbConfigInfo.DbConfigName + tables].Rows[i];
                            DataSet temp = new DataSet();
                            Dictionary<string, string> dicPar = new Dictionary<string, string>();
                            dicPar.Add("@tableName", dr["name"] as string);
                            setStatusBar(string.Format("正在获取{0}中{1}字段信息", dbConfigInfo.DbConfigName, dr["name"] as string));
                            db.Fill(sql, temp, new string[] { dbConfigInfo.DbConfigName + tablesColumns }, dicPar);
                            dsTableColumn.Merge(temp);
                            if (i % temp1 == 0)
                            {
                                setProgress(10);
                            }
                        }
                        FilePathHelper.WriteXml(dsTableColumn);//缓存表字段数据到本地，
                        if (!isDivisible)
                        {
                            setProgress(10);
                        }
                    }
                    #endregion

                    setEnable(true);

                    #endregion

                }
                catch (System.Data.Common.DbException ex)
                {
                    setStatusBar("加载数据失败[" + ex.Message + "]");
                }
            }
        }

        #endregion


        #region 获取选择项

        private delegate DataRow[] getCheckTableDel();
        private DataRow[] getCheckTable()
        {
            if(Explorer.InvokeRequired)
            {
                getCheckTableDel s = new getCheckTableDel(getCheckTable);
                return Explorer.Invoke(s, null) as DataRow[];
            }
            else
            {
                IList<TreeNode> tnCheckList = new List<TreeNode>();
                getTnCheck(tvDB.Nodes, tnCheckList);
                DataRow[] drTable = new DataRow[0];
                if (tnCheckList.Count > 0)
                {
                    drTable = new DataRow[tnCheckList.Count];
                    for (int i = 0; i < drTable.Length; i++)
                    {
                        drTable[i] = (tnCheckList[i].Tag as NodeTag).Dr;
                    }
                }
                return drTable;
            }
           
        }

        private delegate void getTnCheckDel(TreeNodeCollection collection, IList<TreeNode> tnCheckList);
        
        private void getTnCheck(TreeNodeCollection collection, IList<TreeNode> tnCheckList)
        {
            if (Explorer.InvokeRequired)
            {
                getTnCheckDel s = new getTnCheckDel(getTnCheck);
                 Explorer.Invoke(s, null);
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
                        getTnCheck(tn.Nodes, tnCheckList);
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

            this.tag = tag;
            this.dr = dr;
        }


        TagType tag = TagType.None;

        public TagType Tag
        {
            get { return tag; }
        }
        DataRow dr = null;

        public DataRow Dr
        {
            get { return dr; }
        }
    }
    internal enum TagType
    {
        DB,
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
