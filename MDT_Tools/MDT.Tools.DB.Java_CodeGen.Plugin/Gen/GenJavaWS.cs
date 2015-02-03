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
    /// Java WS生成器
    /// </summary>
    internal class GenJavaWS : AbstractHandler
    {
        
        public JavaCodeGenConfig cmc;
        
       public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
                CodeLanguage = "Java";
                //SaveFileEncoding = Encoding.GetEncoding("GBK");
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
                        setStatusBar(string.Format("正在生成{0}包路径ws层代码", cmc.WSPackage));
                        LogHelper.Info("Generating " + cmc.WSPackage + " package path ws layer code.");
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
                            setStatusBar(string.Format("正在生成{0}包路径ws中{1}的代码,共{2}个代码，已生成了{3}个代码,过滤了{4}个代码", cmc.WSPackage,
                                                       CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.WSService,
                                                       drTables.Length, i - j, j));
                            LogHelper.Info("Generatig in " + cmc.WSPackage + " package path ws " + CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.WSService + "'s code,"
                                + drTables.Length + " of codes," + (i - j) + " of code was generated," + j + " of code was filtered.");
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
                    setStatusBar(string.Format("{0}包路径ws代码生成成功", cmc.WSPackage));
                    LogHelper.Info(cmc.WSPackage + " package path ws layer code has generated success.");
                    openDialog();
                }
            }
            catch (Exception ex)
            {
              
                setStatusBar(string.Format("{0}包路径ws代码生成失败[{1}]", cmc.WSPackage, ex.Message));

                LogHelper.Info(cmc.WSPackage + " package path ws layer code generate fail " + ex.Message);
               
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
           string className = "I" + CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.IWSService;
           string path = string.Format(@"{0}", "iws.java.vm");
           var dic = GetNVelocityVars();
           dic.Add("tableInfo", tableInfo);
           dic.Add("bsPackage", cmc.BSPackage);
           dic.Add("wsPackage", cmc.WSPackage);
           dic.Add("codeRule", cmc.CodeRule);
           string str = nVelocityHelper.GenByTemplate(path, dic);


           string title = className + ".java";

           if (cmc.IsShowGenCode)
           {
               CodeShow(title, str);
           }
           else
           {

               FileHelper.Write(cmc.OutPut + title, new[] { str }, SaveFileEncoding);
           }
       }

       public void GenCode(TableInfo tableInfo)
       {
           string tableName = tableInfo.TableName;
           string className = CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.WSService;
           string path = string.Format(@"{0}", "wsimpl.java.vm");
           var dic = GetNVelocityVars();
           dic.Add("tableInfo", tableInfo);
           dic.Add("bsPackage", cmc.BSPackage);
           dic.Add("wsPackage", cmc.WSPackage);
           dic.Add("codeRule", cmc.CodeRule);
           string str = nVelocityHelper.GenByTemplate(path, dic);


           string title = className + ".java";

           if (cmc.IsShowGenCode)
           {
               CodeShow(title, str);
           }
           else
           {

               FileHelper.Write(cmc.OutPut + title, new[] { str }, SaveFileEncoding);
           }
       }
        public GenJavaWS()
        {
            AddContextMenu();
        }
     }
}
