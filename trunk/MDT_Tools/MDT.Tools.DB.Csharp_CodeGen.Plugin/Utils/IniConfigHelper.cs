using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;

using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Common;
using DNCCFrameWork.DataAccess;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Model;

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils
{
    internal class IniConfigHelper
    {

        static IDbHelper db = new DbFactory(@"data source=control\db.plugin.Csharp.db", "SqlLiteHelper").IDbHelper;


        public static bool writeDefaultDBInfo(CsharpCodeGenConfig config)
        {
            bool status = false;
            try
            { 
                string deleteSql = "delete from db_plugin_Csharp_defaultConfig";
                int result = db.ExecuteNonQuery(deleteSql);
                status = true;
                if (config != null)
                {
                    status = false;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("@DisplayName", config.DisplayName);
                   
                    string insertSql = "insert into db_plugin_Csharp_defaultConfig(DisplayName) values(@DisplayName)";
                    result = db.ExecuteNonQuery(insertSql, dic);
                    status = true;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
            return status;
        }

        public static string ReadDefaultDBInfo()
        {
            string str = "";
            try
            {
                string sql = "select DisplayName from db_plugin_Csharp_defaultConfig";
                str = db.ExecuteScalar(sql) + "";
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return str;
        }

        public static bool deleteDBInfo()
        {
            bool status = false;
            try
            {
                string sql = "delete from db_plugin_Csharp_config";
                db.ExecuteNonQuery(sql);
                status = true;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
            return status;
        }

        public static bool WriteDBInfo(CsharpCodeGenConfig config, ref string message)
        {
            bool status = false;
            if (config != null)
            {
                try
                {
                    var dic = new Dictionary<string, string>();
                    string sql = "";
                    if (config.IsDelete)
                    {
                        sql = "delete from db_plugin_Csharp_config where Id = @Id";
                        dic.Add("@Id", config.Id);
                    }
                    else
                    {
                        dic.Add("@ModelNameSpace", config.ModelNameSpace);
                        dic.Add("@DALNameSpace", config.DALNameSpace);
                        dic.Add("@IDALNameSpace", config.IDALNameSpace);
                        dic.Add("@BLLNameSpace", config.BLLNameSpace);
                        dic.Add("@PluginName", config.PluginName);
                        dic.Add("@OutPut", config.OutPut);
                        dic.Add("@TableFilter", config.TableFilter);
                        dic.Add("@IsShowGenCode", config.IsShowGenCode.ToString());
                        dic.Add("@IsShowComment", config.IsShowComment.ToString());
                        dic.Add("@CodeRule", config.CodeRule);
                        dic.Add("@Ibatis", config.Ibatis);
                        dic.Add("@DALDLLName", config.DALDllName);
                        dic.Add("@BLLDLLName", config.BLLDllName);
                        dic.Add("@DisplayName", config.DisplayName);

                        if (string.IsNullOrEmpty(config.Id))
                        {
                            dic.Add("@Id", Guid.NewGuid().ToString());
                            sql = "insert into db_plugin_Csharp_config(Id, ModelNameSpace, DALNameSpace, IDALNameSpace, BLLNameSpace, PluginName, OutPut, TableFilter"
                           + ", IsShowGenCode, IsShowComment, CodeRule, Ibatis, DALDLLName, BLLDLLName, DisplayName) values(@Id, @ModelNameSpace, @DALNameSpace, @IDALNameSpace, @BLLNameSpace, @PluginName, @OutPut, @TableFilter"
                           + ", @IsShowGenCode, @IsShowComment, @CodeRule, @Ibatis, @DALDLLName, @BLLDLLName, @DisplayName)";
                        }
                        else
                        {
                            dic.Add("@Id", config.Id);
                            sql = "update db_plugin_Csharp_config set ModelNameSpace=@ModelNameSpace, DALNameSpace=@DALNameSpace, IDALNameSpace=@IDALNameSpace, BLLNameSpace=@BLLNameSpace, PluginName=@PluginName, OutPut=@OutPut, TableFilter=@TableFilter"
                                + ", IsShowGenCode=@IsShowGenCode, IsShowComment=@IsShowComment, CodeRule=@CodeRule, Ibatis=@Ibatis, DALDLLName=@DALDLLName, BLLDLLName=@BLLDLLName, DisplayName=@DisplayName where Id=@Id";
                        }                                              
                    }
                    db.ExecuteNonQuery(sql, dic);
                    status = true;           
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    message = ex.Message;
                }
            }
            return status;
        }

        public static IList<CsharpCodeGenConfig> ReadDBInfo()
        {
            IList<CsharpCodeGenConfig> configList = new List<CsharpCodeGenConfig>();
            try
            {

                var dataSet = new DataSet();
                db.Fill("select * from db_plugin_Csharp_config", dataSet);
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    CsharpCodeGenConfig config = new CsharpCodeGenConfig();
                    config.BLLDllName = row["BLLDllName"] + "";
                    config.BLLNameSpace = row["BLLNameSpace"] + "";
                    config.CodeRule = row["CodeRule"] + "";
                    config.DALDllName = row["DALDllName"] + "";
                    config.DALNameSpace = row["DALNameSpace"] + "";
                    config.DisplayName = row["DisplayName"] + "";
                    config.Ibatis = row["Ibatis"] + "";
                    config.IDALNameSpace = row["IDALNameSpace"] + "";
                    config.IsShowComment = bool.Parse(row["IsShowComment"] + "");
                    config.IsShowGenCode = bool.Parse(row["IsShowGenCode"] + "");
                    config.ModelNameSpace = row["ModelNameSpace"] + "";
                    config.OutPut = row["OutPut"] + "";
                    config.PluginName = row["PluginName"] + "";
                    config.TableFilter = row["TableFilter"] + "";
                    config.Id = row["Id"] + "";

                    configList.Add(config);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return configList;
        }

        public static CsharpCodeGenConfig getDefaultObject()
        {
            CsharpCodeGenConfig config = new CsharpCodeGenConfig();
            string str = ReadDefaultDBInfo();
            IList<CsharpCodeGenConfig> configList = ReadDBInfo();
            if (string.IsNullOrEmpty(str))
            {
                config.BLLDllName = "";
                config.BLLNameSpace = "";
                config.CodeRule = "";
                config.DALDllName = "";
                config.DALNameSpace = "";
                config.DisplayName = "";
                config.Ibatis = "";
                config.Id = "";
                config.IDALNameSpace = "";
                config.IsDelete = false;
                config.IsShowComment = false;
                config.IsShowGenCode = true;
                config.ModelNameSpace = "";
                config.OutPut = "";
                config.PluginName = "";
                config.TableFilter = "";
                
                return config;
            }
            else
            {
                foreach (CsharpCodeGenConfig tem in configList)
                {
                    if (!string.IsNullOrEmpty(tem.DisplayName))
                    {
                        if (str.Equals(tem.DisplayName))
                        {
                            config = tem;
                        }
                    }
                }
            }
            return config;
        }
    }
}
