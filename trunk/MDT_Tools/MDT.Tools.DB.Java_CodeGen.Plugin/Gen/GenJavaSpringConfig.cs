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
using MDT.Tools.DB.Common;
namespace MDT.Tools.DB.Java_CodeGen.Plugin.Gen
{
    /// <summary>
    /// Java SpringConfig生成器
    /// </summary>
    internal class GenJavaSpringConfig : AbstractHandler
    {

        public JavaCodeGenConfig cmc;
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
                CodeLanguage = "XML";
                OutPut = cmc.OutPut;
                setStatusBar("");
                setEnable(false);
                StringBuilder SqlMapConfig = new StringBuilder();
                StringBuilder DAOContext = new StringBuilder();
                StringBuilder WebServiceContext = new StringBuilder();
                if (drTables != null && dsTableColumns != null)
                {
                    if (cmc.CodeRule == CodeGenRuleHelper.Ibatis)
                    {
                        CodeGenHelper.ReadConfig(cmc.Ibatis);
                    }

                    if (!cmc.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(cmc.OutPut);
                        setStatusBar(string.Format("正在生成Spring配置文件"));
                        LogHelper.Info("Generating spring configuration file.");
                        setProgreesEditValue(0);
                        setProgress(0);
                        setProgressMax(drTables.Length);
                    }
                    int j = 0;
                    for (int i = 0; i < tableInfos.Count; i++)
                    {
                        string tableName = tableInfos[i].TableName;
                        string[] temp = cmc.TableFilter.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                        bool flag = false;
                        foreach (var str in temp)
                        {
                            if (tableName.StartsWith(str))//过滤
                            {
                                flag = true;
                                j++;
                                break;
                            }

                        }
                        if (!cmc.IsShowGenCode)
                        {
                            setStatusBar(string.Format("正在生成Spring配置{0}的配置,共{1}个配置，已生成了{2}个配置,过滤了{3}个配置",
                                                       tableName,
                                                       drTables.Length, i - j, j));
                            LogHelper.Info("Generationg spring configuratin + " + tableName + "'s config, " + drTables.Length + " of configuration, " + (i - j) + " of configuration has generated, " + j + " of configuration was filtered.");
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            SqlMapConfig.Append(GenSqlMapConfig(tableInfos[i]));
                            DAOContext.Append(GetDAOContext(tableInfos[i]));
                            WebServiceContext.Append(GetWebServiceContext(tableInfos[i]));
                        }
                    }

                }

                if (!cmc.IsShowGenCode)
                {

                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.SqlMapConfig, new[] { SqlMapConfig.ToString() });
                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.DAOContext, new[] { DAOContext.ToString() });
                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.WebServiceContext, new[] { WebServiceContext.ToString() });

                    setStatusBar(string.Format("Spring配置生成成功"));
                    LogHelper.Info("spring config has generated success.");
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
                LogHelper.Error("Spring config generated fail " + ex.Message);
            }
            finally
            {
                setEnable(true);
            }
        }

        private readonly NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);

        public string GenSqlMapConfig(TableInfo tableInfo)
        {
            string path = string.Format(@"{0}", "sqlmapconfig.xml.vm");
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);
            dic.Add("bsPackage", cmc.BSPackage);
            dic.Add("wsPackage", cmc.WSPackage);
            dic.Add("codeRule", cmc.CodeRule);
            string str = nVelocityHelper.GenByTemplate(path, dic);
            return str;
        }

        public string GetDAOContext(TableInfo tableInfo)
        {
            string path = string.Format(@"{0}", "daocontext.xml.vm");
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);
            dic.Add("bsPackage", cmc.BSPackage);
            dic.Add("wsPackage", cmc.WSPackage);
            dic.Add("codeRule", cmc.CodeRule);
            string str = nVelocityHelper.GenByTemplate(path, dic);
            return str;
        }

        public string GetWebServiceContext(TableInfo tableInfo)
        {
            string path = string.Format(@"{0}", "webserviceContext.xml.vm");
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);
            dic.Add("bsPackage", cmc.BSPackage);
            dic.Add("wsPackage", cmc.WSPackage);
            dic.Add("codeRule", cmc.CodeRule);
            string str = nVelocityHelper.GenByTemplate(path, dic);
            return str;
        }

        public GenJavaSpringConfig()
        {
            AddContextMenu();
        }
    }
}
