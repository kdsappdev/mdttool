using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Java_CodeGen.Plugin.Model;
using MDT.Tools.DB.Java_CodeGen.Plugin.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.DB.Java_CodeGen.Plugin.Gen
{
    /// <summary>
    /// Java SpringConfig生成器
    /// </summary>
    internal class GenJavaSpringConfig
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
        public JavaCodeGenConfig cmc;
        public Encoding OriginalEncoding;
        public Encoding TargetEncoding;
        public string PluginName;
        private IbatisConfigHelper ibatisConfigHelper = new IbatisConfigHelper();
        public void GetDAOContext(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                setStatusBar("");
                setEnable(false);
                StringBuilder SqlMapConfig = new StringBuilder();
                StringBuilder DAOContext = new StringBuilder();
                StringBuilder WebServiceContext = new StringBuilder();
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
                            SqlMapConfig.Append(GenSqlMapConfig(drTable, drTableColumns));
                            DAOContext.Append(GetDAOContext(drTable, drTableColumns));
                            WebServiceContext.Append(GetWebServiceContext(drTable, drTableColumns));
                        }
                    }

                }

                if (!cmc.IsShowGenCode)
                {

                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.SqlMapConfig, new[] { SqlMapConfig.ToString() });
                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.DAOContext, new[] { DAOContext.ToString() });
                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.WebServiceContext, new[] { WebServiceContext.ToString() });

                    setStatusBar(string.Format("Spring配置生成成功"));
                    openDialog();
                }
                else
                {
                    CodeShow(CodeGenRuleHelper.SqlMapConfig, SqlMapConfig.ToString());
                    CodeShow(CodeGenRuleHelper.DAOContext, DAOContext.ToString());
                    CodeShow(CodeGenRuleHelper.WebServiceContext, WebServiceContext.ToString());
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

        public string GenSqlMapConfig(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            sb.AppendFormat("<sqlMap resource=\"ats/common/model/dao/{0}_SqlMap.xml\" />", tableName).AppendFormat("\r\n");
            return sb.ToString();
        }

        public string GetDAOContext(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            string className = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName));
            string dao = className + CodeGenRuleHelper.DAO;
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<!-- {0} -->", dao).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<bean id=\"{0}\" class=\"ats.common.model.dao.{0}Impl\">", dao).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"sqlMapClientTemplate\">").AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"sqlMapClientTemplate\" />").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</bean>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            return sb.ToString();
        }

        public string GetWebServiceContext(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            string model = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName));
            string bsServer = model + CodeGenRuleHelper.BSServer;
            string wsServer = model + CodeGenRuleHelper.WSService;
            string dao = model + CodeGenRuleHelper.DAO;

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<!-- {0} bs-->", bsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<bean id=\"I{0}\" class=\"{1}.impl.{0}\">", bsServer,cmc.BSPackage).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"{0}\">", CodeGenHelper.StrFirstToLower(dao)).AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"{0}\" />",dao).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"{0}\">", "dataCheckServer").AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"{0}\" />", "DataCheckServer").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</bean>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("<!-- {0} ws-->", wsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<bean id=\"I{0}\" class=\"{1}.impl.{0}\">", wsServer, cmc.WSPackage).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"i{0}\">", bsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"I{0}\" />", bsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</bean>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("<!-- {0} jaxws-->", wsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<jaxws:server id=\"I{0}\" serviceClass=\"{1}.I{0}\" address=\"/I{0}\">", wsServer, cmc.WSPackage).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<jaxws:serviceBean>").AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"I{0}\" />", wsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</jaxws:serviceBean>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</jaxws:server>").AppendFormat("\r\n");

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
        public GenJavaSpringConfig()
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
