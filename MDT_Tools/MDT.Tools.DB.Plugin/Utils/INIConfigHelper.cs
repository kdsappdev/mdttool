using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Common;
using MDT.Tools.DB.Plugin.Model;
using DNCCFrameWork.DataAccess;
namespace MDT.Tools.DB.Plugin.Utils
{
    internal static class IniConfigHelper
    {




        static IDbHelper db = new DbFactory(@"data source=control\db.plugin.db", "SqlLiteHelper").IDbHelper;

        public static bool WriteDefaultDBInfo(DbConfigInfo dbConfigInfo)
        {
            bool status = false;
            if (dbConfigInfo != null)
            {
                try
                {

                    var dic = new Dictionary<string, string>();
                    dic.Add("@dbId", dbConfigInfo.DbId);
                    string delSql = "delete from db_plugin_defaultSelect";
                    int result = db.ExecuteNonQuery(delSql);
                    string insertSql = "INSERT INTO db_plugin_defaultSelect(dbId) VALUES(@dbId)";
                    result = db.ExecuteNonQuery(insertSql, dic);
                    status = true;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);

                }
            }
            return status;
        }
        public static string ReadDefaultDBInfo()
        {
            string str = "";

            try
            {
                string sql = "select dbId from db_plugin_defaultSelect";
                str = db.ExecuteScalar(sql) + "";

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);

            }

            return str;
        }
        public static bool WriteDBInfo(DbConfigInfo dbConfigInfo, ref string message)
        {
            bool status = false;
            if (dbConfigInfo != null)
            {
                try
                {
                    var dic = new Dictionary<string, string>();
                    string sql = "";
                    if (dbConfigInfo.IsDelete)
                    {
                        sql = "delete from db_plugin_config where dbId=@dbId";
                        dic.Add("@dbId", dbConfigInfo.DbId);
                    }
                    else
                    {

                        dic.Add("@dbName", dbConfigInfo.DbConfigName);
                        dic.Add("@dbConnectionString", dbConfigInfo.ConnectionString);
                        dic.Add("@dbType", dbConfigInfo.DbType);
                        dic.Add("@dbEncoder", dbConfigInfo.DbEncoder);

                        if (string.IsNullOrEmpty(dbConfigInfo.DbId))
                        {
                            dic.Add("@dbId", Guid.NewGuid().ToString());
                            sql = "INSERT INTO db_plugin_config(dbId, dbName, dbConnectionString, dbType, dbEncoder) VALUES(@dbId, @dbName, @dbConnectionString, @dbType, @dbEncoder)";
                        }
                        else
                        {
                            dic.Add("@dbId", dbConfigInfo.DbId);
                            sql = "update db_plugin_config set dbName=@dbName,dbConnectionString=@dbConnectionString,dbType=@dbType,dbEncoder=@dbEncoder where dbId=@dbId";
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
        public static IList<DbConfigInfo> ReadDBInfo()
        {
            IList<DbConfigInfo> dbConfigList = new List<DbConfigInfo>();
            try
            {

                var dataSet = new DataSet();
                db.Fill("select * from db_plugin_config", dataSet);
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var dbConfigInfo = new DbConfigInfo();
                    dbConfigInfo.DbType = row["dbtype"] + "";
                    dbConfigInfo.DbConfigName = row["dbName"] + "";
                    dbConfigInfo.ConnectionString = row["dbConnectionString"] + "";
                    dbConfigInfo.DbEncoder = row["dbEncoder"] + "";
                    dbConfigInfo.DbId = row["dbId"] + "";
                    dbConfigList.Add(dbConfigInfo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dbConfigList;
        }
    }
}
