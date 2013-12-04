using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.DB.Common
{
    public abstract class AbstractHandler
    {
        public string dbName;
        public string dbType;
        public string dbConnectionString;
        public ToolStripStatusLabel tsslMessage;
        public ToolStripProgressBar tspbLoadDBProgress;
        public string DBtable;
        public string DBtablesColumns;
        public string DBviews;
        public string DBtablesPrimaryKeys;
        public DataSet dsTable;
        public DataSet dsTableColumn;
        public DataSet dsTablePrimaryKey;
        public ContextMenuStrip MainContextMenu;
        public ToolStripItem tsiGen;
        public DockPanel Panel;
        protected string OutPut ;
        public Encoding OriginalEncoding;
        public Encoding TargetEncoding;
        public string PluginName;
        

        public abstract void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys);
         

        protected void openDialog()
        {
            DialogResult result = MessageBox.Show(MainContextMenu, string.Format("文件已保存成功,是否要打开文件保存目录."), @"提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.Yes))
            {
                Process.Start("Explorer.exe", OutPut);
            }
        }

        protected delegate void CodeShowDel(string titile, string codeContent);

        protected string CodeLanguage = "C#";
        protected void CodeShow(string titile, string codeContent)
        {
            if (MainContextMenu.InvokeRequired)
            {
                var s = new CodeShowDel(CodeShow);
                MainContextMenu.Invoke(s, new object[] { titile, codeContent });
            }
            else
            {
                Code mf = new Code() { CodeLanguage = CodeLanguage, Text = titile, CodeContent = codeContent };
                mf.tbCode.ContextMenuStrip = cms;
                mf.Show(Panel);
            }
        }



        protected ContextMenuStrip cms = new ContextMenuStrip();
        void mf_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cms.Show(sender as Control, e.Location);
            }
        }
        protected readonly ToolStripItem _tsiSave = new ToolStripMenuItem();
        protected readonly ToolStripItem _tsiSaveAll = new ToolStripMenuItem();
        protected delegate void Simple();
        protected void AddContextMenu()
        {
                _tsiSave.Text = "保存";
                _tsiSave.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsiSave.Click += _tsiSave_Click;
                _tsiSaveAll.Text = "全部保存";
                _tsiSaveAll.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsiSaveAll.Click += new EventHandler(_tsiSaveAll_Click);
                cms.Items.Add(_tsiSave);
                cms.Items.Add(_tsiSaveAll);
        }

        void _tsiSaveAll_Click(object sender, EventArgs e)
        {
            IDockContent[] documents = Panel.DocumentsToArray();
            FileHelper.DeleteDirectory(OutPut);
            foreach (var v in documents)
            {
                Code code = v as Code;
                if (v != null)
                {
                    FileHelper.Write(OutPut + code.Text, new string[] { code.CodeContent });
                }
            }
            openDialog();
        }

        void _tsiSave_Click(object sender, EventArgs e)
        {
            var code = Panel.ActiveContent as Code;
            FileHelper.DeleteDirectory(OutPut);
            if (code != null)
            {
                try
                {
                    FileHelper.Write(OutPut + code.Text, new string[] { code.CodeContent });
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            openDialog();
        }


        #region 进度条，状态栏
        protected void setProgreesEditValue(int i)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgreesEditValue);
                MainContextMenu.Invoke(s, new object[] { i });
            }
            else
            {
                tspbLoadDBProgress.Value = i;
                ;
            }

        }
        protected delegate void SimpleInt(int i);
        protected delegate void SimpleStr(string str);
        protected void setProgressMax(int i)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgressMax);
                MainContextMenu.Invoke(s, new object[] { i });

            }
            else
            {
                tspbLoadDBProgress.Maximum = i;
            }

        }
        protected void setProgress(int i)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgress);
                MainContextMenu.Invoke(s, new object[] { i });

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
        protected void setStatusBar(string str)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleStr s = new SimpleStr(setStatusBar);
                MainContextMenu.Invoke(s, new object[] { str });

            }
            else
            {
                tsslMessage.Text = str;

            }
        }

        protected delegate void SimpleBool(bool flag);
        protected void setEnable(bool flag)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleBool s = new SimpleBool(setEnable);
                MainContextMenu.Invoke(s, new object[] { flag });

            }
            else
            {
                tsiGen.Enabled = flag;
                tspbLoadDBProgress.Visible = !flag;
            }
        }
        #endregion

 
    }
}
