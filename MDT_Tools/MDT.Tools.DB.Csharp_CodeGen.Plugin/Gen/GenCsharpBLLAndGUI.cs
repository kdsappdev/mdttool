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
        
        private IbatisConfigHelper ibatisConfigHelper = new IbatisConfigHelper();
      

        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                OutPut = cmc.OutPut;
                setEnable(false);
                string[] strs = null;
                setStatusBar("");
                if (drTables != null && dsTableColumns != null)
                {
                    if (cmc.CodeRule == CodeGenRuleHelper.Ibatis)
                    {
                        ibatisConfigHelper.ReadConfig(cmc.Ibatis);
                    }
                    strs = new string[drTables.Length];
                    if (!cmc.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(cmc.OutPut);
                        setStatusBar(string.Format("正在生成{0}命名空间的BLL&GUI", cmc.BLLNameSpace));
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
                            setStatusBar(string.Format("正在生成{0}命名空间中{1}代码,共{2}个代码，已生成了{3}个代码,过滤了{4}个代码", cmc.BLLNameSpace,
                                                       cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(className) : CodeGenHelper.StrFirstToUpperRemoveUnderline(className),
                                                       drTables.Length, i - j, j));
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");
                            strs[i] = GenCodeInterface(drTable, drTableColumns);
                            strs[i] = GenCode(drTable, drTableColumns);
                        }
                    }

                }

                if (!cmc.IsShowGenCode)
                {
                    setStatusBar(string.Format("{0}命名空间代码生成成功", cmc.BLLNameSpace));
                    openDialog();
                }
            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("{0}命名空间代码生成失败[{1}]", cmc.BLLNameSpace, ex.Message));

            }
            finally
            {
                setEnable(true);
            }

        }

       

        public string GenCodeInterface(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();


            #region 引入命名空间
            sb.AppendFormat("using System;").AppendFormat("\r\n");
            sb.AppendFormat("using System.Collections.Generic;").AppendFormat("\r\n");
            sb.AppendFormat("using System.Text;").AppendFormat("\r\n");
            sb.AppendFormat("using Ats.Foundation.Message;").AppendFormat("\r\n");
             sb.AppendFormat("using Ats.YuKon.Common.Core;").AppendFormat("\r\n");
            sb.AppendFormat("using {0};",cmc.DALNameSpace).AppendFormat("\r\n");
            sb.AppendFormat("using {0};", cmc.ModelNameSpace).AppendFormat("\r\n");
            #endregion

            #region 命名空间
            sb.AppendFormat("namespace {0}", cmc.BLLNameSpace).AppendFormat("\r\n");
            sb.Append("{").AppendFormat("\r\n");

            #region 类名
            
            string className = "I" +cmc.PluginName+ CodeGenRuleHelper.BLLService;
          

            sb.AppendFormat("\t").AppendFormat("/// <summary>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// ").Append(cmc.PluginName+ CodeGenRuleHelper.IBLLServerSummary).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// </summary>").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("/// <remarks>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// ").AppendFormat("{0:0000}.{1:00}.{2:00}: 创建. {3} <br/>", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PluginName).
                AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// </remarks>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public interface {0}", className).AppendFormat(": IBaseBLLService").AppendFormat("\r\n");
            sb.Append("\t{").AppendFormat("\r\n");

            sb.Append("\t}").AppendFormat("\r\n");
            #endregion

            sb.Append("}");
            #endregion

            string title = className + ".cs";

            if (cmc.IsShowGenCode)
            {
                CodeShow(title, sb.ToString());
            }
            else
            {
                FileHelper.Write(cmc.OutPut + title, new[] { sb.ToString() });
            }

            return sb.ToString();
        }

        public string GenCode(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();


            #region 引入命名空间
            sb.AppendFormat("using System;").AppendFormat("\r\n");
            sb.AppendFormat("using System.Collections.Generic;").AppendFormat("\r\n");
            sb.AppendFormat("using System.Text;").AppendFormat("\r\n");
            sb.AppendFormat("using Ats.Foundation.Message;").AppendFormat("\r\n");
            sb.AppendFormat("using Ats.YuKon.Common;").AppendFormat("\r\n");
            sb.AppendFormat("using Ats.YuKon.Common.Core;").AppendFormat("\r\n");
            sb.AppendFormat("using {0};",cmc.IDALNameSpace).AppendFormat("\r\n");
            sb.AppendFormat("using {0};", cmc.ModelNameSpace).AppendFormat("\r\n");
            #endregion

            #region 命名空间
            sb.AppendFormat("namespace {0}", cmc.BLLNameSpace).AppendFormat("\r\n");
            sb.Append("{").AppendFormat("\r\n");

            #region 类名
             
            string className = cmc.PluginName + CodeGenRuleHelper.BLLService;
              string tableName = drTable["name"] as string;
            string Dal = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName)) + CodeGenRuleHelper.DALServer;
     

            sb.AppendFormat("\t").AppendFormat("/// <summary>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// ").Append(cmc.PluginName+ CodeGenRuleHelper.BLLServerSummary).
                AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// </summary>").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("/// <remarks>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// ").AppendFormat("{0:0000}.{1:00}.{2:00}: 创建. {3} <br/>", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PluginName).
                AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// </remarks>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public class {0}", className).AppendFormat(": BaseBLLService,I{0}", className).AppendFormat("\r\n");
            sb.Append("\t{").AppendFormat("\r\n");
    
            #region 属性

            sb.AppendFormat("\t\t#region 属性").AppendFormat("\r\n");
            sb.Append("\t\t").AppendFormat("private I{0} {1};", Dal,CodeGenHelper.StrFirstToLower(Dal)).AppendFormat("\r\n");
            sb.Append("\t\t").AppendFormat("public I{0} I{0}", Dal).AppendFormat("\r\n");
            sb.Append("\t\t").Append("{").AppendFormat("\r\n");
            sb.Append("\t\t\t").AppendFormat("get ").Append("{ return ").AppendFormat("{0};", CodeGenHelper.StrFirstToLower(Dal)).Append("}").AppendFormat("\r\n");
            sb.Append("\t\t\t").AppendFormat("set ").Append("{ ").AppendFormat("{0} = value;", CodeGenHelper.StrFirstToLower(Dal)).Append("}").AppendFormat("\r\n");
            sb.Append("\t\t").Append("}").AppendFormat("\r\n");
            sb.AppendFormat("\t\t#endregion").AppendFormat("\r\n");
            #endregion

            sb.Append("\t}").AppendFormat("\r\n");
            #endregion


            sb.Append("}");
            #endregion

            string title = className + ".cs";

            if (cmc.IsShowGenCode)
            {
                CodeShow(title, sb.ToString());
            }
            else
            {
                FileHelper.Write(cmc.OutPut + title, new[] { sb.ToString() });
            }

            return sb.ToString();
        }

        
        public GenCsharpBLLAndGUI()
        {
            AddContextMenu();
        }
    }
}
