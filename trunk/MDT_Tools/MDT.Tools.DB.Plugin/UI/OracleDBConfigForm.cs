using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.DB.Plugin.Model;
using MDT.Tools.DB.Plugin.Utils;

namespace MDT.Tools.DB.Plugin.UI
{
    internal partial class OracleDBConfigForm : Form
    {
        public OracleDBConfigForm()
        {
            InitializeComponent();
        }
        public OracleDBConfigForm(DbConfigInfo dbConfigInfo)
            : this()
        {
            btnAdd.Text = "删除";
            btnUpdate.Visible = true;
            setDbConfigInfo(dbConfigInfo);

        }
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkData())
                {
                    ThreadPool.QueueUserWorkItem(o =>
                                                     {
                                                         DbConfigInfo dbConfigInfo = getDbConfigInfo();
                                                         string conString =
                                                             dbConfigInfo.ConnectionString.Trim(new char[] {'"'});
                                                         string dbProvider =
                                                             DBType.GetDbProviderString(dbConfigInfo.DbType);
                                                         DNCCFrameWork.DataAccess.IDbHelper db =
                                                             new DNCCFrameWork.DataAccess.DbFactory(conString,
                                                                                                    dbProvider).
                                                                 IDbHelper;
                                                         bool success = db.TestConnection();
                                                         string tip = "数据库连接失败";
                                                         if (success)
                                                         {
                                                             tip = "数据库连接成功";
                                                         }
                                                         MessageBox.Show(this, tip, "提示", MessageBoxButtons.OK,
                                                                         MessageBoxIcon.Information,
                                                                         MessageBoxDefaultButton.Button1);
                                                     });
                }
            }
            catch (System.Data.Common.DbException ex)
            {
                MessageBox.Show(this, ex.Message, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void setDbConfigInfo(DbConfigInfo dbConfigInfo)
        {
            teServerName.Text = dbConfigInfo.DbServerName;
            teUserName.Text = dbConfigInfo.DbUserName;
            teUserPwd.Text = dbConfigInfo.DbUserPwd;
            teConfigName.Text = dbConfigInfo.DbConfigName;
            teConfigName.ReadOnly = true;

        }
        private DbConfigInfo getDbConfigInfo()
        {
            DbConfigInfo dbConfigInfo = new DbConfigInfo();
            dbConfigInfo.DbServerName = teServerName.Text.Trim();
            dbConfigInfo.DbUserName = teUserName.Text.Trim();
            dbConfigInfo.DbUserPwd = teUserPwd.Text.Trim();
            dbConfigInfo.DbType = "Oracle";
            dbConfigInfo.DbConfigName = teConfigName.Text.Trim();
            return dbConfigInfo;
        }
        #region
        private string emptyMsg = "不能为空";
        private bool checkData()
        {
            bool status = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(teServerName.Text))
            {
                errorProvider1.SetError(teServerName, emptyMsg);
                status = false;
            }
            if (string.IsNullOrEmpty(teUserName.Text))
            {
                errorProvider1.SetError(teUserName, emptyMsg);
                status = false;
            }
            if (string.IsNullOrEmpty(teUserPwd.Text))
            {
                errorProvider1.SetError(teUserPwd, emptyMsg);
                status = false;
            }

            if (string.IsNullOrEmpty(teConfigName.Text))
            {
                errorProvider1.SetError(teConfigName, emptyMsg);
                status = false;
            }
            return status;
        }
        #endregion

        internal DbConfigInfo DbConfigInfo = null;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (checkData())
            {
                DbConfigInfo = getDbConfigInfo();
                Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnAdd_Click(sender, e);
        }
    }
}
