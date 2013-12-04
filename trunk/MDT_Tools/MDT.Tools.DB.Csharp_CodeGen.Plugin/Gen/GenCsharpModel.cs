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
 
        private IbatisConfigHelper ibatisConfigHelper = new IbatisConfigHelper();

        #region 处理
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                OutPut = cmc.OutPut;
                setEnable(false);
                setStatusBar("");
                string[] strs = null;
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
                        setStatusBar(string.Format("正在生成{0}命名空间的Model", cmc.ModelNameSpace));
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
                            setStatusBar(string.Format("正在生成{0}命名空间中{1}信息,共{2}个Model，已生成了{3}个Model,过滤了{4}个Model", cmc.ModelNameSpace,
                                cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(className) : CodeGenHelper.StrFirstToUpperRemoveUnderline(className),
                                                       drTables.Length, i - j, j));
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");
                            strs[i] = GenCode(drTable, drTableColumns);
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

       

    

        public string GenCode(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();

            #region 引入命名空间
            sb.AppendFormat("using System;").AppendFormat("\r\n");
            //sb.AppendFormat("using System.Collections.Generic;").AppendFormat("\r\n");
            //sb.AppendFormat("using System.Text;").AppendFormat("\r\n");
            //sb.AppendFormat("using System.Xml.Serialization;").AppendFormat("\r\n");
            #endregion

            #region 命名空间
            sb.AppendFormat("namespace {0}", cmc.ModelNameSpace).AppendFormat("\r\n");
            sb.Append("{").AppendFormat("\r\n");

            #region 类名
            string className = drTable["name"] as string;
            className = cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(className) : CodeGenHelper.StrFirstToUpperRemoveUnderline(className);
            var tablecomments = drTable["comments"] as string;

            sb.AppendFormat("\t").AppendFormat("/// <summary>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// ").Append(string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments)).
                AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// </summary>").AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("/// <remarks>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// ").AppendFormat("{0:0000}.{1:00}.{2:00}: 创建. {3} <br/>", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PluginName).
                AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("/// </remarks>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("[Serializable]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public class {0}", className).AppendFormat("\r\n");
            sb.Append("\t{").AppendFormat("\r\n");

            #region 字段
            sb.AppendFormat("\t\t").AppendFormat("#region 字段").AppendFormat("\r\n").AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                string dataType = "string";
                string nullAble = dr["NULLABLE"] + "";
                dataType = DataTypeMappingHelper.GetCSharpDataTypeByDbType(dbType, dr["DATA_TYPE"] + "", dr["DATA_SCALE"] + "", dr["DATA_LENGTH"] + "", "Y".Equals(nullAble));
                string columnName = dr["COLUMN_NAME"] as string;
                string fieldName = CodeGenHelper.StrFieldWith_(columnName);
                string defaultValue = dr["DATA_DEFAULT"] as string;


                string comments = dr["COMMENTS"] as string;
                sb.AppendFormat("\t\t").AppendFormat("private {0} {1}", dataType, fieldName);
                if (nullAble.Equals("N") || !string.IsNullOrEmpty(defaultValue))
                {
                    defaultValue = CodeGenHelper.GetDefaultValueByDataType(dataType, defaultValue);
                    sb.AppendFormat(" = {0}", defaultValue);
                }
                sb.AppendFormat(";");
                if (!string.IsNullOrEmpty(comments))
                {
                    sb.AppendFormat("//{0}", EncodingHelper.ConvertEncoder(OriginalEncoding, TargetEncoding, comments));
                }
                sb.AppendFormat("\r\n");
            }
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("#endregion").AppendFormat("\r\n");
            #endregion
            sb.AppendFormat("\r\n");

            #region 属性
            sb.AppendFormat("\t\t").AppendFormat("#region 属性").AppendFormat("\r\n").AppendFormat("\r\n");
            foreach (DataRow dr in drTableColumns)
            {
                var dataType = "string";
                string nullAble = dr["NULLABLE"] as string;
                dataType = DataTypeMappingHelper.GetCSharpDataTypeByDbType(dbType, dr["DATA_TYPE"] + "", dr["DATA_SCALE"] + "", dr["DATA_LENGTH"] + "", "Y".Equals(nullAble));
                string comments = dr["COMMENTS"] as string;
                string columnName = dr["COLUMN_NAME"] as string;
                string fieldName = CodeGenHelper.StrFieldWith_(columnName);
                string propertyName = CodeGenHelper.StrProperty(columnName);
                if (!string.IsNullOrEmpty(comments))
                {
                    sb.AppendFormat("\t\t").AppendFormat("/// <summary>").AppendFormat("\r\n");
                    sb.AppendFormat("\t\t").AppendFormat("/// ").Append(EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                                      TargetEncoding,
                                                                                                      comments)).
                        AppendFormat("\r\n");
                    sb.AppendFormat("\t\t").AppendFormat("/// </summary>").AppendFormat("\r\n");
                }
                sb.AppendFormat("\t\t").AppendFormat("public {0} {1}", dataType, propertyName).AppendFormat("\r\n");
                sb.AppendFormat("\t\t").Append("{").AppendFormat("\r\n");
                sb.AppendFormat("\t\t\t").AppendFormat("get ").Append("{ ").AppendFormat("return {0};", fieldName).Append(" }").AppendFormat("\r\n");

                sb.AppendFormat("\t\t\t").AppendFormat("set ").Append("{ ").AppendFormat("{0} = value;", fieldName).Append(" }").AppendFormat("\r\n");
                sb.AppendFormat("\t\t").Append("}").AppendFormat("\r\n");
                sb.AppendFormat("\r\n");
            }
            sb.AppendFormat("\t\t").AppendFormat("#endregion").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
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

        
        public GenCsharpModel()
        {
            AddContextMenu();
        }
    }
}
