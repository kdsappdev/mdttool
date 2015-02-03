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
    /// Java BS生成器
    /// </summary>
    internal class GenJavaBS : AbstractHandler
    {
        
        public JavaCodeGenConfig cmc;
        
        
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
                //SaveFileEncoding = Encoding.GetEncoding("GBK");
                CodeLanguage = "Java";
                OutPut = cmc.OutPut;
                setEnable(false);
                setStatusBar("");
                string[] strs = null;

                if (drTables != null && dsTableColumns != null)
                {
                    if (cmc.CodeRule == CodeGenRuleHelper.Ibatis)
                    {
                        CodeGenHelper.ReadConfig(cmc.Ibatis);
                    }
                    if (!cmc.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(cmc.OutPut);
                        setStatusBar(string.Format("正在生成{0}包路径bs层代码", cmc.BSPackage));
                        LogHelper.Info("Generating " + cmc.BSPackage + " package path bs layer bs code.");
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
                            setStatusBar(string.Format("正在生成{0}包路径bs中{1}的代码,共{2}个代码，已生成了{3}个代码,过滤了{4}个代码", cmc.BSPackage,
                                                       CodeGenHelper.GetClassName(tableName, cmc.CodeRule)+CodeGenRuleHelper.BSServer,
                                                       drTables.Length, i - j, j));
                            LogHelper.Info("Generating in " + cmc.BSPackage + " package path bs " + CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.BSServer + " of code,"
                                + drTables.Length + " of codes," + (i - j) + "of code has generated," + j + " of code was filtered.");
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
                    setStatusBar(string.Format("{0}包路径bs代码生成成功", cmc.BSPackage));
                    LogHelper.Info(cmc.BSPackage + " package path bs code has generated success.");
                    openDialog();
                }
            }
            catch (Exception ex)
            {
               
                setStatusBar(string.Format("{0}包路径bs代码生成失败[{1}]", cmc.BSPackage, ex.Message));

                LogHelper.Error(cmc.BSPackage + " package path bs code generated fail " + ex.Message);
               
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
            string className = "I" + CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.IBSServer;
            string path = string.Format(@"{0}", "ibs.java.vm");
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
            string className =  CodeGenHelper.GetClassName(tableName, cmc.CodeRule) + CodeGenRuleHelper.BSServer;
            string path = string.Format(@"{0}", "bsimpl.java.vm");
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

        
        public GenJavaBS()
        {
            AddContextMenu();
        }
    }
}
