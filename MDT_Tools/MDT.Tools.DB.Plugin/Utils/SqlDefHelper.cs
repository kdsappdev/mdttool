namespace MDT.Tools.DB.Plugin.Utils
{
    public static class SqlDefHelper
    {
        public static string GetTableNames(string dbType)
        {
            string sql = "";
            switch (dbType)
            {
                case "Oracle":
                    sql = OraclePlSqlDefHelper.QueryAllTableInfo;
                    break;
                case "Sql Server":
                    sql = SqlServerTlSqlDefHelper.QueryAllTableInfo;
                    break;


            }
            return sql;
        }
        public static string GetTableColumnNames(string dbType)
        {
            string sql = "";
            switch (dbType)
            {
                case "Oracle":
                    sql = OraclePlSqlDefHelper.QueryTableColumnInfo;
                    break;
                case "Sql Server":
                    sql = SqlServerTlSqlDefHelper.QueryTableColumnInfo;
                    break;
            }
            return sql;
        }
        public static string GetAllTablePrimaryKeys(string dbType)
        {
            string sql = "";
            switch (dbType)
            {
                case "Oracle":
                    sql = OraclePlSqlDefHelper.QueryAllTablePrimaryKeyInfo;
                    break;
                case "Sql Server":
                    sql = SqlServerTlSqlDefHelper.QueryAllTablePrimaryKeyInfo;
                    break;
            }
            return sql;
        }
    }
    #region Oracle Sql
    internal static class OraclePlSqlDefHelper
    {
        //public const string QueryAllTableInfo = "select a.table_name name,b.comments, b.table_type type from user_tables a inner join user_tab_comments b on a.table_name=b.table_name order by a.table_name";
        public const string QueryAllTableInfo = "SELECT   a.object_name NAME, b.comments, b.table_type TYPE     FROM user_objects a INNER JOIN user_tab_comments b          ON a.object_name = b.table_name WHERE object_type IN('TABLE','VIEW') ORDER BY a.object_name";
        public const string QueryTableColumnInfo = "SELECT a.table_name,a.column_name,a.data_type,a.data_length,a.nullable,a.data_scale,a.data_precision,a.data_default,a.column_id,b.comments  FROM user_tab_columns a INNER JOIN user_col_comments b       ON a.table_name = b.table_name          AND a.column_name = b.column_name where a.table_name=@tableName";
        public const string QueryAllTablePrimaryKeyInfo = "select cu.*, au.constraint_type from user_cons_columns cu, user_constraints au where cu.constraint_name = au.constraint_name";
    }



    #endregion

    #region Sql Server
    internal static class SqlServerTlSqlDefHelper
    {
        public const string QueryAllTableInfo = "select a.name ,b.value comments,'TABLE' type from sysobjects a left join sys.extended_properties b on a.id=b.major_id and b.minor_id=0 where a.type='u'";
        public const string QueryTableColumnInfo = "select b.name table_name, a.name column_name,d.name data_type,a.length data_length, nullable = case when a.isnullable=1 then 'Y' else 'N' end,a.xscale data_scale,a.prec data_precision,a.cdefault data_default,a.colorder column_id, c.value comments from syscolumns a inner join sysobjects b on a.id=b.id inner join sys.systypes d on a.xusertype=d.xusertype left join sys.extended_properties c on a.id=c.major_id and a.colid=c.minor_id where  b.type='u' and b.name=@tableName";
        public const string QueryAllTablePrimaryKeyInfo = @"select e.name ower, b.name constraint_name,d.name table_name,c.name column_name,a.keyno position from sysindexkeys a , sysindexes b ,syscolumns c , sysobjects d ,sysusers e where

a.id=b.id and a.indid=b.indid and b.indid>0 and b.indid<255 and (b.status&2048 <>0) and a.id=c.id and a.colid=c.colid and c.id=d.id and d.uid=e.uid and d.type='u'";
    }
    #endregion
}
