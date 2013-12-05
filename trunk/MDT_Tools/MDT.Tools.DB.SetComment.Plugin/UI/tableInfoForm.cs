using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Resources;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Actions;
using MDT.Tools.DB.Common;
namespace MDT.Tools.DB.SetComment.Plugin.UI
{
    internal partial class tableInfoForm : DockContent
    {
        public tableInfoForm()
        {
            InitializeComponent();
            btnExecute.Image = Resources.start;
            tbScript.AllowCaretBeyondEOL = false;
            tbScript.ShowEOLMarkers = false;
            tbScript.ShowHRuler = false;
            tbScript.ShowInvalidLines = false;
            tbScript.ShowSpaces = false;
            tbScript.ShowTabs = false;
            tbScript.ShowVRuler = false;
            tbScript.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TSQL");

        }
        public DataRow drTable;
        public DataRow[] drTableColumns;
        public setComment sc;
        private DataTable dataTable;
        public DataTable DataTable
        {
            set
            {
                dataTable = value;
                dgvTableInfo.DataSource = dataTable;
                dgvTableInfo.Refresh();
                bindSql();
            }
        }
        private string tableNameComment;
        public string TableNameComment
        {
            set
            {
                tbComment.Text = value;
                tableNameComment = value;
            }
        }
        private string tableName;
        public string TableName
        {
            set
            {
                tableName = value;
                Text = tableName + "表基本信息";
            }
        }

        private void bindSql()
        {

            tbScript.Text = createTableSql() + createCommentSql();
        }
        private string createTableSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\r\n");
            if (dataTable != null)
            {
                sb.AppendFormat("\t").AppendFormat("CREATE OR REPLACE TABLE {0}", tableName).Append(" {").AppendFormat("\r\n");
                foreach (DataRow dr in dataTable.Rows)
                {
                    sb.AppendFormat("\t\t").AppendFormat("{0} {1} {2} {3}", dr["列名"], dr["数据类型"], (dr["是否NULL"] + "").Equals("N") ? "NULL" : "NOT NULL", string.IsNullOrEmpty(dr["默认值"] + "") ? "" : " default " + dr["默认值"]).Append(" ,").AppendFormat("\r\n");
                }
                string str = sb.ToString().Trim(new[] { ',' });
                sb = new StringBuilder();
                sb.Append(str);
                sb.AppendFormat("\t").Append("}").AppendFormat("\r\n");
            }
            return sb.ToString();
        }

        private string createCommentSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\r\n");
            if (dataTable != null)
            {
                if (!string.IsNullOrEmpty(tbComment.Text.Trim()))
                {
                    sb.AppendFormat("\t").AppendFormat("comment on table {0} is '{1}';", tableName, tbComment.Text.Trim()).AppendFormat("\r\n");
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["备注"] + ""))
                    {
                        sb.AppendFormat("\t").AppendFormat("comment on column {0}.{1} is '{2}';", tableName, dr["列名"], dr["备注"]).AppendFormat("\r\n");
                    }
                }
                string str = sb.ToString().Trim(new[] { ';' });
                sb = new StringBuilder();
                sb.Append(str).AppendFormat("\r\n");
            }
            return sb.ToString();

        }


        private void dgvTableInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvTableInfo.Columns[e.ColumnIndex].Name.Equals("备注"))
            {
                dgvTableInfo.ReadOnly = false;
            }
        }

        private void dgvTableInfo_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvTableInfo.ReadOnly = true;
        }

        private void dgvTableInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTableInfo.Columns[e.ColumnIndex].Name.Equals("备注"))
            {
                bindSql();
            }
        }

        private void tbComment_TextChanged(object sender, EventArgs e)
        {
            bindSql();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o => execute());
        }

        private void execute()
        {
            try
            {
                bool flag = false;
                string temp = tbComment.Text.Trim();
                if (!string.IsNullOrEmpty(temp))
                {
                    if (sc.OriginalEncoding != null &&
                        sc.OriginalEncoding != null)
                    {
                        temp =
                            MDT.Tools.Core.Utils.EncodingHelper.
                                ConvertEncoder(sc.TargetEncoding,
                                               sc.OriginalEncoding,
                                               temp);
                    }
                    drTable["comments"] = temp;
                    flag = true;
                    DBFileHelper.WriteXml(sc.dsTable);
                }
                foreach (DataRow dr in drTableColumns)
                {
                    DataRow[] drs =
                        dataTable.Select("列名 = '" +
                                         dr["COLUMN_NAME"].
                                             ToString() + "'");

                    if (drs != null &&
                        !string.IsNullOrEmpty(drs[0]["备注"] + ""))
                    {
                        temp = drs[0]["备注"] + "";
                        if (sc.OriginalEncoding != null &&
                            sc.OriginalEncoding != null)
                        {
                            temp =
                                MDT.Tools.Core.Utils.
                                    EncodingHelper.ConvertEncoder(
                                        sc.TargetEncoding,
                                        sc.OriginalEncoding, temp);
                        }
                        dr["COMMENTS"] = temp;
                        flag = true;
                        DBFileHelper.WriteXml(sc.dsTableColumn);
                    }
                }
                if (flag)
                {
                    DNCCFrameWork.DataAccess.IDbHelper db =
                        new DNCCFrameWork.DataAccess.DbFactory(
                            sc.dbConnectionString.Trim(new[] { '"' }),
                            DBType.GetDbProviderString(sc.dbType))
                            .IDbHelper;
                    string[] sql = createCommentSql().Split(new string[]{";"},StringSplitOptions.RemoveEmptyEntries);
                    if (sc.OriginalEncoding != null &&
                        sc.OriginalEncoding != null)
                    {
                        foreach (var s in sql)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                string t = s.Trim(new[] {'\t', '\r', '\n', ' ', ';'});
                                t = MDT.Tools.Core.Utils.EncodingHelper.
                                    ConvertEncoder(sc.TargetEncoding,
                                                   sc.OriginalEncoding,
                                                   t);
                                if (!string.IsNullOrEmpty(t))
                                {
                                    db.ExecuteNonQuery(t);
                                }
                            }
                        }

                    }

                }
                MessageBox.Show("执行成功", "提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败[" + ex.Message + "]", "提示",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }



    }
}
