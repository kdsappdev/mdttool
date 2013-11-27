using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Csharp_Model.Plugin.Utils;
using MDT.Tools.DB.Csharp_ModelGen.Plugin.Model;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.DB.Csharp_Model.Plugin.Gen
{
    /// <summary>
    /// Csharp Model实体类属性生成器
    /// </summary>
    internal class GenCsharpModel
    {
        public string dbName;
        public string dbType;
        public ToolStripStatusLabel tsslMessage;
        public ToolStripProgressBar tspbLoadDBProgress;
        public string DBtable;
        public string DBtablesColumns;
        public string DBviews;
        public string DBtablesPrimaryKeys;
        public DataSet dsTableColumn;
        public DataSet dsTablePrimaryKey;
        public ContextMenuStrip MainContextMenu;
        public ToolStripItem tsiGen;
        public DockPanel Panel;
        public CsharpModelGenConfig cmc;
        public void GenCode(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                setEnable(false);
                string[] strs = null;
                if (drTables != null && dsTableColumns != null)
                {

                    strs = new string[drTables.Length];
                    if (!cmc.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(cmc.OutPut);
                        setStatusBar(string.Format("正在生成{0}命名空间的Model", cmc.NameSpace));
                        setProgreesEditValue(0);
                        setProgress(0);
                        setProgressMax(drTables.Length);
                    }
                    int j = 0;
                    for (int i = 0; i < drTables.Length; i++)
                    {
                        DataRow drTable = drTables[i];
                        string className = drTable["name"] + "";
                        string[] temp = cmc.TableFilter.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                        bool flag = false;
                        foreach (var str in temp)
                        {
                            if (className.StartsWith(str))//过滤
                            {
                                flag = true;
                                j++;
                                break;
                            }

                        }
                        if (!cmc.IsShowGenCode)
                        {
                            setStatusBar(string.Format("正在生成{0}命名空间中{1}信息,共{2}个Model，已生成了{3}个Model,过滤了{4}个Model", cmc.NameSpace,
                                                       CodeGenHelper.StrFirstToUpperRemoveUnderline(className),
                                                       drTables.Length, i-j,j));
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");
                            strs[i] = GenCode(drTable, drTableColumns);
                        }
                    }

                }
                
                if (!cmc.IsShowGenCode)
                {
                    setStatusBar(string.Format("{0}命名空间Model生成成功", cmc.NameSpace));
                    openDialog();
                }
            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("{0}命名空间Model生成失败[{1}]", cmc.NameSpace,ex.Message));
                
            }
            finally
            {
                setEnable(true);
            }


            
        }
        private void openDialog()
        {
            DialogResult result = MessageBox.Show(string.Format("文件已保存成功,是否要打开文件保存目录."), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.Yes))
            {
                Process.Start("Explorer.exe", cmc.OutPut);
            }
        }

        public string GenCode(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();

            #region 引入命名空间
            sb.AppendFormat("using System;").AppendFormat("\r\n");
            sb.AppendFormat("using System.Collections.Generic;").AppendFormat("\r\n");
            sb.AppendFormat("using System.Text;").AppendFormat("\r\n");
            sb.AppendFormat("using System.Xml.Serialization;").AppendFormat("\r\n");
            #endregion

            #region 命名空间
            sb.AppendFormat("namespace {0}", cmc.NameSpace).AppendFormat("\r\n");
            sb.Append("{").AppendFormat("\r\n");

            #region 类名
            string className = drTable["name"] as string;
            className = CodeGenHelper.StrFirstToUpperRemoveUnderline(className);
            sb.AppendFormat("\t").AppendFormat("[Serializable]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public class {0}", className).AppendFormat("\r\n");
            sb.Append("\t{").AppendFormat("\r\n");

            #region 字段
            sb.AppendFormat("\t\t").AppendFormat("#region 字段").AppendFormat("\r\n").AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                string dataType = "string";
                string nullAble = dr["NULLABLE"] + "";
                dataType = DataTypeMappingHelper.GetCSharpDataTypeByDbType(dbType, dr["DATA_TYPE"] + "", dr["DATA_SCALE"] + "", dr["DATA_LENGTH"] + "", "Y".Equals(nullAble));
                string columnName = dr["COLUMN_NAME"] as string;
                string fieldName = Utils.CodeGenHelper.StrFieldWith_(columnName);
                string defaultValue = dr["DATA_DEFAULT"] as string;


                string comments = dr["COMMENTS"] as string;
                sb.AppendFormat("\t\t").AppendFormat("private {0} {1}", dataType, fieldName);
                if (nullAble.Equals("N") || !string.IsNullOrEmpty(defaultValue))
                {
                    defaultValue = CodeGenHelper.GetDefaultValueByDataType(dataType, defaultValue);
                    sb.AppendFormat(" = {0}", defaultValue);
                }
                sb.AppendFormat(";");
                if (!string.IsNullOrEmpty(comments))
                {
                    sb.AppendFormat("//{0}", comments);
                }
                sb.AppendFormat("\r\n");
            }
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("#endregion").AppendFormat("\r\n");
            #endregion
            sb.AppendFormat("\r\n");

            #region 属性
            sb.AppendFormat("\t\t").AppendFormat("#region 属性").AppendFormat("\r\n").AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                var dataType = "string";
                string nullAble = dr["NULLABLE"] as string;
                dataType = Utils.DataTypeMappingHelper.GetCSharpDataTypeByDbType(dbType, dr["DATA_TYPE"] + "", dr["DATA_SCALE"] + "", dr["DATA_LENGTH"] + "", "Y".Equals(nullAble));

                string columnName = dr["COLUMN_NAME"] as string;
                string fieldName = CodeGenHelper.StrFieldWith_(columnName);
                string propertyName = CodeGenHelper.StrProperty(columnName);
                sb.AppendFormat("\t\t").AppendFormat("public {0} {1}", dataType, propertyName).AppendFormat("\r\n");
                sb.AppendFormat("\t\t").Append("{").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("get ").Append("{ ").AppendFormat("return {0};", fieldName).Append(" }").AppendFormat("\r\n");

                sb.AppendFormat("\t\t\t").AppendFormat("set ").Append("{ ").AppendFormat("{0} = value;", fieldName).Append(" }").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").Append("}").AppendFormat("\r\n");
                sb.AppendFormat("\r\n");
            }
            sb.AppendFormat("\t\t").AppendFormat("#endregion").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            sb.Append("\t}").AppendFormat("\r\n");
            #endregion

            sb.Append("}");
            #endregion

            string title = CodeGenHelper.StrFirstToUpperRemoveUnderline(drTable["name"] as string) + ".cs";

            if (cmc.IsShowGenCode)
            {
                CodeShow(title, sb.ToString());
            }
            else
            {
                FileHelper.Write(cmc.OutPut +title, new [] { sb.ToString() });
            }

            return sb.ToString();
        }

        private delegate void CodeShowDel(string titile, string codeContent);
        private void CodeShow(string titile, string codeContent)
        {
            if (MainContextMenu.InvokeRequired)
            {
                var s = new CodeShowDel(CodeShow);
                MainContextMenu.Invoke(s, new object[] { titile, codeContent });
            }
            else
            {
                Code mf = new Code() { Text = titile, CodeContent = codeContent };
                //mf.MouseClick += (mf_MouseClick);
                //mf.ContextMenuStrip = cms;
                mf.richTextBox1.ContextMenuStrip = cms;
                mf.Show(Panel);
            }
        }
        public GenCsharpModel()
        {
            AddContextMenu();
        }

        ContextMenuStrip cms = new ContextMenuStrip();
        void mf_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cms.Show(sender as Control, e.Location);
            }
        }
        private readonly ToolStripItem _tsiSave = new ToolStripMenuItem();
        private readonly ToolStripItem _tsiSaveAll = new ToolStripMenuItem();
        private delegate void Simple();
        private void AddContextMenu()
        {
            //if (MainContextMenu.InvokeRequired)
            //{
            //    var s = new Simple(AddContextMenu);
            //    MainContextMenu.Invoke(s, null);
            //}
            //else
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
        }

        void _tsiSaveAll_Click(object sender, EventArgs e)
        {
            IDockContent[] documents = Panel.DocumentsToArray();
            FileHelper.DeleteDirectory(cmc.OutPut);
            foreach (var v in documents)
            {
                Code code = v as Code;
                if (v != null)
                {
                    FileHelper.Write(cmc.OutPut + code.Text, new string[] { code.CodeContent });
                }
            }
            openDialog();
        }

        void _tsiSave_Click(object sender, EventArgs e)
        {
            var code = Panel.ActiveContent as Code;
            FileHelper.DeleteDirectory(cmc.OutPut);
            if (code != null)
            {
                try
                {
                    FileHelper.Write(cmc.OutPut + code.Text, new string[] { code.CodeContent });
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            openDialog();
        }

        #region
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
        delegate void SimpleInt(int i);
        delegate void SimpleStr(string str);
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

        private delegate void SimpleBool(bool flag);
        private void setEnable(bool flag)
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
