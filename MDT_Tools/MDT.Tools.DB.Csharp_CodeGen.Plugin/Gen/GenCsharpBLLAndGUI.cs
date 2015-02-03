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
    /// Csharp BLL GUI生成器
    /// </summary>
    internal class GenCsharpBLLAndGUI:AbstractHandler
    {
       
        public CsharpCodeGenConfig cmc;
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables,dsTableColumns,dsTablePrimaryKeys);
                OutPut = cmc.OutPut;
                setEnable(false);
                setStatusBar("");
                if (drTables != null && dsTableColumns != null)
                {
                    if (cmc.CodeRule == CodeGenRuleHelper.Ibatis)
                    {
                        CodeGenHelper.ReadConfig(cmc.Ibatis);
                    }
                    if (!cmc.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(cmc.OutPut);
                        setStatusBar(string.Format("正在生成{0}命名空间的BLL&GUI", cmc.BLLNameSpace));
                        LogHelper.Info("Generating " + cmc.BLLNameSpace + " namespace BLL&GUI.");
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
                            setStatusBar(string.Format("正在生成{0}命名空间中{1}代码,共{2}个代码，已生成了{3}个代码,过滤了{4}个代码", cmc.BLLNameSpace,
                                                        CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.BLLService,
                                                       drTables.Length, i - j, j));
                            LogHelper.Info("Generationg in " + cmc.BLLNameSpace + " namespace " + CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.BLLService
                                + " code, of " + drTables.Length + " codes, " + (i - j) + " of code has generated, " + j + " filtering code");
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            GenCodeInterface(tableInfos[i]);
                            GenCode(tableInfos[i]);
                        }
                    }

                }

                if (!cmc.IsShowGenCode)
                {
                    setStatusBar(string.Format("{0}命名空间代码生成成功", cmc.BLLNameSpace));
                    LogHelper.Info(cmc.BLLNameSpace + " namespace code has generated success.");
                    openDialog();
                }
            }
            catch (Exception ex)
            {
              
                setStatusBar(string.Format("{0}命名空间代码生成失败[{1}]", cmc.BLLNameSpace, ex.Message));

                LogHelper.Error(cmc.BLLNameSpace + " namespace code generated fail " + ex.Message);
              
            }
            finally
            {
                setEnable(true);
            }

        }


        private readonly NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);

        public void GenCodeInterface(TableInfo tableInfo)
        {
            string tableName = tableInfo.TableName;
            string className = "I" + CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.BLLService; ;
            string path = string.Format(@"{0}", "ibll.cs.vm");
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);
            dic.Add("modelNameSpace", cmc.ModelNameSpace);
            dic.Add("idalNameSpace", cmc.IDALNameSpace);
            dic.Add("dalNameSpace", cmc.DALNameSpace);
            dic.Add("bllNameSpace", cmc.BLLNameSpace);
            dic.Add("guiPluginName", cmc.PluginName);
            dic.Add("codeRule", cmc.CodeRule);
            string str = nVelocityHelper.GenByTemplate(path, dic);


            string title = className + ".cs";

            if (cmc.IsShowGenCode)
            {
                CodeShow(title, str);
            }
            else
            {
                FileHelper.Write(cmc.OutPut + title, new[] { str });
            }
        }
        public void GenCode(TableInfo tableInfo)
        {
            string tableName = tableInfo.TableName;
            string className = CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.BLLService; ;
            string path = string.Format(@"{0}", "bll.cs.vm");
            var dic = GetNVelocityVars();
            dic.Add("tableInfo", tableInfo);
            dic.Add("modelNameSpace", cmc.ModelNameSpace);
            dic.Add("idalNameSpace", cmc.IDALNameSpace);
            dic.Add("dalNameSpace", cmc.DALNameSpace);
            dic.Add("bllNameSpace", cmc.BLLNameSpace);
            dic.Add("guiPluginName", cmc.PluginName);
            dic.Add("codeRule", cmc.CodeRule);
            string str = nVelocityHelper.GenByTemplate(path, dic);


            string title = className + ".cs";

            if (cmc.IsShowGenCode)
            {
                CodeShow(title, str);
            }
            else
            {
                FileHelper.Write(cmc.OutPut + title, new[] { str });
            }
        }
         
        public GenCsharpBLLAndGUI()
        {
            AddContextMenu();
        }
    }
}
