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
using MDT.Tools.DB.Common; 

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Gen
{
    /// <summary>
    /// Csharp Model实体类属性生成器
    /// </summary>
    internal class GenCsharpModel : AbstractHandler
    {
       
        public CsharpCodeGenConfig cmc;
 
       
        
        #region 处理
      
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
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
                        setStatusBar(string.Format("正在生成{0}命名空间的Model", cmc.ModelNameSpace));
                        setProgreesEditValue(0);
                        setProgress(0);
                        setProgressMax(drTables.Length);
                    }
                    int j = 0;
                    for (int i = 0; i <tableInfos.Count; i++)
                    {
                       
                        string tableName =tableInfos[i].TableName;
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
                            setStatusBar(string.Format("正在生成{0}命名空间中{1}信息,共{2}个Model，已生成了{3}个Model,过滤了{4}个Model", cmc.ModelNameSpace,
                                CodeGenHelper.GetClassName(tableName,cmc.CodeRule),
                                                       drTables.Length, i - j, j));
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            
                           GenCode(tableInfos[i]);
                        }
                    }

                }

                if (!cmc.IsShowGenCode)
                {
                    setStatusBar(string.Format("{0}命名空间Model生成成功", cmc.ModelNameSpace));
                    openDialog();
                }
            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("{0}命名空间Model生成失败[{1}]", cmc.ModelNameSpace, ex.Message));

            }
            finally
            {
                setEnable(true);
            }


        }

        #endregion


        private readonly NVelocityHelper nVelocityHelper = new NVelocityHelper(FilePathHelper.TemplatesPath);
        public void GenCode(TableInfo tableInfo)
        {
            string tableName = tableInfo.TableName;
            string className = CodeGenHelper.GetClassName(tableName, cmc.CodeRule);
            string path = string.Format(@"{0}", "model.cs.vm");
            var dic = GetNVelocityVars();
            dic.Add("tableInfo",tableInfo);
            dic.Add("modelNameSpace", cmc.ModelNameSpace);
            dic.Add("idalNameSpace", cmc.IDALNameSpace);
            dic.Add("dalNameSpace", cmc.DALNameSpace);
            dic.Add("bllNameSpace", cmc.BLLNameSpace);
            dic.Add("guiPluginName", cmc.PluginName);
            dic.Add("codeRule",cmc.CodeRule);
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
        
        public GenCsharpModel()
        {
            AddContextMenu();
        }
    }
}
