using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Resources;
using MDT.Tools.DB.Plugin.Model;
using MDT.Tools.DB.Plugin.Utils;

namespace MDT.Tools.DB.Plugin.UI
{
    internal partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            Icon = Resources.setting;
        }

        public delegate void dbConfigInfoChanged();

        public event dbConfigInfoChanged DBConfigInfoChanged;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isChanged)
            {
                IniConfigHelper.DeleteFile();
                foreach (var dbConfigInfo in dbConfigInfos)
                {
                    string message = "";
                    IniConfigHelper.WriteDBInfo(dbConfigInfo, ref message);
                }
                if (DBConfigInfoChanged != null)
                {
                    DBConfigInfoChanged();
                }
            }
            Close();
        }

        private IList<DbConfigInfo> dbConfigInfos = null;
        private void ConfigForm_Load(object sender, EventArgs e)
        {
            dbConfigInfos = IniConfigHelper.ReadDBInfo();
            createTreeNode();

        }
        private void createTreeNode()
        {
            tvDBConfig.Nodes.Clear();
            TreeNode tnOracle = new TreeNode("Oracle");
            tvDBConfig.Nodes.Add(tnOracle);
            if (dbConfigInfos != null)
            {
                foreach (var dbConfigInfo in dbConfigInfos)
                {
                    TreeNode tn = new TreeNode(dbConfigInfo.DbConfigName);
                    tn.Tag = dbConfigInfo;
                    tn.SelectedImageIndex = tn.ImageIndex = 2;
                    if (dbConfigInfo.DbType == tnOracle.Text)
                    {
                        tnOracle.Nodes.Add(tn);
                    }
                }
            }
            tvDBConfig.ExpandAll();
            tvDBConfig.Refresh();

        }

        private bool isChanged = false;
        private void tvDBConfig_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var text = tvDBConfig.SelectedNode.Text as string;
             
            switch (text)
            {
                case "Oracle":
                    OracleDBConfigForm of = new OracleDBConfigForm();
                    DialogResult dr = of.ShowDialog(this);
                    if (dr.Equals(DialogResult.OK))
                    {
                        dbConfigInfos.Insert(0,of.DbConfigInfo);
                        
                        createTreeNode();
                        isChanged = true;
                    }
                    break;
                default:
                    DbConfigInfo dbConfigInfo = tvDBConfig.SelectedNode.Tag as DbConfigInfo;
                    if (dbConfigInfo != null)
                    {
                        if (dbConfigInfo.DbType == "Oracle")
                        {
                            OracleDBConfigForm of2 = new OracleDBConfigForm(dbConfigInfo);
                            DialogResult dr2 = of2.ShowDialog(this);
                            if (dr2.Equals(DialogResult.OK))
                            {
                                dbConfigInfos.Remove(dbConfigInfo);
                                
                                createTreeNode();
                                isChanged = true;
                            }
                        }
                    }
                    break;
            }
        }

        private void tvDBConfig_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            //e.Cancel = true;
        }
    }
}
