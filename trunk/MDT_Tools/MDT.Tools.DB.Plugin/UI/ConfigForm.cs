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
            bool status = true;
            string message = "";
            if (isChanged)
            {
                foreach (var dbConfigInfo in dbConfigInfos)
                {
                   
                    status = IniConfigHelper.WriteDBInfo(dbConfigInfo, ref message);
                    if (!status)
                    {
                        break;
                    }

                }
                if (status)
                {
                    if (DBConfigInfoChanged != null)
                    {
                        DBConfigInfoChanged();
                    }
                }
            }
            if (status)
            {
                Close();
            }
            else
            {
                MessageBox.Show(this,"保存失败[" + message+"]", @"提示");
            }
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
                    if (!dbConfigInfo.IsDelete)
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
                   of.ShowDialog(this);
                   if (of.DialogResult.Equals(DialogResult.OK))
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
                             of2.ShowDialog(this);
                            if (of2.DialogResult.Equals(DialogResult.OK))
                            {
                                dbConfigInfo.IsDelete = true;
                                
                                createTreeNode();
                                isChanged = true;
                            }
                            else if (of2.DialogResult.Equals(DialogResult.Yes))
                            {
                                dbConfigInfos.Remove(dbConfigInfo);
                                dbConfigInfos.Insert(0,of2.DbConfigInfo);
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
