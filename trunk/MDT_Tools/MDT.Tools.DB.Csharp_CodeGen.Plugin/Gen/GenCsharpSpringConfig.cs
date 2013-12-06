using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Common;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Model;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Gen
{
    /// <summary>
    /// Csharp SpringConfig生成器
    /// </summary>
    internal class GenCsharpSpringConfig : AbstractHandler
    {
        
        public CsharpCodeGenConfig cmc;
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
                CodeLanguage = "XML";
                OutPut = cmc.OutPut;
                setStatusBar("");
                setEnable(false);

                StringBuilder DAL = new StringBuilder();

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
                                                       CodeGenHelper.GetClassName(tableName, cmc.CodeRule),
                                                       drTables.Length, i - j, j));
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            DAL.Append(GetSpringConfig(tableInfos[i]));
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
        private readonly NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);
        
        public string GetSpringConfig(TableInfo tableInfo)
        {
            string tableName = tableInfo.TableName;
            string className = CodeGenHelper.GetClassName(tableName, cmc.CodeRule);
            string path = string.Format(@"{0}", "object.xml.vm");
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);
            dic.Add("modelNameSpace", cmc.ModelNameSpace);
            dic.Add("idalNameSpace", cmc.IDALNameSpace);
            dic.Add("dalNameSpace", cmc.DALNameSpace);
            dic.Add("bllNameSpace", cmc.BLLNameSpace);
            dic.Add("guiPluginName", cmc.PluginName);
            dic.Add("codeRule", cmc.CodeRule);
            string str = nVelocityHelper.GenByTemplate(path, dic);

            return str;
        }
 
        public GenCsharpSpringConfig()
        {
            AddContextMenu();
        }
 
     
 

 


    }
}
