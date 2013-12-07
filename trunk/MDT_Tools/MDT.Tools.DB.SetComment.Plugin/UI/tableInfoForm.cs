using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;
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
            dgvTableInfo.AutoGenerateColumns = false;
        }

        public TableInfo tableInfo;
        public TableInfo TableInfo
        {
            set
            {
                tableInfo = value;
                tbComment.Text = tableInfo.TableComments;
                Text = tableInfo.TableName + "表基本信息";
                dgvTableInfo.DataSource = tableInfo.Columns;
                dgvTableInfo.Refresh();
                bindSql();
            }
        }

        public setComment sc;

        private readonly NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);

        private void bindSql()
        {

            tbScript.Text = createTableSql() + createCommentSql();
        }
        private string createTableSql()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("tableInfo", tableInfo);
            dic.Add("codeGenHelper", new CodeGenHelper());
            return nVelocityHelper.GenByTemplate("createtable.sql.vm", dic);
        }

        private string createCommentSql()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("tableInfo", tableInfo);
            dic.Add("codeGenHelper", new CodeGenHelper());
            return nVelocityHelper.GenByTemplate("comment.sql.vm", dic);

        }

        private void dgvTableInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bindSql();
        }

        private void tbComment_TextChanged(object sender, EventArgs e)
        {
            tableInfo.TableComments = tbComment.Text;
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
                        temp = EncodingHelper.
                                ConvertEncoder(sc.TargetEncoding,
                                               sc.OriginalEncoding,
                                               temp);
                    }

                    DataRow[] drs = sc.dsTable.Tables[sc.dbName + sc.DBtable].Select("name = '" + tableInfo.TableName + "'");

                    if (drs != null && drs.Length > 0)
                    {
                        drs[0]["comments"] = temp;
                        flag = true;
                        DBFileHelper.WriteXml(sc.dsTable);
                    }
                }
                foreach (ColumnInfo column in tableInfo.Columns)
                {
                    if (!string.IsNullOrEmpty(column.Comments))
                    {
                        temp = column.Comments;
                        DataRow[] drs = sc.dsTableColumn.Tables[sc.dbName + sc.DBtablesColumns].Select("TABLE_NAME = '" + tableInfo.TableName + "' and COLUMN_NAME = '" + column.Name + "'");

                        if (drs != null && drs.Length > 0 && sc.OriginalEncoding != null &&
                            sc.OriginalEncoding != null)
                        {
                            temp = EncodingHelper.ConvertEncoder(
                                        sc.TargetEncoding,
                                        sc.OriginalEncoding, temp);
                        }
                        drs[0]["COMMENTS"] = temp;
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
                    string[] sql = createCommentSql().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (sc.OriginalEncoding != null &&
                        sc.OriginalEncoding != null)
                    {
                        foreach (var s in sql)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                string t = s.Trim(new[] { '\t', '\r', '\n', ' ', ';' });
                                t = EncodingHelper.
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
