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
using MDT.Tools.DB.TriggerGen.Plugin.Gen;

namespace MDT.Tools.DB.TriggerGen.Plugin.UI
{
    public partial class ExecuteSql : DockContent
    {
        public ExecuteSql()
        {
            InitializeComponent();

            tbScript.AllowCaretBeyondEOL = false;
            tbScript.ShowEOLMarkers = false;
            tbScript.ShowHRuler = false;
            tbScript.ShowInvalidLines = false;
            tbScript.ShowSpaces = false;
            tbScript.ShowTabs = false;
            tbScript.ShowVRuler = false;
            tbScript.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TSQL");
        }

        public IGenTrigger gt = null;
        private string sqlComment = "";

        public string SqlComment
        {
            get { return sqlComment; }
            set 
            { 
                sqlComment = value;
                tbScript.Text = value;
            }
        }


        public ExecuteSql(string sql,string text)
            : this()
        {
            this.SqlComment = sql;
            this.Text = text;
        }


        private void btnExecute_Click(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o => execute());
        }

        private void execute()
        {
            try
            {
                DNCCFrameWork.DataAccess.IDbHelper db = new DNCCFrameWork.DataAccess.DbFactory(
                    gt.dbConnectionString.Trim(new[] { '"' }),
                    DBType.GetDbProviderString(gt.dbType)).IDbHelper;

                string[] sql = tbScript.Text.Split(new[] { "--MQ TG" }, StringSplitOptions.RemoveEmptyEntries);
                if (gt.OriginalEncoding != null && gt.TargetEncoding != null)
                {
                    foreach (string str in sql)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            db.ExecuteNonQuery(str);
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
