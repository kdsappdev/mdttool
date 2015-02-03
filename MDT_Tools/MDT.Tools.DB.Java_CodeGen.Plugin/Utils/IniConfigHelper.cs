using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;

using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Java_CodeGen.Plugin.Model;
using MDT.Tools.DB.Java_CodeGen.Plugin.Utils;
using DNCCFrameWork.DataAccess;

namespace MDT.Tools.DB.Java_CodeGen.Plugin.Utils
{
    internal class IniConfigHelper
    {
        static IDbHelper db = new DbFactory(@"data source=control\db.plugin.Java.db", "SqlLiteHelper").IDbHelper;


        public static bool writeDefaultDBInfo(JavaCodeGenConfig config)
        {
            bool status = false;
            try
            {
                string deleteSql = "delete from db_plugin_Java_defaultConfig";
                int result = db.ExecuteNonQuery(deleteSql);
                status = true;
                if (config != null)
                {
                    status = false;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("@DisplayName", config.DisplayName);

                    string insertSql = "insert into db_plugin_Java_defaultConfig(DisplayName) values(@DisplayName)";
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
                string sql = "select DisplayName from db_plugin_Java_defaultConfig";
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
                string sql = "delete from db_plugin_Java_config";
                db.ExecuteNonQuery(sql);
                status = true;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
            return status;
        }

        public static bool WriteDBInfo(JavaCodeGenConfig config, ref string message)
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
                        sql = "delete from db_plugin_Java_config where Id = @Id";
                        dic.Add("@Id", config.Id);
                    }
                    else
                    {
                        dic.Add("@BSPackage", config.BSPackage);
                        dic.Add("@CodeRule", config.CodeRule);
                        dic.Add("@DisplayName", config.DisplayName);
                        dic.Add("@Ibatis", config.Ibatis);
                        
                        dic.Add("@IsShowGenCode", config.IsShowGenCode.ToString());
                        dic.Add("@OutPut", config.OutPut);
                        dic.Add("@TableFilter", config.TableFilter);
                        dic.Add("@WSPackage", config.WSPackage);
                       
                        if (string.IsNullOrEmpty(config.Id))
                        {
                            dic.Add("@Id", Guid.NewGuid().ToString());
                            sql = "insert into db_plugin_Java_config(BSPackage, CodeRule, DisplayName, Ibatis, Id, IsShowGenCode, OutPut, TableFilter, WSPackage)"
                           + " values(@BSPackage, @CodeRule, @DisplayName, @Ibatis, @Id, @IsShowGenCode, @OutPut, @TableFilter, @WSPackage)";
                    
                        }
                        else
                        {
                            dic.Add("@Id", config.Id);
                            sql = "update db_plugin_Java_config set BSPackage=@BSPackage, CodeRule=@CodeRule, DisplayName=@DisplayName, Ibatis=@Ibatis, Id=@Id, IsShowGenCode=@IsShowGenCode, OutPut=@OutPut"
                                + ", TableFilter=@TableFilter, WSPackage=@WSPackage where Id=@Id";
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

        public static IList<JavaCodeGenConfig> ReadDBInfo()
        {
            IList<JavaCodeGenConfig> configList = new List<JavaCodeGenConfig>();
            try
            {

                var dataSet = new DataSet();
                db.Fill("select * from db_plugin_Java_config", dataSet);
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    JavaCodeGenConfig config = new JavaCodeGenConfig();
                    config.BSPackage = row["BSPackage"] + "";
                    config.CodeRule = row["CodeRule"] + "";
                    config.DisplayName = row["DisplayName"] + "";
                    config.Ibatis = row["Ibatis"] + "";
                    config.Id = row["Id"] + "";
                    config.IsShowGenCode = bool.Parse(row["IsShowGenCode"] + "");
                    config.OutPut = row["OutPut"] + "";
                    config.TableFilter = row["TableFilter"] + "";
                    config.WSPackage = row["WSPackage"] + "";
                  

                    configList.Add(config);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return configList;
        }

        public static JavaCodeGenConfig getDefaultObject()
        {
            JavaCodeGenConfig config = new JavaCodeGenConfig();
            string str = ReadDefaultDBInfo();
            IList<JavaCodeGenConfig> configList = ReadDBInfo();
            if (string.IsNullOrEmpty(str))
            {
                config.BSPackage = "";
                config.CodeRule = "";
                config.DisplayName = "";
                config.Ibatis = "";
                config.Id = "";
                config.IsDelete = false;
                config.IsShowGenCode = true;
                config.OutPut = "";
                config.TableFilter = "";
                config.WSPackage = "";
                return config;
            }
            else
            {
                foreach (JavaCodeGenConfig tem in configList)
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
