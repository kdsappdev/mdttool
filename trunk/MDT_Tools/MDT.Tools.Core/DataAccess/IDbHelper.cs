using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
namespace DNCCFrameWork.DataAccess
{      
    public interface IDbHelper
    {

        #region DbProviderFactory 数据提供工厂
        DbProviderFactory Instance();
        #endregion

        #region ADO.net对象
        DbConnectionStringBuilder GetDbConnectionStringBuilder();
        DbCommand GetDbCommand();
        DbConnection GetDbConnection();
        DbTransaction GetDbTransaction(DbConnection connection);
        DbDataAdapter GetDataAdapter();
        //DbDataReader GetDataReader();
        DbParameter GetParameter();
        #endregion

        #region 参数
        void attachParameters(DbCommand command, DbParameter[] commandParameters);
        DbParameter[] createParameters(Dictionary<string, string> dic);
        void PrepareCommand(CommandType commandType, string commandText, DbParameter[] commandParameters, bool EnableTransaction);
        #endregion

        #region ExecuteNonQuery命令
        int ExecuteNonQuery(string commandText);

        int ExecuteNonQuery(CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        int ExecuteNonQuery(string commandText, Dictionary<string, string> commandParameters);

        int ExecuteNonQuery(bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        int ExecuteNonQuery(bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters);

        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        int ExecuteNonQuery(string connectionString, bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        int ExecuteNonQuery(string connectionString, bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters);

        #endregion

        #region ExecuteScalar命令
        object ExecuteScalar(CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        object ExecuteScalar(string commandText);

        object ExecuteScalar(string commandText, Dictionary<string, string> commandParameters);

        object ExecuteScalar(bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        object ExecuteScalar(bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters);

        object ExecuteScalar(string connectionString, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        object ExecuteScalar(string connectionString, bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        object ExecuteScalar(string connectionString, bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters);

        #endregion

        #region ExecuteReader命令
        DbDataReader ExecuteReader(CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        DbDataReader ExecuteReader(string commandText);

        DbDataReader ExecuteReader(string commandText, Dictionary<string, string> commandParameters);

        DbDataReader ExecuteReader(bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        DbDataReader ExecuteReader(bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters);

        DbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        DbDataReader ExecuteReader(string connectionString, bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters);

        DbDataReader ExecuteReader(string connectionString, bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters);

        #endregion

        #region Fill命令
        void Fill(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames,
       Dictionary<string, string> commandParameter);

        void Fill(string commandText, DataSet dataSet);

        void Fill(string commandText, DataSet dataSet, string[] tableNames,
       Dictionary<string, string> commandParameter);

        void Fill(string commandText, DataSet dataSet, string[] tableNames);

        #endregion

        #region Update命令
        void Update(DbCommand insertCommand, DbCommand deleteCommand, DbCommand updateCommand, DataSet dataSet, string tableName);


        void Update(string updateCommandText, Dictionary<string, string> commandParameters);

        #endregion

        #region 数据库常用操作
        //获得数据库当前时间
        String GetDBNow();
        int GetMaxID(string columnName, string tableName);
        #endregion

        #region 测试数据库
        bool TestConnection();
        #endregion
    }
}
