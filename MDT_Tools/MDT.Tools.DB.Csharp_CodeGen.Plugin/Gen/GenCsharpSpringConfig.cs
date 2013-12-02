using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Model;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Gen
{
    /// <summary>
    /// Csharp SpringConfig生成器
    /// </summary>
    internal class GenCsharpSpringConfig
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
        public CsharpCodeGenConfig cmc;
        public Encoding OriginalEncoding;
        public Encoding TargetEncoding;
        public string PluginName;
        private IbatisConfigHelper ibatisConfigHelper = new IbatisConfigHelper();
        public void GetSpringConfig(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                setStatusBar("");
                setEnable(false);
                
                StringBuilder DAL = new StringBuilder();
 
                if (drTables != null && dsTableColumns != null)
                {
                    if (cmc.CodeRule == CodeGenRuleHelper.Ibatis)
                    {
                        ibatisConfigHelper.ReadConfig(cmc.Ibatis);
                    }

                    if (!cmc.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(cmc.OutPut);
                        setStatusBar(string.Format("正在生成Spring配置文件"));
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
                            setStatusBar(string.Format("正在生成Spring配置{0}的配置,共{1}个配置，已生成了{2}个配置,过滤了{3}个配置",
                                                       cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(className) : CodeGenHelper.StrFirstToUpperRemoveUnderline(className),
                                                       drTables.Length, i - j, j));
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");
                            
                            DAL.Append(GetSpringConfig(drTable, drTableColumns));
                           
                        }
                    }

                }

                if (!cmc.IsShowGenCode)
                {

                   
                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.DAL, new[] { DAL.ToString() });
                    

                    setStatusBar(string.Format("Spring配置生成成功"));
                    openDialog();
                }
                else
                {
                     
                    CodeShow("Object.xml", DAL.ToString());
                    
                }
            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("Spring配置生成失败[{0}]", ex.Message));

            }
            finally
            {
                setEnable(true);
            }



        }
        private void openDialog()
        {
            DialogResult result = MessageBox.Show(MainContextMenu, string.Format("文件已保存成功,是否要打开文件保存目录."), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.Yes))
            {
                Process.Start("Explorer.exe", cmc.OutPut);
            }
        }

     

        public string GetSpringConfig(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            string className = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName));
            string dal = className + CodeGenRuleHelper.DALServer;
            string bll = cmc.PluginName + CodeGenRuleHelper.BLLService;
            #region DAL
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<!-- I{0} DAL-->", dal).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<object id=\"I{0}\" class=\"{1}.{0}\">", dal,cmc.DALNameSpace).AppendFormat("\r\n");

            if (cmc.IsShowComment)
            {
                sb.AppendFormat("\t\t").AppendFormat("<!--数据拦截，系统特殊的数据，或系统部分缺陷造成的，业务不正确，而进行数据拦截-->").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("<!--").AppendFormat("<property name=\"Filters\">").AppendFormat(
                    "\r\n");
                sb.AppendFormat("\t\t").AppendFormat("<list name=\"Filters\">").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("<ref object=\"##DALFilter\" />").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("</list>").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("-->").AppendFormat("\r\n");
            }
            sb.AppendFormat("\t").AppendFormat("</object>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion
               
            #region BLL
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<!-- I{0} BLL-->", bll).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<object id=\"I{0}\" class=\"{1}.{0}\">", bll,cmc.BLLNameSpace).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"I{0}\" ref=\"I{0}\"/>",dal).AppendFormat("\r\n");
            if (cmc.IsShowComment)
            {
                sb.AppendFormat("\t\t").AppendFormat("<!--数据拦截，根据客户定制化进行数据拦截处理-->").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("<!--").AppendFormat("<property name=\"Filters\">").AppendFormat(
                    "\r\n");
                sb.AppendFormat("\t\t").AppendFormat("<list>").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("<ref object=\"##BLLFilter\" />").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("</list>").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("-->").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("<!--监听DAL层实时数据-->").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("<!--").AppendFormat(
                    "<listener  event=\"OnDALData\" method=\"OnData\">").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("<ref object=\"I{0}\"/>", dal).AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("</listener>").AppendFormat("-->").AppendFormat("\r\n");
            }
            sb.AppendFormat("\t").AppendFormat("</object>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion


            #region UI
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<!-- {0} UI-->", cmc.PluginName).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<object id=\"{0}\" class=\"{1}.{0}GUI\">",cmc.PluginName, cmc.BLLNameSpace).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"I{0}\" ref=\"I{0}\"/>", bll).AppendFormat("\r\n");
            if (cmc.IsShowComment)
            {
                sb.AppendFormat("\t\t").AppendFormat("<!--监听BLL层实时数据-->").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("<!--").AppendFormat(
                    "<listener  event=\"OnBLLData\" method=\"OnData\">").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("<ref object=\"I{0}\"/>", bll).AppendFormat("\r\n");
                sb.AppendFormat("\t\t").AppendFormat("</listener>").AppendFormat("-->").AppendFormat("\r\n");
            }
            sb.AppendFormat("\t").AppendFormat("</object>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion


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
                Code mf = new Code() { CodeLanguage = "XML", Text = titile, CodeContent = codeContent};
                mf.tbCode.ContextMenuStrip = cms;
                mf.Show(Panel);
            }
        }
        public GenCsharpSpringConfig()
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
                    FileHelper.Write(cmc.OutPut + code.Text, new string[] { code.CodeContent }, Encoding.GetEncoding("GBK"));
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
                    FileHelper.Write(cmc.OutPut + code.Text, new string[] { code.CodeContent }, Encoding.GetEncoding("GBK"));
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
