using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
namespace DNCCFrameWork.DataAccess
{
    public abstract class BaseDbHelper : IDbHelper
    {
        #region 字段
        private DbConnectionStringBuilder _connectionStringBuilder = null;
        private DbConnection _connection = null;
        private DbCommand _command = null;
        private DbTransaction _transaction = null;
        private DbDataAdapter _dataAdapter = null;
        //private DbDataReader _dataReader = null;
        private DbParameter _parameter = null;
        #endregion

        #region 属性
        public DbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = GetDbConnection();
                }
                if (_connection.State == ConnectionState.Closed)//确保在没有打开的情况下，可以进行赋值
                {
                    _connection.ConnectionString = ConnectionString;//创建连接类接受字符串
                }
                return _connection;
            }
        }
        public DbCommand Command
        {
            get
            {
                if (_command == null)
                {
                    _command = GetDbCommand();
                }
                if (_command.Connection == null)//确保只连接一次
                {
                    _command.Connection = Connection;
                }
                return _command;
            }
        }
        public DbTransaction Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = Connection.BeginTransaction();
                }
                return _transaction;
            }
        }
        public DbDataAdapter DataAdapter
        {
            get
            {
                if (_dataAdapter == null)
                {
                    _dataAdapter = GetDataAdapter();
                }
                return _dataAdapter;
            }
        }
        public DbParameter Parameter
        {
            get
            {
                if (_parameter == null)
                {
                    _parameter = GetParameter();
                }
                return _parameter;
            }
        }
        public DbConnectionStringBuilder ConnectionStringBuilder
        {
            get
            {
                if (_connectionStringBuilder == null)
                {
                    _connectionStringBuilder = GetDbConnectionStringBuilder();
                }
                return _connectionStringBuilder;
            }
        }
        public string ConnectionString
        {
            get
            {
                return ConnectionStringBuilder.ConnectionString;
            }
            set
            {
                try
                {
                    ConnectionStringBuilder.ConnectionString = value;
                }
                catch (ArgumentException ex)
                {
                    throw ex;
                }
            }
        }
        #endregion


        #region DbProviderFactory 数据提供工
        public abstract DbProviderFactory Instance();

        #endregion

        #region ADO.net对象
        //连接字符串构造器
        public DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            DbConnectionStringBuilder csb = Instance().CreateConnectionStringBuilder();
            return csb;
        }
        public DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString)
        {
            DbConnectionStringBuilder csb = GetDbConnectionStringBuilder();
            csb.ConnectionString = connectionString;
            return csb;
        }
        //连接
        public DbConnection GetDbConnection()
        {
            DbConnection con = Instance().CreateConnection();
            return con;
        }
        public DbConnection GetDbConnection(string connectionString)
        {
            DbConnection con = GetDbConnection();
            con.ConnectionString = GetDbConnectionStringBuilder(connectionString).ConnectionString;//用字符串构造器来初始化字符串，避免Sql注入       
            return con;
        }
        //命令
        public DbCommand GetDbCommand()
        {
            DbCommand com = Instance().CreateCommand();
            return com;
        }
        public DbCommand GetDbCommand(string cmdText)
        {
            DbCommand com = GetDbCommand();
            com.CommandText = cmdText;
            return com;
        }
        public DbCommand GetDbCommand(string cmdText, DbConnection connection)
        {
            DbCommand com = GetDbCommand(cmdText);
            com.Connection = GetDbConnection();
            return com;
        }
        public DbCommand GetDbCommand(string cmdText, DbConnection connection, bool transactionIsEnable)
        {
            DbCommand com = GetDbCommand(cmdText, connection);
            if (transactionIsEnable)
            {
                com.Transaction = GetDbTransaction(connection);
            }
            return com;
        }
        //事务
        public DbTransaction GetDbTransaction(DbConnection connection)
        {
            DbTransaction tran = connection.BeginTransaction();
            return tran;
        }
        //适配器
        public DbDataAdapter GetDataAdapter()
        {
            DbDataAdapter da = Instance().CreateDataAdapter();//创建适配器
            return da;
        }
        public DbDataAdapter GetDataAdapter(DbCommand selectCommand)
        {
            DbDataAdapter da = GetDataAdapter();
            da.SelectCommand = selectCommand;
            return da;
        }
        public DbDataAdapter GetDataAdapter(string selectCommandText, DbConnection selectConnection)
        {
            DbCommand selectCommand = GetDbCommand(selectCommandText, selectConnection);//创建命令
            DbDataAdapter da = GetDataAdapter(selectCommand);
            return da;
        }
        public DbDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            DbConnection con = GetDbConnection(selectConnectionString);
            DbCommand selectCommand = GetDbCommand(selectCommandText, con);
            DbDataAdapter da = GetDataAdapter(selectCommand);
            return da;
        }
        //public  DbDataReader GetDataReader()
        //{
        //}
        //参数
        public DbParameter GetParameter()
        {
            DbParameter par = Instance().CreateParameter();
            return par;
        }
        public DbParameter[] GetParameters(int length)
        {
            IList<DbParameter> liDbPar = new List<DbParameter>();
            for (int i = 0; i < length; i++)
            {
                liDbPar.Add(GetParameter());
            }
            DbParameter[] dbPar = new DbParameter[length];
            liDbPar.CopyTo(dbPar, 0);
            return dbPar;
        }
        public DbParameter GetParameter(string parameterName, Object value)
        {
            DbParameter par = GetParameter();
            par.ParameterName = parameterName;
            par.Value = value;
            return par;
        }
        public DbParameter GetParameter(string parameterName, DbType dbType)
        {
            DbParameter par = GetParameter();
            par.ParameterName = parameterName;
            par.DbType = dbType;
            return par;
        }
        public DbParameter GetParameter(string parameterName, DbType dbType, int size)
        {
            DbParameter par = GetParameter(parameterName, dbType);
            par.Size = size;
            return par;
        }
        public DbParameter GetParameter(string parameterName, DbType dbType, int size, string sourceColumn)
        {
            DbParameter par = GetParameter(parameterName, dbType, size);
            par.SourceColumn = sourceColumn;
            return par;
        }
        #endregion

        #region 参数
        public void attachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (commandParameters != null)
            {
                command.Parameters.Clear();//清理参数
                command.Parameters.AddRange(commandParameters);
            }
        }
        public DbParameter[] createParameters(Dictionary<string, string> dic)
        {
            DbParameter[] par = null;
            if (dic != null)
            {
                par = GetParameters(dic.Count);
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    par[i].ParameterName = ConvertToCommandTestOrParameterName(kvp.Key);
                    if (string.IsNullOrEmpty(kvp.Value))
                    {
                        par[i].Value = DBNull.Value;
                    }
                    else
                    {
                        par[i].Value = kvp.Value;
                    }
                    i++;
                }
            }
            return par;
        }

        public void PrepareCommand(CommandType commandType, string commandText, DbParameter[] commandParameters, bool EnableTransaction)
        {

            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            Command.CommandText = ConvertToCommandTestOrParameterName(commandText);//转换Sql
            Command.CommandType = commandType;
            // 启动事务

            if (EnableTransaction)
            {
                Open();
                Command.Transaction = Transaction;
            }
            if (commandParameters != null)
            {
                attachParameters(Command, commandParameters);
            }
        }
        //获得符合数据类型的参数 格式 sql server @{} oledb ?{} oracle :{} mysql ?{}
        public string GetParameterMarkerFormat()
        {
            string parFormat = null;
            if (Instance().GetType().ToString().Equals("System.Data.SqlClient.SqlClientFactory"))
            {
                parFormat = "@{0}";
                return parFormat;
            }
            if (Instance().GetType().ToString().Equals("System.Data.SQLite.SQLiteFactory"))
            {
                parFormat = "@{0}";
                return parFormat;
            }
            if (Instance().GetType().ToString().Equals("MySql.Data.MySqlClient.MySqlClientFactory"))
            {
                parFormat = "?{0}";
            }
            else
            {
                DataTable dt;
                Open();
                dt = Connection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
                Close();
                parFormat = (string)dt.Rows[0]["ParameterMarkerFormat"];
            }
            return parFormat;
        }
        //获得参数标记名 @,?,:等
        public char GetparameterMarker()
        {
            return GetParameterMarkerFormat().ToCharArray()[0];
        }
        //加工commandText和参数,任何数据提供程序都要用@来形成参数,在这里我们可以根据具体情况来重新生成新的参数和sql语句
        public string ConvertToCommandTestOrParameterName(string str)
        {
            return str.Replace('@', GetparameterMarker());
        }
        ////根据参数名,转化为parameter格式
        //public string GetParameterMarker(string parameterName)
        //{
        //    return string.Format(GetParameterMarkerFormat(), parameterName);
        //}
        #endregion

        #region 连接打开 Open
        public void Open()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 连接关闭 Close
        public void Close()
        {
            try
            {
                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ExecuteNonQuery命令
        public abstract int ExecuteNonQuery(string commandText);

        public int ExecuteNonQuery(CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(commandType, commandText, createParameters(commandParameters), false);
            return ExecuteNonQuery(commandText);
        }

        public int ExecuteNonQuery(string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), false);
            return ExecuteNonQuery(commandText);
        }

        public int ExecuteNonQuery(bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(commandType, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteNonQuery(commandText);
        }

        public int ExecuteNonQuery(bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteNonQuery(commandText);
        }

        public int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(commandType, commandText, createParameters(commandParameters), false);
            return ExecuteNonQuery(commandText);
        }

        public int ExecuteNonQuery(string connectionString, bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(commandType, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteNonQuery(commandText);
        }

        public int ExecuteNonQuery(string connectionString, bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteNonQuery(commandText);
        }

        #endregion

        #region ExecuteScalar命令
        public object ExecuteScalar(CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {

            PrepareCommand(commandType, commandText, createParameters(commandParameters), false);
            return ExecuteScalar(commandText);
        }

        public abstract object ExecuteScalar(string commandText);

        public object ExecuteScalar(string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), false);
            return ExecuteScalar(commandText);
        }

        public object ExecuteScalar(bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(commandType, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteScalar(commandText);
        }

        public object ExecuteScalar(bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteScalar(commandText);
        }

        public object ExecuteScalar(string connectionString, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(commandType, commandText, createParameters(commandParameters), false);
            return ExecuteScalar(commandText);
        }

        public object ExecuteScalar(string connectionString, bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(commandType, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteScalar(commandText);
        }

        public object ExecuteScalar(string connectionString, bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteScalar(commandText);
        }

        #endregion

        #region ExecuteReader命令
        public DbDataReader ExecuteReader(CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(commandType, commandText, createParameters(commandParameters), false);
            return ExecuteReader(commandText);
        }

        public abstract DbDataReader ExecuteReader(string commandText);

        public DbDataReader ExecuteReader(string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), false);
            return ExecuteReader(commandText);
        }

        public DbDataReader ExecuteReader(bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(commandType, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteReader(commandText);
        }

        public DbDataReader ExecuteReader(bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters)
        {
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteReader(commandText);
        }

        public DbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(commandType, commandText, createParameters(commandParameters), false);
            return ExecuteReader(commandText);
        }

        public DbDataReader ExecuteReader(string connectionString, bool EnableTransaction, CommandType commandType, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(commandType, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteReader(commandText);
        }

        public DbDataReader ExecuteReader(string connectionString, bool EnableTransaction, string commandText, Dictionary<string, string> commandParameters)
        {
            ConnectionString = connectionString;
            PrepareCommand(CommandType.Text, commandText, createParameters(commandParameters), EnableTransaction);
            return ExecuteReader(commandText);
        }

        #endregion

        #region Fill命令
        public void Fill(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames,
        Dictionary<string, string> commandParameter)
        {
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            using (DataAdapter)
            {
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    StringBuilder tableMappingName = new StringBuilder(tableName);
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");

                        DataAdapter.TableMappings.Add(tableMappingName.ToString(), tableNames[index]);
                        tableMappingName.Insert(5, (index + 1).ToString());

                    }
                }
                PrepareCommand(commandType, commandText, createParameters(commandParameter), false);
                DataAdapter.SelectCommand = Command;
                string par = "";
                if (commandParameter != null)
                {
                    foreach (KeyValuePair<string, string> kvp in commandParameter)
                    {
                        par += kvp.ToString();
                    }
                }
                DataAdapter.Fill(dataSet);
            }
            Close();
        }

        public void Fill(string commandText, DataSet dataSet)
        {
            Fill(CommandType.Text, commandText, dataSet, null, null);
        }

        public void Fill(string commandText, DataSet dataSet, string[] tableNames,
        Dictionary<string, string> commandParameter)
        {
            Fill(CommandType.Text, commandText, dataSet, tableNames, commandParameter);
        }

        public void Fill(string commandText, DataSet dataSet, string[] tableNames)
        {
            Fill(CommandType.Text, commandText, dataSet, tableNames, new Dictionary<string, string>());
        }

        #endregion

        #region Update命令
        public void Update(DbCommand insertCommand, DbCommand deleteCommand, DbCommand updateCommand, DataSet dataSet, string tableName)
        {
            using (DataAdapter)
            {
                if (insertCommand != null)
                {
                    DataAdapter.InsertCommand = insertCommand;
                }
                if (deleteCommand != null)
                {
                    DataAdapter.DeleteCommand = deleteCommand;
                }
                if (updateCommand != null)
                {
                    DataAdapter.UpdateCommand = updateCommand;
                }

                DataAdapter.Update(dataSet, tableName);
                dataSet.AcceptChanges();
            }
        }
        public void Update(string updateCommandText, Dictionary<string, string> commandParameters)
        {
            ExecuteNonQuery(updateCommandText, commandParameters);
        }

        #endregion

        #region 数据库常用操作
        //获得数据库当前时间      
        public String GetDBNow()
        {
            String dbNowTime = " Getdate() ";
            switch (Instance().GetType().ToString())
            {
                case "System.Data.SqlClient.SqlClientFactory":
                    dbNowTime = "Getdate()";
                    break;
                case "System.Data.OleDb.OleDbFactory ":
                    dbNowTime = DateTime.Now.ToString();
                    break;
                case "System.Data.OracleClient":
                    dbNowTime = " SYSDATE ";
                    break;
                case "MySql.Data.MySqlClient.MySqlClientFactory":
                    dbNowTime = " NOW() ";
                    break;
            }
            string sql = "select " + dbNowTime + " as DbNow";
            return ExecuteScalar(sql).ToString();
        }
        //获得递增最大值，用于实现订单
        public int GetMaxID(string columnName, string tableName)
        {
            columnName = columnName.Replace("'", "");
            tableName = tableName.Replace("'", "");
            string sql = "select max(" + columnName + ")+1 from " + tableName;
            int maxId = Convert.ToInt32(ExecuteScalar(sql));
            return maxId;
        }
        #endregion

        #region 测试数据库
        public bool TestConnection()
        {
            bool status = false;
            try
            {
                Open();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                try
                {
                    Close();
                }
                catch
                { }
            }
            return status;
        }
        #endregion
    }
}
