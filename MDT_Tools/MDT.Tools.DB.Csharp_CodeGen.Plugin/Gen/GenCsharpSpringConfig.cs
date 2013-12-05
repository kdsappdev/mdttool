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
       
        private IbatisConfigHelper ibatisConfigHelper = new IbatisConfigHelper();
        
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
 
        public GenCsharpSpringConfig()
        {
            AddContextMenu();
        }
 
     
 

 


    }
}
