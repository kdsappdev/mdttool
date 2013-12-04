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
        
        private IbatisConfigHelper ibatisConfigHelper = new IbatisConfigHelper();
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                CodeLanguage = "Java";
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
                        setStatusBar(string.Format("正在生成{0}包路径ws层代码", cmc.WSPackage));
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
                            setStatusBar(string.Format("正在生成{0}包路径ws中{1}的代码,共{2}个代码，已生成了{3}个代码,过滤了{4}个代码", cmc.WSPackage,
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
                    setStatusBar(string.Format("{0}包路径ws代码生成成功", cmc.WSPackage));
                    openDialog();
                }
            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("{0}包路径ws代码生成失败[{1}]", cmc.WSPackage, ex.Message));

            }
            finally
            {
                setEnable(true);
            }
        }
        

        public string GenCodeInterface(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string className = drTable["name"] as string;
            className = "I" + (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(className) : CodeGenHelper.StrFirstToUpperRemoveUnderline(className)) + CodeGenRuleHelper.IWSService;

            string modelClass = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(drTable["name"] as string) : CodeGenHelper.StrFirstToUpperRemoveUnderline(className));

            #region 引入包路径
            sb.AppendFormat("package {0};", cmc.WSPackage).AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("import javax.jws.WebService;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #region 类名
            var tablecomments = drTable["comments"] as string;

            sb.AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat(" *").Append(string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments) + CodeGenRuleHelper.IWSSummary).
                AppendFormat("\r\n");
            sb.AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat(" *").AppendFormat("{0:0000}.{1:00}.{2:00}: 创建. {3} <br/>", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PluginName).AppendFormat("\r\n");
            sb.AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("@WebService").AppendFormat("\r\n");
            sb.AppendFormat("public interface {0}", className).Append(" {").AppendFormat("\r\n");

            #region 接口



            #region select

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("根据location 查询出{0}集合对象", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param location实体[字符串压缩字节码]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}AtsMsg[字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] select(byte[] location);").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            #endregion

            #region selectByShortName

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("根据shortName,location 查询出{0}对象", modelClass).AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param shortName短名称[字符串压缩字节码]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param location实体[字符串压缩字节码]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] selectByShortName(byte[] shortName, byte[] location);").AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            #endregion

            #region insert

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("增加{0}方法", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param {0}{1}[{2}]", CodeGenHelper.StrFirstToLower(modelClass), string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments), "MsgHelper.Serialize序列化字符串压缩字节码").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] insert(byte[] {0});", CodeGenHelper.StrFirstToLower(modelClass)).AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            #endregion

            #region delete
            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("删除{0}方法", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param {0}{1}[{2}]", CodeGenHelper.StrFirstToLower(modelClass), string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments), "MsgHelper.Serialize序列化字符串压缩字节码").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] delete(byte[] {0});", CodeGenHelper.StrFirstToLower(modelClass)).AppendFormat("\r\n");

            sb.AppendFormat("\r\n");
            #endregion

            #region update

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("更新{0}方法", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param {0}{1}[{2}]", CodeGenHelper.StrFirstToLower(modelClass), string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments), "MsgHelper.Serialize序列化字符串压缩字节码").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] update(byte[] {0});", CodeGenHelper.StrFirstToLower(modelClass)).AppendFormat("\r\n");

            #endregion

            #endregion

            sb.Append("}").AppendFormat("\r\n");
            #endregion


            string title = className + ".java";

            if (cmc.IsShowGenCode)
            {
                CodeShow(title, sb.ToString());
            }
            else
            {

                FileHelper.Write(cmc.OutPut + title, new[] { sb.ToString() }, Encoding.GetEncoding("GBK"));
            }

            return sb.ToString();
        }

        public string GenCode(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            string className = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName)) + CodeGenRuleHelper.WSService;
            string bsInterfaceName = "I" + (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName)) + CodeGenRuleHelper.IBSServer;
            string wsInterfaceName = "I" + (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName)) + CodeGenRuleHelper.IWSService;
            
            string modelClass = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName));


            #region 引入包路径
            sb.AppendFormat("package {0}{1};", cmc.WSPackage, ".impl").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("import java.util.List;").AppendFormat("\r\n");
            sb.AppendFormat("import org.apache.log4j.Logger;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("import ats.common.model.po.{0};", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("import ats.message2.Exception.SerializerException;").AppendFormat("\r\n");
            sb.AppendFormat("import ats.message2.MsgHelper;").AppendFormat("\r\n");
            sb.AppendFormat("import ats.message2.CharsetConvertType;").AppendFormat("\r\n");
            sb.AppendFormat("import ats.pingo.integration.endpoint.ws.basewebservice.BaseWebService;").AppendFormat("\r\n");
            sb.AppendFormat("import ats.yukon.datamanager.bs.{0};", bsInterfaceName).AppendFormat("\r\n");
            sb.AppendFormat("import ats.yukon.integration.endpoint.ws.datamanager.{0};", wsInterfaceName).AppendFormat("\r\n");

            sb.AppendFormat("import ats.foundation.utils.util.ZipUtils;").AppendFormat("\r\n");
            sb.AppendFormat("import ats.foundation.utils.util.CharsetConvert;").AppendFormat("\r\n");
            sb.AppendFormat("import ats.foundation.utils.exception.CodeException;").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #region 类名
            var tablecomments = drTable["comments"] as string;

            sb.AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat(" *").Append(string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments) + CodeGenRuleHelper.WSSummary).
                AppendFormat("\r\n");
            sb.AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat(" *").AppendFormat("{0:0000}.{1:00}.{2:00}: 创建. {3} <br/>", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PluginName).AppendFormat("\r\n");
            sb.AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("").AppendFormat("public class {0} extends BaseWebService implements {1}", className, wsInterfaceName).Append(" {").AppendFormat("\r\n");

            #region 实现

            #region 字段

            string bsInterfaceNameF = CodeGenHelper.StrFirstAndSecondToLower(bsInterfaceName);
            sb.AppendFormat("\t").AppendFormat("private Logger logger = Logger.getLogger(this.getClass().getName());").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("private {0} {1};", bsInterfaceName, bsInterfaceNameF).AppendFormat("\r\n");
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("public void set{1}({0} {2})", bsInterfaceName, CodeGenHelper.StrFirstToUpper(bsInterfaceNameF), bsInterfaceNameF).Append(" {").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("this.{0} = {0};", bsInterfaceNameF).AppendFormat("\r\n");
            sb.AppendFormat("\t").Append("}").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #region select

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("根据location 查询出{0}集合对象", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param location实体[字符串压缩字节码]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}AtsMsg[字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("@Override").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] select(byte[] location)").Append(" {").AppendFormat("\r\n");

            #region 方法体
            sb.AppendFormat("\t\t").AppendFormat("return null;").AppendFormat("\r\n");
            #endregion

            sb.AppendFormat("\t").Append("}").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #region selectByShortName

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("根据shortName,location 查询出{0}对象", modelClass).AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param shortName短名称[字符串压缩字节码]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param location实体[字符串压缩字节码]").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("@Override").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] selectByShortName(byte[] shortName, byte[] location)").Append(" {").AppendFormat("\r\n");


            #region 方法体
            sb.AppendFormat("\t\t").AppendFormat("return null;").AppendFormat("\r\n");
            #endregion

            sb.AppendFormat("\t").Append("}").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #region insert

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("增加{0}方法", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param {0}{1}[{2}]", CodeGenHelper.StrFirstToLower(modelClass), string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments), "MsgHelper.Serialize序列化字符串压缩字节码").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("@Override").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] insert(byte[] {0})", CodeGenHelper.StrFirstToLower(modelClass)).Append(" {").AppendFormat("\r\n");

            #region 方法体
            sb.AppendFormat("\t\t").AppendFormat("return null;").AppendFormat("\r\n");
            #endregion

            sb.AppendFormat("\t").Append("}").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #region delete
            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("删除{0}方法", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param {0}{1}[{2}]", CodeGenHelper.StrFirstToLower(modelClass), string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments), "MsgHelper.Serialize序列化字符串压缩字节码").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("@Override").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] delete(byte[] {0}) ", CodeGenHelper.StrFirstToLower(modelClass)).Append(" {").AppendFormat("\r\n");

            #region 方法体
            sb.AppendFormat("\t\t").AppendFormat("return null;").AppendFormat("\r\n");
            #endregion

            sb.AppendFormat("\t").Append("}").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #region update

            sb.AppendFormat("\t").AppendFormat("/**").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("更新{0}方法", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" *").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @param {0}{1}[{2}]", CodeGenHelper.StrFirstToLower(modelClass), string.IsNullOrEmpty(tablecomments) ? className : EncodingHelper.ConvertEncoder(OriginalEncoding,
                                                                                              TargetEncoding,
                                                                                              tablecomments), "MsgHelper.Serialize序列化字符串压缩字节码").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" * @return {0}[AtsMsg字符串压缩字节码]", modelClass).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat(" */").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("@Override").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("public byte[] update(byte[] {0})", CodeGenHelper.StrFirstToLower(modelClass)).Append(" {").AppendFormat("\r\n");

            #region 方法体
            sb.AppendFormat("\t\t").AppendFormat("return null;").AppendFormat("\r\n");
            #endregion

            sb.AppendFormat("\t").Append("}").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            #endregion

            #endregion

            sb.Append("}").AppendFormat("\r\n");
            #endregion


            string title = className + ".java";

            if (cmc.IsShowGenCode)
            {
                CodeShow(title, sb.ToString());
            }
            else
            {

                FileHelper.Write(cmc.OutPut + title, new[] { sb.ToString() }, Encoding.GetEncoding("GBK"));
            }

            return sb.ToString();
        }

       
        public GenJavaWS()
        {
            AddContextMenu();
        }
     }
}
